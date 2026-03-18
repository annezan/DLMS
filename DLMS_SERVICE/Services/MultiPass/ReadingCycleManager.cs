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
        var commandRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionCommandRepository>();
        var compteurEquipementUtilities = scope.ServiceProvider.GetRequiredService<CompteurEquipementUtilities>();

        // Load all active meters
        var allMeters = await compteurEquipementUtilities.GetCompteurEquipement();
        if (allMeters == null || allMeters.Count == 0)
        {
            _logger.LogWarning("Aucun compteur trouve pour la session de lecture");
            return (null!, new List<CompteurEquipement>());
        }

        // Close any active cycle (1 cycle = 1 session horaire, pas de multi-session)
        var activeCycle = await cycleRepo.GetActiveCycleAsync();
        if (activeCycle != null)
        {
            _logger.LogInformation(
                "Fermeture cycle #{CycleId} (session precedente) — {Read}/{Total} compteurs lus",
                activeCycle.Id, activeCycle.CompteursLus, activeCycle.TotalCompteurs);

            activeCycle.Statut = ReadingCycleStatus.Termine;
            activeCycle.DateFin = DateTime.Now;
            activeCycle.UpdatedAt = DateTime.Now;
            await commandRepo.UpdateCycleAsync(activeCycle);
        }

        // Create new cycle — 1 session avec TOUS les compteurs
        // Les profils sont lus en incremental (MeterProfileReadHistory),
        // donc relire un compteur deja lu est quasi-gratuit.
        var newCycle = new ReadingCycle
        {
            Libelle = $"Session {DateTime.Now:yyyy-MM-dd HH:mm}",
            DateDebut = DateTime.Now,
            Statut = ReadingCycleStatus.EnCours,
            TotalCompteurs = allMeters.Count,
            CompteursLus = 0,
            MaxSessions = 1,
            SessionActuelle = 0,
            CreatedAt = DateTime.Now,
            CreatedBy = "system"
        };

        newCycle = await commandRepo.AddCycleAsync(newCycle);

        _logger.LogInformation(
            "Nouvelle session #{CycleId} — {Total} compteurs (tous, lecture incrementale)",
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

        // 5. Close cycle (1 cycle = 1 session horaire)
        cycle.CompteursLus = report.TotalSucceeded;
        cycle.SessionActuelle = 1;
        cycle.Statut = ReadingCycleStatus.Termine;
        cycle.DateFin = DateTime.Now;
        cycle.UpdatedAt = DateTime.Now;
        await commandRepo.UpdateCycleAsync(cycle);

        _logger.LogInformation(
            "Session #{CycleId} terminee : {Read}/{Total} compteurs lus ({Rate:F1}%)",
            cycle.Id, report.TotalSucceeded, cycle.TotalCompteurs, report.TauxReussite);
    }
}
