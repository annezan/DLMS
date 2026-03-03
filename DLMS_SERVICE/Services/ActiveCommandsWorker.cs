using DLMS_MODELS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services
{
    public class ActiveCommandsWorker : BackgroundService
    {
        private readonly ILogger<ActiveCommandsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Vérification toutes les minutes
        private readonly IIPWorkerService _workerService;

        public ActiveCommandsWorker(
            ILogger<ActiveCommandsWorker> logger,
            IServiceProvider serviceProvider,
            IIPWorkerService workerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _workerService = workerService ?? throw new ArgumentNullException(nameof(workerService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=== Démarrage du ActiveCommandsWorker ===");
            _logger.LogInformation("Mode: Traitement des commandes utilisateur actives");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    _logger.LogDebug("Cycle de traitement des commandes à {Time}", now);

                    await EnqueueActiveCommandsAsync(now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du cycle de traitement des commandes");
                }

                try
                {
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Arrêt du ActiveCommandsWorker demandé");
                    break;
                }
            }

            _logger.LogInformation("=== Arrêt du ActiveCommandsWorker ===");
        }

        private async Task EnqueueActiveCommandsAsync(DateTime now)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var commandProcessorService = scope.ServiceProvider.GetRequiredService<IDLMSCommandProcessorService>();
                
                var activeCommands = await commandProcessorService.GetActiveCommandsAsync();
                
                if (activeCommands == null || activeCommands.Count == 0)
                {
                    return; // Pas de logs si tout est OK
                }

                _logger.LogInformation("⚡ Enqueue de {Count} commandes actives", activeCommands.Count);

                // Envoyer chaque commande dans la queue prioritaire
                var tasks = activeCommands
                    .Where(c => !string.IsNullOrEmpty(c.AdresseIp) && c.AdresseIp != null)
                    .Select(command => _workerService.EnqueueCommandAsync(command))
                    .ToList();

                await Task.WhenAll(tasks);
                
                _logger.LogInformation("✅ {Count} commandes en queue avec priorité haute", activeCommands.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enqueue des commandes");
            }
        }
    }
}
