using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSMetricsService
    {
        void RecordMeterRead(string ip, string meterSerial, TimeSpan duration, bool success);
        void RecordSessionCreation(string ip, TimeSpan duration, bool success);
        void RecordQueueWait(string ip, JobType jobType, TimeSpan waitTime);
        Task<PerformanceReport> GetPerformanceReportAsync();
        Task LogCurrentStatusAsync();
    }

    public class PerformanceReport
    {
        public int TotalMeters { get; set; }
        public int SuccessfulReads { get; set; }
        public int FailedReads { get; set; }
        public TimeSpan AverageReadTime { get; set; }
        public TimeSpan MaxReadTime { get; set; }
        public TimeSpan MinReadTime { get; set; }
        public Dictionary<string, IPPerformance> IPStats { get; set; } = new();
        public int JobsInQueue { get; set; }
        public int ActiveWorkers { get; set; }
    }

    public class IPPerformance
    {
        public string IP { get; set; }
        public int MetersCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public TimeSpan AverageReadTime { get; set; }
        public TimeSpan TotalReadTime { get; set; }
        public TimeSpan MaxReadTime { get; set; }
        public TimeSpan MinReadTime { get; set; }
        public double SuccessRate => MetersCount > 0 ? (double)SuccessCount / MetersCount * 100 : 0;
    }

    public class DLMSMetricsService : IDLMSMetricsService
    {
        private readonly ConcurrentDictionary<string, List<MeterReadMetric>> _readMetrics = new();
        private readonly ConcurrentDictionary<string, List<SessionMetric>> _sessionMetrics = new();
        private readonly ConcurrentDictionary<string, List<QueueMetric>> _queueMetrics = new();
        private readonly ILogger<DLMSMetricsService> _logger;

        public DLMSMetricsService(ILogger<DLMSMetricsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RecordMeterRead(string ip, string meterSerial, TimeSpan duration, bool success)
        {
            var metrics = _readMetrics.GetOrAdd(ip, _ => new List<MeterReadMetric>());
            
            lock (metrics)
            {
                metrics.Add(new MeterReadMetric
                {
                    Timestamp = DateTime.Now,
                    MeterSerial = meterSerial,
                    Duration = duration,
                    Success = success
                });
                
                // Garder seulement les 1000 dernières lectures par IP
                if (metrics.Count > 1000)
                {
                    metrics.RemoveAt(0);
                }
            }
        }

        public void RecordSessionCreation(string ip, TimeSpan duration, bool success)
        {
            var metrics = _sessionMetrics.GetOrAdd(ip, _ => new List<SessionMetric>());
            
            lock (metrics)
            {
                metrics.Add(new SessionMetric
                {
                    Timestamp = DateTime.Now,
                    Duration = duration,
                    Success = success
                });
                
                if (metrics.Count > 100)
                {
                    metrics.RemoveAt(0);
                }
            }
        }

        public void RecordQueueWait(string ip, JobType jobType, TimeSpan waitTime)
        {
            var key = $"{ip}:{jobType}";
            var metrics = _queueMetrics.GetOrAdd(key, _ => new List<QueueMetric>());
            
            lock (metrics)
            {
                metrics.Add(new QueueMetric
                {
                    Timestamp = DateTime.Now,
                    JobType = jobType,
                    WaitTime = waitTime
                });
                
                if (metrics.Count > 500)
                {
                    metrics.RemoveAt(0);
                }
            }
        }

        public async Task<PerformanceReport> GetPerformanceReportAsync()
        {
            var report = new PerformanceReport();
            var allReads = new List<MeterReadMetric>();

            // Agréger les métriques par IP
            foreach (var kvp in _readMetrics)
            {
                var ip = kvp.Key;
                var metrics = kvp.Value;
                
                lock (metrics)
                {
                    allReads.AddRange(metrics);
                    
                    var ipPerf = new IPPerformance
                    {
                        IP = ip,
                        MetersCount = metrics.Count,
                        SuccessCount = metrics.Count(m => m.Success),
                        FailureCount = metrics.Count(m => !m.Success),
                        TotalReadTime = TimeSpan.FromTicks(metrics.Sum(m => m.Duration.Ticks)),
                        MaxReadTime = metrics.Any() ? metrics.Max(m => m.Duration) : TimeSpan.Zero,
                        MinReadTime = metrics.Any() ? metrics.Min(m => m.Duration) : TimeSpan.Zero
                    };
                    
                    if (metrics.Count > 0)
                    {
                        ipPerf.AverageReadTime = TimeSpan.FromTicks((long)metrics.Average(m => m.Duration.Ticks));
                    }
                    
                    report.IPStats[ip] = ipPerf;
                }
            }

            // Calculer les statistiques globales
            report.TotalMeters = allReads.Count;
            report.SuccessfulReads = allReads.Count(m => m.Success);
            report.FailedReads = allReads.Count(m => !m.Success);
            
            if (allReads.Any())
            {
                report.AverageReadTime = TimeSpan.FromTicks((long)allReads.Average(m => m.Duration.Ticks));
                report.MaxReadTime = allReads.Max(m => m.Duration);
                report.MinReadTime = allReads.Min(m => m.Duration);
            }

            return report;
        }

        public async Task LogCurrentStatusAsync()
        {
            var report = await GetPerformanceReportAsync();
            
            _logger.LogInformation("📊 === RAPPORT DE PERFORMANCE DLMS ===");
            _logger.LogInformation("📈 Compteurs traités: {Total} (✅ {Success} / ❌ {Failed})", 
                report.TotalMeters, report.SuccessfulReads, report.FailedReads);
            
            if (report.TotalMeters > 0)
            {
                var successRate = (double)report.SuccessfulReads / report.TotalMeters * 100;
                _logger.LogInformation("🎯 Taux de succès: {SuccessRate:F1}%", successRate);
                _logger.LogInformation("⏱️ Temps moyen: {AvgMs:F0}ms (min: {MinMs:F0}ms / max: {MaxMs:F0}ms)", 
                    report.AverageReadTime.TotalMilliseconds, 
                    report.MinReadTime.TotalMilliseconds, 
                    report.MaxReadTime.TotalMilliseconds);
            }

            // Top 5 des IP les plus lentes
            var slowIPs = report.IPStats
                .Where(kvp => kvp.Value.MetersCount > 0)
                .OrderByDescending(kvp => kvp.Value.AverageReadTime)
                .Take(5);

            if (slowIPs.Any())
            {
                _logger.LogInformation("🐌 Top 5 IP les plus lentes:");
                foreach (var kvp in slowIPs)
                {
                    var ip = kvp.Value;
                    _logger.LogInformation("   {IP}: {AvgMs:F0}ms avg ({SuccessRate:F1}% succès, {Count} compteurs)", 
                        ip.IP, ip.AverageReadTime.TotalMilliseconds, ip.SuccessRate, ip.MetersCount);
                }
            }

            // Top 5 des IP avec le plus d'échecs
            var failingIPs = report.IPStats
                .Where(kvp => kvp.Value.FailureCount > 0)
                .OrderByDescending(kvp => kvp.Value.FailureCount)
                .Take(5);

            if (failingIPs.Any())
            {
                _logger.LogInformation("❌ Top 5 IP avec le plus d'échecs:");
                foreach (var kvp in failingIPs)
                {
                    var ip = kvp.Value;
                    _logger.LogInformation("   {IP}: {Failures} échecs ({SuccessRate:F1}% succès)", 
                        ip.IP, ip.FailureCount, ip.SuccessRate);
                }
            }

            _logger.LogInformation("🏁 === FIN DU RAPPORT ===");
        }

        private class MeterReadMetric
        {
            public DateTime Timestamp { get; set; }
            public string MeterSerial { get; set; }
            public TimeSpan Duration { get; set; }
            public bool Success { get; set; }
        }

        private class SessionMetric
        {
            public DateTime Timestamp { get; set; }
            public TimeSpan Duration { get; set; }
            public bool Success { get; set; }
        }

        private class QueueMetric
        {
            public DateTime Timestamp { get; set; }
            public JobType JobType { get; set; }
            public TimeSpan WaitTime { get; set; }
        }
    }
}
