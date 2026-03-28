using System.Collections.Concurrent;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;

namespace DLMS_SERVICE.Services.MultiPass;

public class MeterReadOutcome
{
    public string Serial { get; set; } = "";
    public string Ip { get; set; } = "";
    public string Port { get; set; } = "";
    public int CompteurEquipementId { get; set; }
    public bool Success { get; set; }
    public string Error { get; set; } = "";
    public long KeysRetrievalMs { get; set; }
    public long HdlcMs { get; set; }
    public long ReadMs { get; set; }
    public long TotalMs { get; set; }
    public int TimeoutApplied { get; set; }
    public MeterReadingResult ResultCategory { get; set; } = MeterReadingResult.NonTraite;
    public List<ProfileReadResult> ProfileResults { get; set; } = new();
    public int ProfilesInserted => ProfileResults.Where(p => p.Success).Sum(p => p.RowsRead);
}

public class ProfileReadResult
{
    public string ProfileObis { get; set; } = "";
    public bool Success { get; set; }
    public int RowsRead { get; set; }
    public long DurationMs { get; set; }
    public string Error { get; set; } = "";
}

public class PassResult
{
    public int PassNumber { get; set; }
    public ConcurrentBag<MeterReadOutcome> Results { get; } = new();
    public ConcurrentBag<CompteurEquipement> DeferredMeters { get; } = new();
    public ConcurrentDictionary<string, byte> DeferredIps { get; } = new();
    public long ElapsedMs { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }

    public int Succeeded => Results.Count(r => r.Success);
    public int Failed => Results.Count(r => !r.Success);
    public int DeferredCount => DeferredMeters.Count;
    public int InScope => Results.Count + DeferredCount;
    public int TotalProfilesInserted => Results.Sum(r => r.ProfilesInserted);

    public void DeferIp(string ipKey) => DeferredIps.TryAdd(ipKey, 0);
    public void DeferMeter(CompteurEquipement m) => DeferredMeters.Add(m);
    public HashSet<string> GetDeferredIpSet() => DeferredIps.Keys.ToHashSet();
}

public class ReadSessionReport
{
    public int SessionNumber { get; set; }
    public int CycleId { get; set; }
    public List<PassResult> Passes { get; set; } = new();
    public long TotalElapsedMs { get; set; }
    public long TotalReadingMs { get; set; }
    public long TotalPauseMs { get; set; }
    public int TotalMetersInScope { get; set; }
    public int TotalIps { get; set; }
    public int IpsAccessibles { get; set; }

    public int TotalSucceeded => Passes.Sum(p => p.Succeeded);
    public int TotalFailed => TotalMetersInScope - TotalSucceeded;
    public double TauxReussite => TotalMetersInScope > 0 ? (double)TotalSucceeded / TotalMetersInScope * 100 : 0;

    public List<MeterReadOutcome> AllResults => Passes.SelectMany(p => p.Results).ToList();

    public Dictionary<string, List<string>> UnreadByReason { get; set; } = new();
}

public class ConcentratorStats
{
    private readonly ConcurrentDictionary<string, IpStats> _stats = new();

    public void Record(string ipKey, bool success, long latencyMs)
    {
        var stats = _stats.GetOrAdd(ipKey, _ => new IpStats());
        Interlocked.Increment(ref stats.TotalAttempts);
        Interlocked.Add(ref stats.TotalLatencyMs, latencyMs);

        if (success)
        {
            Interlocked.Increment(ref stats.Successes);
            Interlocked.Exchange(ref stats.ConsecutiveFailures, 0);
        }
        else
        {
            Interlocked.Increment(ref stats.Failures);
            Interlocked.Increment(ref stats.ConsecutiveFailures);
        }
    }

    public (double successRate, double avgLatencyMs, int total) GetGlobalStats()
    {
        int totalAttempts = 0, totalSuccesses = 0;
        long totalLatency = 0;

        foreach (var kv in _stats)
        {
            totalAttempts += Volatile.Read(ref kv.Value.TotalAttempts);
            totalSuccesses += Volatile.Read(ref kv.Value.Successes);
            totalLatency += Volatile.Read(ref kv.Value.TotalLatencyMs);
        }

        var rate = totalAttempts > 0 ? (double)totalSuccesses / totalAttempts * 100 : 0;
        var avgLat = totalAttempts > 0 ? (double)totalLatency / totalAttempts : 0;
        return (rate, avgLat, totalAttempts);
    }

    public Dictionary<string, IpStats> GetAllStats() =>
        _stats.ToDictionary(kv => kv.Key, kv => kv.Value);

    public class IpStats
    {
        public int TotalAttempts;
        public int Successes;
        public int Failures;
        public int ConsecutiveFailures;
        public long TotalLatencyMs;

        public double SuccessRate => TotalAttempts > 0 ? (double)Successes / TotalAttempts * 100 : 0;
        public double AvgLatencyMs => TotalAttempts > 0 ? (double)TotalLatencyMs / TotalAttempts : 0;
    }
}

public class AdaptivePool
{
    private readonly SemaphoreSlim _semaphore;
    private int _currentSize;
    private readonly int _minSize;
    private readonly int _maxSize;
    private readonly object _lock = new();

    public AdaptivePool(int initialSize, int minSize = 5, int maxSize = 20)
    {
        _currentSize = initialSize;
        _minSize = minSize;
        _maxSize = maxSize;
        _semaphore = new SemaphoreSlim(initialSize, maxSize);
    }

    public Task WaitAsync() => _semaphore.WaitAsync();
    public void Release() => _semaphore.Release();

    public int CurrentSize
    {
        get { lock (_lock) { return _currentSize; } }
    }

    public void Expand(int delta)
    {
        lock (_lock)
        {
            var actual = Math.Min(delta, _maxSize - _currentSize);
            if (actual > 0)
            {
                _semaphore.Release(actual);
                _currentSize += actual;
            }
        }
    }

    public void Shrink(int delta)
    {
        lock (_lock)
        {
            var actual = Math.Min(delta, _currentSize - _minSize);
            for (int i = 0; i < actual; i++)
            {
                if (_semaphore.Wait(0))
                    _currentSize--;
                else
                    break;
            }
        }
    }
}
