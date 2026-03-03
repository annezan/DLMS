using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services
{
    public class MetricsReportingService : BackgroundService
    {
        private readonly IDLMSMetricsService _metricsService;
        private readonly ILogger<MetricsReportingService> _logger;
        private readonly TimeSpan _reportingInterval = TimeSpan.FromMinutes(5);

        public MetricsReportingService(
            IDLMSMetricsService metricsService,
            ILogger<MetricsReportingService> logger)
        {
            _metricsService = metricsService ?? throw new ArgumentNullException(nameof(metricsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("📊 Démarrage du service de reporting des métriques DLMS");

            // Attendre un peu avant le premier rapport
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("📈 Génération du rapport de performance DLMS...");
                    await _metricsService.LogCurrentStatusAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erreur lors de la génération du rapport de métriques");
                }

                try
                {
                    await Task.Delay(_reportingInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("📊 Arrêt du service de reporting des métriques DLMS");
                    break;
                }
            }
        }
    }
}
