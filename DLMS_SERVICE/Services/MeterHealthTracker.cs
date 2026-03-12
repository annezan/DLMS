using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DLMS_SERVICE.Services
{
    public enum MeterPerformanceCategory
    {
        Fast,       // <30s average
        Medium,     // 30-90s average
        Slow,       // >90s or <50% success
        Failing,    // 3+ consecutive failures
        Unknown     // New/no data
    }

    public class MeterHealthInfo
    {
        public TimeSpan AdaptiveTimeout { get; set; }
        public int WaitTime { get; set; }
        public int RetryCount { get; set; }
        public int PriorityScore { get; set; }
        public MeterPerformanceCategory Category { get; set; }
    }

    public interface IMeterHealthTracker
    {
        void RecordResult(string meterSerial, TimeSpan duration, bool success);
        MeterHealthInfo GetHealthInfo(string meterSerial);
        int GetPriorityScore(string meterSerial);
        MeterPerformanceCategory GetCategory(string meterSerial);
        bool HasAssociationCache(string serial);
    }

    public class MeterHealthTracker : IMeterHealthTracker
    {
        private readonly ConcurrentDictionary<string, MeterStats> _stats = new();
        private readonly ILogger<MeterHealthTracker> _logger;

        public MeterHealthTracker(ILogger<MeterHealthTracker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RecordResult(string meterSerial, TimeSpan duration, bool success)
        {
            if (string.IsNullOrEmpty(meterSerial)) return;

            var stats = _stats.GetOrAdd(meterSerial, _ => new MeterStats());
            lock (stats)
            {
                // Circular buffer of 20 response times
                if (stats.ResponseTimes.Count >= 20)
                    stats.ResponseTimes.Dequeue();
                stats.ResponseTimes.Enqueue(duration);

                // Success tracking over last 50 attempts
                if (stats.Results.Count >= 50)
                    stats.Results.Dequeue();
                stats.Results.Enqueue(success);

                if (success)
                {
                    stats.ConsecutiveFailures = 0;
                    stats.LastSuccessTime = DateTime.Now;
                }
                else
                {
                    stats.ConsecutiveFailures++;
                }

                stats.TotalAttempts++;
            }
        }

        public MeterHealthInfo GetHealthInfo(string meterSerial)
        {
            var category = GetCategory(meterSerial);

            return category switch
            {
                MeterPerformanceCategory.Fast => new MeterHealthInfo
                {
                    AdaptiveTimeout = TimeSpan.FromSeconds(60),
                    WaitTime = 3000,
                    RetryCount = 2,
                    PriorityScore = 1,
                    Category = category
                },
                MeterPerformanceCategory.Medium => new MeterHealthInfo
                {
                    AdaptiveTimeout = TimeSpan.FromSeconds(90),
                    WaitTime = 5000,
                    RetryCount = 2,
                    PriorityScore = 2,
                    Category = category
                },
                MeterPerformanceCategory.Slow => new MeterHealthInfo
                {
                    AdaptiveTimeout = TimeSpan.FromSeconds(45),
                    WaitTime = 3000,
                    RetryCount = 1,
                    PriorityScore = 3,
                    Category = category
                },
                MeterPerformanceCategory.Failing => new MeterHealthInfo
                {
                    AdaptiveTimeout = TimeSpan.FromSeconds(20),
                    WaitTime = 2000,
                    RetryCount = 1,
                    PriorityScore = 4,
                    Category = category
                },
                _ => new MeterHealthInfo // Unknown
                {
                    AdaptiveTimeout = TimeSpan.FromSeconds(120),
                    WaitTime = 5000,
                    RetryCount = 2,
                    PriorityScore = 0,
                    Category = MeterPerformanceCategory.Unknown
                }
            };
        }

        public int GetPriorityScore(string meterSerial)
        {
            return GetHealthInfo(meterSerial).PriorityScore;
        }

        public MeterPerformanceCategory GetCategory(string meterSerial)
        {
            if (string.IsNullOrEmpty(meterSerial))
                return MeterPerformanceCategory.Unknown;

            if (!_stats.TryGetValue(meterSerial, out var stats))
                return MeterPerformanceCategory.Unknown;

            lock (stats)
            {
                if (stats.TotalAttempts == 0)
                    return MeterPerformanceCategory.Unknown;

                // 3+ consecutive failures
                if (stats.ConsecutiveFailures >= 3)
                    return MeterPerformanceCategory.Failing;

                var successRate = stats.Results.Count > 0
                    ? (double)stats.Results.Count(r => r) / stats.Results.Count
                    : 0;

                var avgResponseSeconds = stats.ResponseTimes.Count > 0
                    ? stats.ResponseTimes.Average(t => t.TotalSeconds)
                    : 0;

                // Slow/unstable: >90s average or <50% success
                if (avgResponseSeconds > 90 || successRate < 0.5)
                    return MeterPerformanceCategory.Slow;

                // Fast: <30s average and >80% success
                if (avgResponseSeconds < 30 && successRate > 0.8)
                    return MeterPerformanceCategory.Fast;

                // Medium: everything else
                return MeterPerformanceCategory.Medium;
            }
        }

        public bool HasAssociationCache(string serial)
        {
            if (string.IsNullOrEmpty(serial)) return false;
            var category = GetCategory(serial);
            return category == MeterPerformanceCategory.Fast
                || category == MeterPerformanceCategory.Medium;
        }

        private class MeterStats
        {
            public Queue<TimeSpan> ResponseTimes { get; } = new();
            public Queue<bool> Results { get; } = new();
            public int ConsecutiveFailures { get; set; }
            public int TotalAttempts { get; set; }
            public DateTime? LastSuccessTime { get; set; }
        }
    }
}
