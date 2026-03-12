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
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
        private readonly IIPWorkerService _workerService;
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

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;

                    if (now.Minute == 31)
                    {
                        await RunMultiPassSessionAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du cycle de planification horaire");
                }

                try
                {
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Arret du HourlyReadsWorker demande");
                    break;
                }
            }

            _logger.LogInformation("=== Arret du HourlyReadsWorker ===");
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
