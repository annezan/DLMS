using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE
{
    public class DLMSScheduler : BackgroundService
    {
        private readonly ILogger<DLMSScheduler> _logger;

        public DLMSScheduler(
            ILogger<DLMSScheduler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=== DLMSScheduler est maintenant déprécié ===");
            _logger.LogInformation("Les fonctionnalités ont été déplacées vers les workers dédiés:");
            _logger.LogInformation("- HourlyReadsWorker: lectures horaires");
            _logger.LogInformation("- MissingReadsWorker: lectures manquantes");
            _logger.LogInformation("- ActiveCommandsWorker: commandes actives");

            // Le scheduler ne fait plus rien, tout est géré par les workers dédiés
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
