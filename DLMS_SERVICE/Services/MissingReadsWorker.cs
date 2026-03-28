using DLMS_MODELS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services
{
    public class MissingReadsWorker : BackgroundService
    {
        private readonly ILogger<MissingReadsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Vérification toutes les minutes
        private readonly IIPWorkerService _workerService;
        private readonly IIPJobQueue _jobQueue;

        public MissingReadsWorker(
            ILogger<MissingReadsWorker> logger,
            IServiceProvider serviceProvider,
            IIPWorkerService workerService,
            IIPJobQueue jobQueue)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _workerService = workerService ?? throw new ArgumentNullException(nameof(workerService));
            _jobQueue = jobQueue ?? throw new ArgumentNullException(nameof(jobQueue));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=== Démarrage du MissingReadsWorker ===");
            _logger.LogInformation("Mode: Détection et rattrapage des lectures manquantes");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    _logger.LogDebug("Cycle de détection des lectures manquantes à {Time}", now);

                    // Fenêtre temporelle: minutes 10-25 uniquement
                    // Le HourlyReadsWorker démarre à minute 31, donc pas de conflit
                    if (now.Minute >= 10 && now.Minute <= 25)
                    {
                        await EnqueueMissingReadsAsync(now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du cycle de détection des lectures manquantes");
                }

                try
                {
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Arrêt du MissingReadsWorker demandé");
                    break;
                }
            }

            _logger.LogInformation("=== Arrêt du MissingReadsWorker ===");
        }

        private async Task EnqueueMissingReadsAsync(DateTime now)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var missingReadService = scope.ServiceProvider.GetRequiredService<IDLMSMissingReadService>();
                
                _logger.LogInformation("🔍 Vérification des lectures manquantes à {Time}", now);
                
                var missingReads = await missingReadService.GetMissingReadsAsync(now);
                
                _logger.LogInformation("📊 Résultat de la détection: {Count} lectures manquantes trouvées", 
                    missingReads?.Count ?? 0);
                
                if (missingReads == null || missingReads.Count == 0)
                {
                    _logger.LogInformation("✅ Aucune lecture manquante détectée - Pas de rattrapage nécessaire");
                    return;
                }

                // Exclure les compteurs déjà en queue ou en cours de traitement
                var pendingSerials = _jobQueue.GetPendingMissingSerials();
                var filteredReads = missingReads
                    .Where(m => !pendingSerials.Contains(m.NumeroCompteur))
                    .ToList();

                var excluded = missingReads.Count - filteredReads.Count;
                if (excluded > 0)
                {
                    _logger.LogInformation("⏭️ {Excluded}/{Total} lectures exclues (déjà en queue/cours)", excluded, missingReads.Count);
                }

                if (filteredReads.Count == 0)
                {
                    _logger.LogInformation("✅ Toutes les lectures manquantes sont déjà en cours de traitement");
                    return;
                }

                _logger.LogInformation("🔍 Enqueue de {Count} lectures de rattrapage", filteredReads.Count);

                // Envoyer les lectures manquantes dans la queue prioritaire
                await _workerService.EnqueueMissingReadsAsync(filteredReads);

                _logger.LogInformation("✅ {Count} lectures de rattrapage en queue avec priorité", filteredReads.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enqueue des lectures de rattrapage");
            }
        }
    }
}
