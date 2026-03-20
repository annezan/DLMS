using DLMS_MODELS;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_SERVICE.Services.MultiPass;
using DLMS_UTILITIES;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DLMS_SERVICE.Services
{
    public class HourlyReadsWorker : BackgroundService
    {
        private readonly ILogger<HourlyReadsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIPWorkerService _workerService;
        private readonly TimeSpan _minRestDelay = TimeSpan.FromMinutes(10);
        private volatile bool _sessionInProgress;

        public HourlyReadsWorker(
            ILogger<HourlyReadsWorker> logger,
            IServiceProvider serviceProvider,
            IIPWorkerService workerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _workerService = workerService ?? throw new ArgumentNullException(nameof(workerService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=== Demarrage du HourlyReadsWorker (Multi-Pass) ===");
            _logger.LogInformation("Premiere session immediate, puis a chaque heure pleine (repos min {Rest} en cas de debordement)",
                _minRestDelay);

            while (!stoppingToken.IsCancellationRequested)
            {
                var sessionStart = DateTime.Now;

                try
                {
                    await RunMultiPassSessionAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du cycle de planification horaire");
                }

                try
                {
                    var delay = ComputeDelayUntilNextSession(sessionStart);
                    _logger.LogInformation("Prochaine session a {NextTime:HH:mm} (dans {Delay})",
                        DateTime.Now.Add(delay), delay);
                    await Task.Delay(delay, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Arret du HourlyReadsWorker demande");
                    break;
                }
            }

            _logger.LogInformation("=== Arret du HourlyReadsWorker ===");
        }

        private TimeSpan ComputeDelayUntilNextSession(DateTime sessionStart)
        {
            // Prochain créneau = heure pleine suivant le début de la session
            var nextSlot = new DateTime(sessionStart.Year, sessionStart.Month, sessionStart.Day,
                sessionStart.Hour, 0, 0).AddHours(1);

            var now = DateTime.Now;
            var timeUntilSlot = nextSlot - now;

            if (timeUntilSlot > TimeSpan.Zero)
            {
                // Session finie avant le prochain créneau → attendre le créneau
                return timeUntilSlot;
            }
            else
            {
                // Débordement → repos minimum de 10 min
                return _minRestDelay;
            }
        }

        private async Task RunMultiPassSessionAsync(CancellationToken ct)
        {
            // Guard anti-overlap
            if (_sessionInProgress)
            {
                _logger.LogWarning("Session multi-pass deja en cours — skip");
                return;
            }

            _sessionInProgress = true;
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var cycleManager = scope.ServiceProvider.GetRequiredService<IReadingCycleManager>();
                var orchestrator = scope.ServiceProvider.GetRequiredService<IReadSessionOrchestrator>();
                var reportService = scope.ServiceProvider.GetRequiredService<ISessionReportService>();
                var config = scope.ServiceProvider.GetRequiredService<IOptions<MultiPassConfig>>().Value;

                // 1. Get or create cycle + unread meters
                var (cycle, metersToRead) = await cycleManager.GetOrCreateCycleAsync();

                if (cycle == null || metersToRead.Count == 0)
                {
                    _logger.LogWarning("Aucun compteur a lire pour cette session");
                    return;
                }

                _logger.LogInformation(
                    "Lancement session multi-pass: cycle #{CycleId}, {Count} compteurs, session {SessionNum}",
                    cycle.Id, metersToRead.Count, cycle.SessionActuelle + 1);

                // 2. Run orchestrator
                var report = await orchestrator.RunSessionAsync(
                    metersToRead, config,
                    cycle.SessionActuelle + 1,
                    cycle.Id, ct);

                // 3. Persist results
                await cycleManager.PersistSessionResultsAsync(cycle, report, config);

                // 4. Log technical report
                reportService.LogSessionReport(report);

                _logger.LogInformation(
                    "Session multi-pass terminee: {OK}/{Total} lus ({Rate:F1}%)",
                    report.TotalSucceeded, report.TotalMetersInScope, report.TauxReussite);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Session multi-pass annulee (arret demande)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la session multi-pass");
            }
            finally
            {
                _sessionInProgress = false;
            }
        }
    }
}
