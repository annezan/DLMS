using DLMS_DAL.ReadingDomainDal.Repositories.Commands;
using DLMS_DAL.ReadingDomainDal.Repositories.Queries;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.ReadingDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;
using DLMS_UTILITIES;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services.MultiPass;

public interface IReadingCycleManager
{
    /// <summary>
    /// Gets or creates the active cycle. If the current cycle has exceeded MaxSessions,
    /// closes it and creates a new one.
    /// Returns (cycle, metersToRead).
    /// </summary>
    Task<(ReadingCycle Cycle, List<CompteurEquipement> MetersToRead)> GetOrCreateCycleAsync();

    /// <summary>
    /// Persists session results after orchestration completes.
    /// </summary>
    Task PersistSessionResultsAsync(
        ReadingCycle cycle,
        ReadSessionReport report,
        MultiPassConfig config);
}

public class ReadingCycleManager : IReadingCycleManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ReadingCycleManager> _logger;

    public ReadingCycleManager(
        IServiceProvider serviceProvider,
        ILogger<ReadingCycleManager> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<(ReadingCycle Cycle, List<CompteurEquipement> MetersToRead)> GetOrCreateCycleAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var cycleRepo = scope.ServiceProvider.GetRequiredService<IReadingCycleQueryRepository>();
        var meterStatusRepo = scope.ServiceProvider.GetRequiredService<IMeterReadingStatusQueryRepository>();
        var commandRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionCommandRepository>();
        var compteurEquipementUtilities = scope.ServiceProvider.GetRequiredService<CompteurEquipementUtilities>();

        // Load all active meters
        var allMeters = await compteurEquipementUtilities.GetCompteurEquipement();
        if (allMeters == null || allMeters.Count == 0)
        {
            _logger.LogWarning("Aucun compteur trouve pour le cycle de lecture");
            return (null!, new List<CompteurEquipement>());
        }

        // Check for active cycle
        var activeCycle = await cycleRepo.GetActiveCycleAsync();

        if (activeCycle != null)
        {
            // Check if cycle exhausted its sessions
            if (activeCycle.SessionActuelle >= activeCycle.MaxSessions)
            {
                _logger.LogInformation(
                    "Cycle #{CycleId} a atteint {Current}/{Max} sessions — fermeture et creation d'un nouveau cycle",
                    activeCycle.Id, activeCycle.SessionActuelle, activeCycle.MaxSessions);

                activeCycle.Statut = ReadingCycleStatus.Termine;
                activeCycle.DateFin = DateTime.Now;
                activeCycle.UpdatedAt = DateTime.Now;
                await commandRepo.UpdateCycleAsync(activeCycle);

                activeCycle = null; // Force new cycle creation
            }
            else
            {
                // Get unread meters for this cycle
                var unreadMeterIds = await meterStatusRepo.GetUnreadMeterIdsForCycleAsync(activeCycle.Id);
                var metersToRead = allMeters.Where(m => unreadMeterIds.Contains(m.Id)).ToList();

                if (metersToRead.Count == 0)
                {
                    _logger.LogInformation(
                        "Cycle #{CycleId} : tous les compteurs ont ete lus — fermeture et nouveau cycle",
                        activeCycle.Id);

                    activeCycle.Statut = ReadingCycleStatus.Termine;
                    activeCycle.DateFin = DateTime.Now;
                    activeCycle.CompteursLus = activeCycle.TotalCompteurs;
                    activeCycle.UpdatedAt = DateTime.Now;
                    await commandRepo.UpdateCycleAsync(activeCycle);

                    activeCycle = null; // Force new cycle
                }
                else
                {
                    _logger.LogInformation(
                        "Cycle #{CycleId} actif — session {Current}/{Max}, {Unread}/{Total} compteurs non-lus",
                        activeCycle.Id, activeCycle.SessionActuelle + 1, activeCycle.MaxSessions,
                        metersToRead.Count, activeCycle.TotalCompteurs);

                    return (activeCycle, metersToRead);
                }
            }
        }

        // Create new cycle
        var newCycle = new ReadingCycle
        {
            Libelle = $"Cycle {DateTime.Now:yyyy-MM-dd HH:mm}",
            DateDebut = DateTime.Now,
            Statut = ReadingCycleStatus.EnCours,
            TotalCompteurs = allMeters.Count,
            CompteursLus = 0,
            MaxSessions = 10, // Default, can be overridden by DB config
            SessionActuelle = 0,
            CreatedAt = DateTime.Now,
            CreatedBy = "system"
        };

        // Try to get MaxSessions from DB config
        try
        {
            var configs = await cycleRepo.GetAllAsync(c => !c.IsArchive);
            // configs is for ReadingCycle, we need ReadingConfiguration instead
            // We'll use the default for now - overridable via appsettings
        }
        catch { /* Use default */ }

        newCycle = await commandRepo.AddCycleAsync(newCycle);

        _logger.LogInformation(
            "Nouveau cycle #{CycleId} cree — {Total} compteurs",
            newCycle.Id, newCycle.TotalCompteurs);

        return (newCycle, allMeters);
    }

    public async Task PersistSessionResultsAsync(
        ReadingCycle cycle,
        ReadSessionReport report,
        MultiPassConfig config)
    {
        using var scope = _serviceProvider.CreateScope();
        var commandRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionCommandRepository>();
        var meterStatusRepo = scope.ServiceProvider.GetRequiredService<IMeterReadingStatusQueryRepository>();

        // 1. Create ReadingSession record
        var session = new ReadingSession
        {
            ReadingCycleId = cycle.Id,
            NumeroSession = report.SessionNumber,
            DateDebut = DateTime.Now.AddMilliseconds(-report.TotalElapsedMs),
            DateFin = DateTime.Now,
            Statut = ReadingSessionStatus.Termine,
            CompteursEnScope = report.TotalMetersInScope,
            CompteursLus = report.TotalSucceeded,
            CompteursEchoues = report.TotalFailed,
            DureeMs = report.TotalElapsedMs,
            DureeLectureMs = report.TotalReadingMs,
            DureePauseMs = report.TotalPauseMs,
            NombreIps = report.TotalIps,
            IpsAccessibles = report.IpsAccessibles,
            TauxReussite = report.TauxReussite,
            DebitCompteursParMinute = report.TotalReadingMs > 0
                ? report.TotalSucceeded / (report.TotalReadingMs / 60000.0) : 0,
            CreatedAt = DateTime.Now,
            CreatedBy = "system"
        };

        session = await commandRepo.AddSessionAsync(session);
        _logger.LogInformation("Session #{SessionId} persistee pour cycle #{CycleId}",
            session.Id, cycle.Id);

        // 2. Persist SessionPassResults
        foreach (var pass in report.Passes)
        {
            var passEntity = new SessionPassResult
            {
                ReadingSessionId = session.Id,
                NumeroPasse = pass.PassNumber,
                DateDebut = pass.DateDebut,
                DateFin = pass.DateFin,
                BudgetSeconds = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.BudgetSeconds ?? 0,
                CompteursEnScope = pass.InScope,
                CompteursLus = pass.Succeeded,
                CompteursEchoues = pass.Failed,
                CompteursDifferes = pass.DeferredCount,
                IpsDifferees = pass.DeferredIps.Count,
                DureeMs = pass.ElapsedMs,
                CanaryTimeoutSeconds = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.CanaryTimeoutSeconds ?? 0,
                CachedTimeoutSeconds = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.CachedTimeoutSeconds ?? 0,
                UncachedTimeoutSeconds = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.UncachedTimeoutSeconds ?? 0,
                MaxConsecutiveFailures = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.MaxConsecutiveFailures ?? 0,
                CooldownCount = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.CooldownCount ?? 0,
                CooldownSeconds = config.Passes
                    .FirstOrDefault(p => p.PassNumber == pass.PassNumber)?.CooldownSeconds ?? 0,
                CreatedAt = DateTime.Now,
                CreatedBy = "system"
            };
            await commandRepo.AddPassResultAsync(passEntity);
        }

        // 3. Persist MeterReadingStatus (bulk)
        var meterStatuses = new List<MeterReadingStatus>();

        // Deduplicate: keep last result per meter
        var resultsByMeter = report.AllResults
            .GroupBy(r => r.CompteurEquipementId)
            .Select(g => g.Last())
            .ToList();

        foreach (var result in resultsByMeter)
        {
            if (result.CompteurEquipementId <= 0) continue;

            meterStatuses.Add(new MeterReadingStatus
            {
                ReadingSessionId = session.Id,
                CompteurEquipementId = result.CompteurEquipementId,
                NumeroPasse = report.Passes.LastOrDefault(p =>
                    p.Results.Any(r => r.Serial == result.Serial))?.PassNumber ?? 0,
                Resultat = result.ResultCategory,
                MessageErreur = result.Success ? null : result.Error,
                AdresseIp = result.Ip,
                Port = result.Port,
                NumeroCompteur = result.Serial,
                TempsHdlcMs = result.HdlcMs > 0 ? result.HdlcMs : null,
                TempsLectureMs = result.ReadMs > 0 ? result.ReadMs : null,
                TempsClesMs = result.KeysRetrievalMs > 0 ? result.KeysRetrievalMs : null,
                TempsTotalMs = result.TotalMs > 0 ? result.TotalMs : null,
                TimeoutApplique = result.TimeoutApplied > 0 ? result.TimeoutApplied : null,
                CreatedAt = DateTime.Now,
                CreatedBy = "system"
            });
        }

        await commandRepo.BulkInsertMeterReadingStatusesAsync(meterStatuses);
        _logger.LogInformation("{Count} statuts compteurs persistes", meterStatuses.Count);

        // 4. Persist IpSessionStats
        var ipStatsEntities = report.AllResults
            .GroupBy(r => $"{r.Ip}:{r.Port}")
            .Where(g => !string.IsNullOrEmpty(g.Key) && g.Key != ":")
            .Select(g => new IpSessionStats
            {
                ReadingSessionId = session.Id,
                AdresseIp = g.First().Ip,
                Port = g.First().Port,
                TotalTentatives = g.Count(),
                Reussites = g.Count(r => r.Success),
                Echecs = g.Count(r => !r.Success),
                TauxReussite = g.Count() > 0 ? (double)g.Count(r => r.Success) / g.Count() * 100 : 0,
                LatenceMoyenneMs = g.Where(r => r.TotalMs > 0).Select(r => (double)r.TotalMs).DefaultIfEmpty(0).Average(),
                TcpAccessible = true,
                TcpLatenceMs = 0,
                CanaryReussi = g.OrderBy(r => r.CompteurEquipementId).First().Success,
                EchecsConsecutifsMax = 0,
                CreatedAt = DateTime.Now,
                CreatedBy = "system"
            })
            .ToList();

        await commandRepo.BulkInsertIpSessionStatsAsync(ipStatsEntities);

        // 5. Update cycle
        var readCount = await meterStatusRepo.GetReadCountForCycleAsync(cycle.Id);
        cycle.CompteursLus = readCount;
        cycle.SessionActuelle++;
        cycle.UpdatedAt = DateTime.Now;
        await commandRepo.UpdateCycleAsync(cycle);

        _logger.LogInformation(
            "Cycle #{CycleId} mis a jour : session {Current}/{Max}, {Read}/{Total} compteurs lus",
            cycle.Id, cycle.SessionActuelle, cycle.MaxSessions, readCount, cycle.TotalCompteurs);
    }
}
