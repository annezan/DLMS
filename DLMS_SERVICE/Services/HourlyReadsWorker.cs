using DLMS_MODELS;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_UTILITIES;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services
{
    public class HourlyReadsWorker : BackgroundService
    {
        private readonly ILogger<HourlyReadsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Vérification toutes les minutes
        private readonly IIPWorkerService _workerService;

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
            _logger.LogInformation("=== Démarrage du HourlyReadsWorker ===");
            _logger.LogInformation("Mode: Planification des lectures horaires automatiques");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    _logger.LogDebug("Cycle de planification horaire à {Time}", now);

                    // Exécuter seulement à chaque heure pile (minute 0)
                    if (now.Minute == 31)
                    {
                        await EnqueueHourlyReadsAsync(now);
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
                    _logger.LogInformation("Arrêt du HourlyReadsWorker demandé");
                    break;
                }
            }

            _logger.LogInformation("=== Arrêt du HourlyReadsWorker ===");
        }

        private async Task EnqueueHourlyReadsAsync(DateTime now)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var compteurEquipementUtilities = scope.ServiceProvider.GetRequiredService<CompteurEquipementUtilities>();
                
                var compteurs = await compteurEquipementUtilities.GetCompteurEquipement();
                
                if (compteurs == null || compteurs.Count == 0)
                {
                    _logger.LogWarning("Aucun compteur trouvé pour les lectures horaires");
                    return;
                }

                // Grouper les compteurs par IP pour optimiser les traitements
                var compteursByIp = compteurs
                    .Where(c => c.Equipement?.AdresseIp != null)
                    .GroupBy(c => new { 
                        IP = c.Equipement.AdresseIp, 
                        Port = int.TryParse(c.Equipement.Port, out var port) ? port : 0 
                    })
                    .ToList();

                _logger.LogInformation("📝 Enqueue des lectures horaires pour {Count} groupes IP ({TotalCount} compteurs)", 
                    compteursByIp.Count, compteurs.Count);

                // Envoyer chaque groupe d'IP dans la queue prioritaire avec le cycle start time
                var cycleStartTime = DateTime.Now;
                var tasks = compteursByIp
                    .Select(group => _workerService.EnqueueHourlyReadsAsync(group.ToList(), now, cycleStartTime))
                    .ToList();

                await Task.WhenAll(tasks);
                
                _logger.LogInformation("✅ {Count} groupes de lectures horaires en queue avec priorité", compteursByIp.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enqueue des lectures horaires");
            }
        }
    }
}
