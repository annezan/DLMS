# Multi-Pass DLMS Meter Reading Optimization

**Date:** 2026-03-12
**Status:** Draft
**Scope:** `DLMS_IntegrationTest/Program.cs` (parallel mode)

## Problem

The current single-pass parallel reading achieves 16-45% success rate (50-136/300 meters) in 21-80 minutes. Three root causes account for ~85% of failures:

1. **IP cascade abandonment** (40-65% of failures): 3 consecutive failures → abandon IP → all remaining meters lost
2. **TCP pre-scan too short** (5s): 3-9 IPs fail per run, losing 12-65 meters untried
3. **Sequential per-IP reading**: A concentrator with 22 meters at ~250s each becomes the pacing bottleneck

## Solution

Replace the single-pass approach with a 3-pass strategy within a 45-minute budget (50-minute hard ceiling). Each pass uses progressively relaxed timeouts, and failed meters carry over to the next pass with a 5-minute recovery interval between passes.

## Architecture

```
RunParallelTest()
  |
  +-- Global budget: 50 min hard ceiling (Stopwatch)
  |
  +-- Phase 0: DB Load
  |
  +-- Phase 1: Pass 1 "Fast Harvest" (budget 15 min)
  |     +-- TCP Pre-scan (8s timeout, individual retry fallback)
  |     +-- Canary test per IP (30s) — result recorded as first meter read
  |     +-- IPs OK: read remaining meters, cached first (60s), then uncached (90s)
  |     +-- IPs KO at canary: deferred to Pass 2
  |     +-- Abandon IP after 2 consecutive failures: defer to Pass 2
  |     +-- Output: successful reads + failed/deferred meters list
  |
  +-- Pause 5 min (log intermediate summary)
  |
  +-- Phase 2: Pass 2 "Recovery" (budget 10 min)
  |     +-- TCP re-scan ONLY on deferred/failed IPs from Pass 1
  |     +-- Canary test (45s)
  |     +-- Increased timeouts: cached 90s, uncached 120s
  |     +-- Abandon IP after 3 consecutive failures: defer to Pass 3
  |     +-- 1x 15s cooldown before defer
  |     +-- Output: cumulative successes + remaining failures
  |
  +-- Pause 5 min (log intermediate summary)
  |
  +-- Phase 3: Pass 3 "Last Chance" (budget = min(8 min, globalTimeLeft))
  |     +-- TCP re-scan ONLY on deferred/failed IPs from Pass 2
  |     +-- Canary test (60s)
  |     +-- Generous timeouts: cached 120s, uncached 180s
  |     +-- Abandon IP after 5 consecutive failures (definitive)
  |     +-- 1x 30s cooldown before definitive abandon
  |     +-- Output: final cumulative results
  |
  +-- PrintMultiPassReport() — consolidated 3-pass report
```

## Pass Configuration

| Parameter              | Pass 1         | Pass 2         | Pass 3         |
|------------------------|----------------|----------------|----------------|
| Budget                 | 900s (15 min)  | 600s (10 min)  | 480s (8 min)   |
| Canary timeout         | 30s            | 45s            | 60s            |
| Cached meter timeout   | 60s            | 90s            | 120s           |
| Uncached meter timeout | 90s            | 120s           | 180s           |
| Max consecutive fails  | 2              | 3              | 5              |
| Cooldown count         | 0 (defer immediately) | 1x 15s  | 1x 30s         |
| Pause after            | 300s (5 min)   | 300s (5 min)   | 0              |
| TCP scan timeout       | 8s (hardcoded) | 8s (hardcoded) | 8s (hardcoded) |

## Data Structures

### PassConfig

```csharp
class PassConfig
{
    public int PassNumber;              // 1, 2, 3
    public int BudgetSeconds;           // 900, 600, 480
    public int CanaryTimeoutSeconds;    // 30, 45, 60
    public int CachedTimeoutSeconds;    // 60, 90, 120
    public int UncachedTimeoutSeconds;  // 90, 120, 180
    public int MaxConsecutiveFailures;  // 2, 3, 5
    public int CooldownCount;           // 0, 1, 1
    public int CooldownSeconds;         // 0, 15, 30
    public int PauseAfterSeconds;       // 300, 300, 0

    // Hardcoded: TCP scan timeout is always 8s for all passes
    public const int TcpScanTimeoutSeconds = 8;

    public PassConfig(int passNumber, int budget, int canary, int cached,
        int uncached, int maxFails, int cooldownCount, int cooldownSeconds, int pause)
    {
        PassNumber = passNumber;
        BudgetSeconds = budget;
        CanaryTimeoutSeconds = canary;
        CachedTimeoutSeconds = cached;
        UncachedTimeoutSeconds = uncached;
        MaxConsecutiveFailures = maxFails;
        CooldownCount = cooldownCount;
        CooldownSeconds = cooldownSeconds;
        PauseAfterSeconds = pause;
    }
}
```

### PassBudget

```csharp
class PassBudget
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private readonly int _budgetSeconds;

    public PassBudget(int budgetSeconds) => _budgetSeconds = budgetSeconds;

    public double TimeLeftSeconds => _budgetSeconds - _sw.Elapsed.TotalSeconds;
    public bool IsExpired => TimeLeftSeconds < 30;
    public long ElapsedMs => _sw.ElapsedMilliseconds;

    /// <summary>
    /// Clamp a desired timeout to fit within remaining budget.
    /// Returns -1 if less than 15s remain (signal: no time left).
    /// Also used for canary timeouts.
    /// </summary>
    public int ClampTimeout(int desiredTimeout)
    {
        var maxAllowed = (int)TimeLeftSeconds - 5;
        if (maxAllowed < 15) return -1;
        return Math.Min(desiredTimeout, maxAllowed);
    }
}
```

### PassResult

```csharp
class PassResult
{
    public int PassNumber;
    public List<MeterResult> Results = new();       // meters actually attempted (canary + others)
    public List<MeterInfo> DeferredMeters = new();   // meters NOT attempted: canary fail, IP abandon, budget expired
    public HashSet<string> DeferredIps = new();      // IPs to re-scan at next pass (canary fail, abandon, TCP fail)
    public long ElapsedMs;

    public int Succeeded => Results.Count(r => r.Success);
    public int Failed => Results.Count(r => !r.Success);  // attempted but failed
    public int DeferredCount => DeferredMeters.Count;      // not attempted, carried to next pass
    public int InScope => Results.Count + DeferredCount;   // total meters assigned to this pass
}
```

### MultiPassReport

```csharp
class MultiPassReport
{
    public List<PassResult> Passes = new();
    public long TotalElapsedMs;         // includes pauses
    public long TotalReadingMs;         // excludes pauses
    public long TotalPauseMs;
    public int TotalMeters;

    public int TotalSucceeded => Passes.Sum(p => p.Succeeded);
    public int TotalFailed => TotalMeters - TotalSucceeded;
    public List<MeterResult> AllResults => Passes.SelectMany(p => p.Results).ToList();

    // Failure breakdown for final report
    // Key = failure reason, Value = list of serial numbers
    public Dictionary<string, List<string>> UnreadByReason = new();
}
```

### Failure reason tracking

`MeterResult.Error` already captures the failure reason as a string. For the final report breakdown,
the orchestrator categorizes failures after all passes complete:

```csharp
// After all passes, categorize unread meters
var allAttempted = multiPassReport.AllResults.ToDictionary(r => r.Serial, r => r);
foreach (var meter in allMeters)
{
    if (successfulSerials.Contains(meter.Serial)) continue;
    if (allAttempted.TryGetValue(meter.Serial, out var lastResult))
    {
        var reason = lastResult.Error switch
        {
            var e when e.Contains("TCP") || e.Contains("pre-scan") => "TCP unreachable",
            var e when e.Contains("Abandon") || e.Contains("Ignore") => "Definitive abandon",
            var e when e.Contains("Timeout") => "Timeout",
            var e when e.Contains("Budget") => "Budget expired",
            _ => "Other"
        };
        multiPassReport.UnreadByReason.GetOrAdd(reason, _ => new()).Add(meter.Serial);
    }
    else
    {
        // Never attempted in any pass (e.g., all 3 TCP scans failed for this IP)
        multiPassReport.UnreadByReason.GetOrAdd("TCP unreachable", _ => new()).Add(meter.Serial);
    }
}
```

## Helper: HasCache

```csharp
static bool HasCache(MeterInfo m) =>
    File.Exists(Path.Combine("associations", $"{m.Serial}_Read.xml"));
```

## Canary Test Logic

For each IP at the start of a pass:

1. **Select canary meter**: prefer meter with existing cache XML, otherwise first meter in list
2. **Read canary** with `budget.ClampTimeout(passConfig.CanaryTimeoutSeconds)`
3. **If success**:
   - **Record canary result in `PassResult.Results`** (it counts as a real read)
   - **Remove canary meter from the remaining-meters list for this IP**
   - Record `canaryLatency`
   - Compute adaptive timeout: `clamp(canaryLatency * 2.5, floor=45, ceiling=passConfig.XxxTimeoutSeconds)`
   - Proceed to read remaining meters with adaptive timeout
4. **If failure**:
   - Record canary result in `PassResult.Results` (with error)
   - If Pass 1 or 2: defer entire IP — add remaining meters (excluding canary) to `PassResult.DeferredMeters`, add IP to `PassResult.DeferredIps`
   - If Pass 3: mark as failed, continue to next IP
   - Release pool slot immediately

Adaptive timeout computation:

```csharp
int ComputeAdaptiveTimeout(long canaryLatencyMs, bool hasCacheFile, PassConfig config)
{
    var raw = (int)(canaryLatencyMs / 1000.0 * 2.5);
    var ceiling = hasCacheFile ? config.CachedTimeoutSeconds : config.UncachedTimeoutSeconds;
    return Math.Clamp(raw, 45, ceiling);
}
```

## IP Sorting Strategy

IPs are sorted to maximize throughput per pass:

```csharp
var sortedIps = reachableGroups
    .OrderBy(g => g.Meters.Count <= 5 ? 0 : 1)                 // small groups first (free pool slots fast)
    .ThenByDescending(g => g.Meters.Count(m => HasCache(m)))    // more cached = higher priority
    .ThenBy(g => g.Meters.Count)                                // fewer meters first
    .ToList();
```

Within each IP, meters sorted: cached first, then uncached (already implemented).

## TCP Pre-scan Improvements

Changes from current implementation:

1. **Timeout increased: 5s to 8s** — recovers slow IPs (e.g., 10.60.15.158 with 22 meters)
2. **Individual retry fallback** — IPs that fail parallel scan are retested individually (avoids false negatives from socket saturation)
3. **Re-scan at Pass 2 and 3** — ONLY on IPs in `previousPass.DeferredIps` (not all IPs)

Extracted as reusable helpers:

```csharp
static async Task<List<(string Key, bool Reachable, long Ms)>> ParallelTcpScan(
    IEnumerable<(string Ip, string Port)> targets, TimeSpan timeout)
{
    var tasks = targets.Select(t => Task.Run(async () =>
    {
        var key = $"{t.Ip}:{t.Port}";
        try
        {
            using var client = new TcpClient();
            using var cts = new CancellationTokenSource(timeout);
            var sw = Stopwatch.StartNew();
            await client.ConnectAsync(t.Ip, int.Parse(t.Port), cts.Token);
            return (Key: key, Reachable: true, Ms: sw.ElapsedMilliseconds);
        }
        catch { return (Key: key, Reachable: false, Ms: (long)timeout.TotalMilliseconds); }
    }));
    return (await Task.WhenAll(tasks)).ToList();
}

static async Task<(string Key, bool Reachable, long Ms)> SingleTcpScan(
    string ip, string port, TimeSpan timeout)
{
    var key = $"{ip}:{port}";
    try
    {
        using var client = new TcpClient();
        using var cts = new CancellationTokenSource(timeout);
        var sw = Stopwatch.StartNew();
        await client.ConnectAsync(ip, int.Parse(port), cts.Token);
        return (key, true, sw.ElapsedMilliseconds);
    }
    catch { return (key, false, (long)timeout.TotalMilliseconds); }
}
```

Pass 1 scans all IPs. Pass 2/3 only scan IPs from `previousPass.DeferredIps`:

```csharp
// In RunSinglePass:
IEnumerable<(string Ip, string Port)> ipsToScan;
if (passConfig.PassNumber == 1)
    ipsToScan = allDistinctIps;          // scan everything
else
    ipsToScan = previousDeferredIps;     // only re-scan deferred IPs

// IPs that were reachable in a previous pass and have meters to retry:
// skip scan, go directly to canary
```

## IP Abandonment by Pass

| Pass | Threshold | Cooldown | Action on Threshold |
|------|-----------|----------|---------------------|
| 1    | 2 consecutive failures | None | Defer IP to Pass 2 immediately |
| 2    | 3 consecutive failures | 1x 15s before defer | Defer IP to Pass 3 |
| 3    | 5 consecutive failures | 1x 30s before abandon | Definitive abandon |

Key change: Pass 1 has **zero cooldown**. Dead IPs are ejected in seconds instead of minutes.

Pseudocode for the abandonment logic within each IP's meter loop:

```csharp
int consecutiveFailures = 0;
int cooldownsUsed = 0;

// After each meter read (success or failure):
if (meterResult.Success)
{
    Interlocked.Exchange(ref consecutiveFailures, 0);
}
else
{
    var failures = Interlocked.Increment(ref consecutiveFailures);
    if (failures >= passConfig.MaxConsecutiveFailures)
    {
        if (cooldownsUsed < passConfig.CooldownCount)
        {
            // Cooldown then retry
            Console.WriteLine($"    [{ip}] Cooldown {passConfig.CooldownSeconds}s...");
            await Task.Delay(TimeSpan.FromSeconds(passConfig.CooldownSeconds));
            Interlocked.Exchange(ref consecutiveFailures, 0);
            cooldownsUsed++;
        }
        else if (passConfig.PassNumber < 3)
        {
            // Defer remaining meters to next pass
            foreach (var remaining in metersNotYetRead)
                passResult.DeferredMeters.Add(remaining);
            passResult.DeferredIps.Add(ipKey);
            break; // exit meter loop for this IP
        }
        else
        {
            // Pass 3: definitive abandon
            foreach (var remaining in metersNotYetRead)
                passResult.Results.Add(new MeterResult {
                    Serial = remaining.Serial, Ip = remaining.Ip, Port = remaining.Port,
                    Error = "Abandon definitif"
                });
            break;
        }
    }
}
```

Thread safety: same pattern as current code — `Volatile.Read/Write` for `aborted` flag,
`Interlocked.Increment/Exchange` for `consecutiveFailures` and `cooldownsUsed`. The semaphore(1)
per IP ensures sequential meter reads within an IP, so these counters are effectively single-threaded
per IP. The `Volatile`/`Interlocked` usage is defensive for the abort flag which is checked before
acquiring the semaphore.

## Budget Time Management

### Global budget

A hard 50-minute ceiling protects against unbounded runtime:

```csharp
var globalBudget = Stopwatch.StartNew();
const int GlobalCeilingSeconds = 3000; // 50 minutes

foreach (var passConfig in passConfigs)
{
    if (metersToRead.Count == 0) break;

    // Check global budget before each pass
    var globalTimeLeft = GlobalCeilingSeconds - globalBudget.Elapsed.TotalSeconds;
    if (globalTimeLeft < 60)
    {
        Console.WriteLine("  Global budget expired, skipping remaining passes.");
        break;
    }

    // Clamp pass budget to remaining global budget
    var effectiveBudget = (int)Math.Min(passConfig.BudgetSeconds, globalTimeLeft);
    // ... run pass with effectiveBudget
}
```

### Per-pass budget

Each pass tracks remaining time via `PassBudget`:

- Before starting an IP: check `budget.IsExpired` (< 30s left). If expired, defer all unstarted IPs and their meters.
- Before each meter (including canary): clamp timeout to `budget.ClampTimeout(desiredTimeout)`. If returns -1, defer remaining meters on this IP.
- 5s safety margin on all timeout clamps to avoid races.

## AdaptivePool Behavior Across Passes

The `AdaptivePool` is **reset to `maxConcurrentIps` at the start of each pass**. Each pass represents
different network conditions (after 5-min recovery), so carrying over a shrunken pool from Pass 1
would penalize Pass 2 unnecessarily.

```csharp
// In RunSinglePass, create a fresh pool:
var adaptivePool = new AdaptivePool(maxConcurrentIps, minSize: 5, maxSize: 20);
```

The `ConcentratorStats` instance is **shared across all passes** to accumulate global KPI data.
Per-pass stats are available via `PassResult`.

The expand/shrink heuristic (every 5 completed IPs) remains the same within each pass.

## RunSinglePass Pseudocode

```csharp
private static async Task<PassResult> RunSinglePass(
    List<MeterInfo> metersToRead,
    PassConfig passConfig,
    int effectiveBudget,     // may be clamped by global budget
    DLMSGuruxSessionFactory sessionFactory,
    DLMSKeyService keyService,
    int maxConcurrentIps,
    ConcentratorStats concentratorStats,
    HashSet<string> previousDeferredIps)  // null for Pass 1
{
    var passResult = new PassResult { PassNumber = passConfig.PassNumber };
    var budget = new PassBudget(effectiveBudget);
    var adaptivePool = new AdaptivePool(maxConcurrentIps, minSize: 5, maxSize: 20);

    Console.WriteLine($"=== PASS {passConfig.PassNumber}/3 ({effectiveBudget}s budget) ===");

    // --- Step 1: TCP scan ---
    var ipGroups = metersToRead.GroupBy(m => $"{m.Ip}:{m.Port}").ToList();
    HashSet<string> reachableIps;

    if (passConfig.PassNumber == 1)
    {
        // Pass 1: scan all IPs
        var allIps = ipGroups.Select(g => (g.First().Ip, g.First().Port)).Distinct();
        var scanResults = await ParallelTcpScan(allIps, TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));

        // Individual retry fallback for failures
        var failedScan = scanResults.Where(r => !r.Reachable).ToList();
        reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));
        if (failedScan.Count > 0 && failedScan.Count <= 10)
        {
            foreach (var f in failedScan)
            {
                var parts = f.Key.Split(':');
                var retry = await SingleTcpScan(parts[0], parts[1], TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));
                if (retry.Reachable) reachableIps.Add(retry.Key);
            }
        }

        // Defer unreachable IPs
        foreach (var g in ipGroups.Where(g => !reachableIps.Contains($"{g.First().Ip}:{g.First().Port}")))
        {
            var ipKey = $"{g.First().Ip}:{g.First().Port}";
            passResult.DeferredIps.Add(ipKey);
            foreach (var m in g) passResult.DeferredMeters.Add(m);
        }
    }
    else
    {
        // Pass 2/3: only re-scan deferred IPs from previous pass
        var ipsNeedingScan = ipGroups
            .Where(g => previousDeferredIps.Contains($"{g.First().Ip}:{g.First().Port}"))
            .Select(g => (g.First().Ip, g.First().Port)).Distinct();
        var scanResults = await ParallelTcpScan(ipsNeedingScan, TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));
        var newlyReachable = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));

        // IPs with retry meters that were reachable before: no re-scan needed
        var previouslyReachableIps = ipGroups
            .Select(g => $"{g.First().Ip}:{g.First().Port}")
            .Where(ip => !previousDeferredIps.Contains(ip))
            .ToHashSet();

        reachableIps = new HashSet<string>(newlyReachable.Union(previouslyReachableIps));

        // Defer still-unreachable IPs
        foreach (var g in ipGroups.Where(g => !reachableIps.Contains($"{g.First().Ip}:{g.First().Port}")))
        {
            var ipKey = $"{g.First().Ip}:{g.First().Port}";
            passResult.DeferredIps.Add(ipKey);
            foreach (var m in g) passResult.DeferredMeters.Add(m);
        }
    }

    // --- Step 2: Sort reachable IPs ---
    var reachableGroups = ipGroups
        .Where(g => reachableIps.Contains($"{g.First().Ip}:{g.First().Port}"))
        .Select(g => (Key: $"{g.First().Ip}:{g.First().Port}", Meters: g.OrderByDescending(m => HasCache(m) ? 1 : 0).ToList()))
        .OrderBy(g => g.Meters.Count <= 5 ? 0 : 1)
        .ThenByDescending(g => g.Meters.Count(m => HasCache(m)))
        .ThenBy(g => g.Meters.Count)
        .ToList();

    // --- Step 3: Process each IP in parallel (pool-limited) ---
    var consoleLock = new object();
    int completedIps = 0;

    var tasks = reachableGroups.Select(entry => Task.Run(async () =>
    {
        await adaptivePool.WaitAsync();
        try
        {
            if (budget.IsExpired)
            {
                // Budget expired: defer all meters on this IP
                foreach (var m in entry.Meters) passResult.DeferredMeters.Add(m);
                passResult.DeferredIps.Add(entry.Key);
                return;
            }

            var meterList = entry.Meters;
            var firstMeter = meterList[0];
            var ipKey = entry.Key;

            // Open TCP
            var session = sessionFactory.CreateSession(new DLMSConnectionParameters
            {
                AddressIp = firstMeter.Ip, Port = firstMeter.Port, Trace = TraceLevel.Off
            });
            using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var tcpOk = await session.OpenTransportAsync(tcpCts.Token);
            if (!tcpOk)
            {
                passResult.DeferredIps.Add(ipKey);
                foreach (var m in meterList) passResult.DeferredMeters.Add(m);
                return;
            }

            try
            {
                // --- Canary test ---
                var canaryMeter = meterList[0]; // already sorted: cached first
                var canaryTimeout = budget.ClampTimeout(passConfig.CanaryTimeoutSeconds);
                if (canaryTimeout == -1)
                {
                    foreach (var m in meterList) passResult.DeferredMeters.Add(m);
                    passResult.DeferredIps.Add(ipKey);
                    return;
                }

                MeterResult canaryResult;
                var canarySw = Stopwatch.StartNew();
                try
                {
                    canaryResult = await ReadSingleMeter(session, canaryMeter, keyService)
                        .WaitAsync(TimeSpan.FromSeconds(canaryTimeout));
                }
                catch (TimeoutException)
                {
                    canaryResult = new MeterResult
                    {
                        Serial = canaryMeter.Serial, Ip = canaryMeter.Ip, Port = canaryMeter.Port,
                        Error = $"Canary timeout ({canaryTimeout}s)"
                    };
                }
                canarySw.Stop();

                // Record canary result (it is a real read attempt)
                lock (passResult.Results) { passResult.Results.Add(canaryResult); }

                if (!canaryResult.Success)
                {
                    // Canary failed: defer remaining meters
                    try { session.Reader?.Disconnect(); } catch { }
                    for (int i = 1; i < meterList.Count; i++)
                        passResult.DeferredMeters.Add(meterList[i]);
                    passResult.DeferredIps.Add(ipKey);
                    return;
                }

                // Canary OK: compute adaptive timeout
                var canaryLatencyMs = canarySw.ElapsedMilliseconds;
                // Read remaining meters (skip index 0 = canary)
                int consecutiveFailures = 0;
                int cooldownsUsed = 0;

                for (int i = 1; i < meterList.Count; i++)
                {
                    var meter = meterList[i];

                    if (budget.IsExpired)
                    {
                        // Defer remaining
                        for (int j = i; j < meterList.Count; j++)
                            passResult.DeferredMeters.Add(meterList[j]);
                        break;
                    }

                    var hasCacheFile = HasCache(meter);
                    var adaptiveTimeout = ComputeAdaptiveTimeout(canaryLatencyMs, hasCacheFile, passConfig);
                    var clampedTimeout = budget.ClampTimeout(adaptiveTimeout);
                    if (clampedTimeout == -1)
                    {
                        for (int j = i; j < meterList.Count; j++)
                            passResult.DeferredMeters.Add(meterList[j]);
                        break;
                    }

                    MeterResult meterResult;
                    try
                    {
                        meterResult = await ReadSingleMeter(session, meter, keyService)
                            .WaitAsync(TimeSpan.FromSeconds(clampedTimeout));
                    }
                    catch (TimeoutException)
                    {
                        meterResult = new MeterResult
                        {
                            Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                            Error = $"Timeout ({clampedTimeout}s)"
                        };
                        try { session.Reader?.Disconnect(); } catch { }
                    }

                    lock (passResult.Results) { passResult.Results.Add(meterResult); }

                    // Abandonment logic
                    if (meterResult.Success)
                    {
                        consecutiveFailures = 0;
                    }
                    else
                    {
                        consecutiveFailures++;
                        if (consecutiveFailures >= passConfig.MaxConsecutiveFailures)
                        {
                            if (cooldownsUsed < passConfig.CooldownCount)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(passConfig.CooldownSeconds));
                                consecutiveFailures = 0;
                                cooldownsUsed++;
                            }
                            else if (passConfig.PassNumber < 3)
                            {
                                for (int j = i + 1; j < meterList.Count; j++)
                                    passResult.DeferredMeters.Add(meterList[j]);
                                passResult.DeferredIps.Add(ipKey);
                                break;
                            }
                            else
                            {
                                // Pass 3: definitive abandon
                                for (int j = i + 1; j < meterList.Count; j++)
                                {
                                    lock (passResult.Results)
                                    {
                                        passResult.Results.Add(new MeterResult
                                        {
                                            Serial = meterList[j].Serial,
                                            Ip = meterList[j].Ip,
                                            Port = meterList[j].Port,
                                            Error = "Abandon definitif"
                                        });
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                await session.DisconnectAsync();
            }

            // Adaptive pool adjustment
            var ipNum = Interlocked.Increment(ref completedIps);
            if (ipNum % 5 == 0) { /* same expand/shrink logic as current */ }
        }
        finally
        {
            adaptivePool.Release();
        }
    })).ToList();

    await Task.WhenAll(tasks);

    passResult.ElapsedMs = budget.ElapsedMs;
    return passResult;
}
```

## Orchestrator Flow

```csharp
private static async Task<MultiPassReport> RunParallelTest(
    List<MeterInfo> metersWithKeys, DbContextOptions<DLMSDBContext> dbOptions,
    long dbLoadMs, int totalMeters, int totalIps, int maxConcurrentIps)
{
    var multiPassReport = new MultiPassReport { TotalMeters = metersWithKeys.Count };
    var successfulSerials = new HashSet<string>();
    var totalSw = Stopwatch.StartNew();
    const int GlobalCeilingSeconds = 3000; // 50 minutes hard ceiling

    var contextFactory = new SimpleDbContextFactory(dbOptions);
    var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
    var keyService = new DLMSKeyService(contextFactory, cache, _loggerFactory.CreateLogger<DLMSKeyService>());
    var sessionFactory = new DLMSGuruxSessionFactory(
        _loggerFactory.CreateLogger<DLMSGuruxSessionFactory>(),
        _loggerFactory.CreateLogger<DLMSGuruxSession>());
    var concentratorStats = new ConcentratorStats(); // shared across passes

    var passConfigs = new[]
    {
        new PassConfig(1, budget:900, canary:30, cached:60,  uncached:90,
                       maxFails:2, cooldownCount:0, cooldownSeconds:0,  pause:300),
        new PassConfig(2, budget:600, canary:45, cached:90,  uncached:120,
                       maxFails:3, cooldownCount:1, cooldownSeconds:15, pause:300),
        new PassConfig(3, budget:480, canary:60, cached:120, uncached:180,
                       maxFails:5, cooldownCount:1, cooldownSeconds:30, pause:0),
    };

    var metersToRead = metersWithKeys.ToList();
    HashSet<string> previousDeferredIps = null;

    foreach (var passConfig in passConfigs)
    {
        if (metersToRead.Count == 0) break;

        // Check global budget
        var globalTimeLeft = GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds;
        if (globalTimeLeft < 60)
        {
            Console.WriteLine("  Global budget expired, skipping remaining passes.");
            break;
        }
        var effectiveBudget = (int)Math.Min(passConfig.BudgetSeconds, globalTimeLeft);

        var passResult = await RunSinglePass(
            metersToRead, passConfig, effectiveBudget,
            sessionFactory, keyService, maxConcurrentIps,
            concentratorStats, previousDeferredIps);

        multiPassReport.Passes.Add(passResult);
        PrintPassSummary(passResult);

        // Remove successes, keep failures for next pass
        successfulSerials.UnionWith(
            passResult.Results.Where(r => r.Success).Select(r => r.Serial));
        metersToRead = metersToRead
            .Where(m => !successfulSerials.Contains(m.Serial))
            .ToList();
        previousDeferredIps = passResult.DeferredIps;

        // Inter-pass pause
        if (passConfig.PauseAfterSeconds > 0 && metersToRead.Count > 0)
        {
            var pauseMin = passConfig.PauseAfterSeconds / 60;
            Console.WriteLine($"\n  Pause {pauseMin} min avant Pass {passConfig.PassNumber + 1}...\n");
            await Task.Delay(TimeSpan.FromSeconds(passConfig.PauseAfterSeconds));
            multiPassReport.TotalPauseMs += passConfig.PauseAfterSeconds * 1000;
        }
    }

    // Categorize unread meters
    var allAttempted = multiPassReport.AllResults.ToDictionary(r => r.Serial, r => r);
    foreach (var meter in metersWithKeys.Where(m => !successfulSerials.Contains(m.Serial)))
    {
        string reason;
        if (allAttempted.TryGetValue(meter.Serial, out var lastResult))
        {
            reason = lastResult.Error switch
            {
                var e when e.Contains("TCP") || e.Contains("pre-scan") => "TCP unreachable",
                var e when e.Contains("Abandon") || e.Contains("Ignore") => "Definitive abandon",
                var e when e.Contains("Timeout") || e.Contains("timeout") => "Timeout",
                var e when e.Contains("Budget") => "Budget expired",
                _ => "Other"
            };
        }
        else { reason = "TCP unreachable"; }

        if (!multiPassReport.UnreadByReason.ContainsKey(reason))
            multiPassReport.UnreadByReason[reason] = new List<string>();
        multiPassReport.UnreadByReason[reason].Add(meter.Serial);
    }

    multiPassReport.TotalElapsedMs = totalSw.ElapsedMilliseconds;
    multiPassReport.TotalReadingMs = multiPassReport.Passes.Sum(p => p.ElapsedMs);
    PrintMultiPassReport(multiPassReport);
    return multiPassReport;
}
```

## Report Output

### Intermediate (after each pass)

```
=== PASS 1/3 COMPLETE (12m 34s) ===
  IPs processed  : 20/28 (8 deferred to Pass 2)
  In scope       : 162 meters
  Reads OK       : 98 (60.5%)
  Failed         : 34
  Deferred       : 30 (canary fail / IP abandon / budget)
  Throughput     : 7.9 meters/min
  Next: Pause 5 min then Pass 2 on 202 remaining meters
```

"In scope" = meters assigned to this pass. "Failed" = attempted but failed.
"Deferred" = not attempted, carried to next pass.

### Final consolidated

```
=== MULTI-PASS REPORT (3 passes, 43m 12s) ===

  Per-pass summary:
  +--------+---------+----------+------+-------+------------+
  | Pass   | Duration| In Scope | OK   | Rate  | Throughput |
  +--------+---------+----------+------+-------+------------+
  | Pass 1 | 12m 34s | 162      | 98   | 60.5% | 7.9/min    |
  | Pass 2 |  8m 45s |  95      | 34   | 35.8% | 3.9/min    |
  | Pass 3 |  6m 53s |  61      | 12   | 19.7% | 1.7/min    |
  +--------+---------+----------+------+-------+------------+
  | TOTAL  | 43m 12s | 300      | 144  | 48.0% | 3.3/min    |
  | Pauses | +10m 0s |          |      |       |            |
  +--------+---------+----------+------+-------+------------+

  Unread meters (156):
    TCP unreachable     :  24 ( 8.0%) — IPs dead across 3 passes
    Definitive abandon  :  45 (15.0%) — persistent concentrator failures
    Timeout             :  52 (17.3%) — slow individual meters
    Budget expired      :  35 (11.7%) — no time left in Pass 3
```

## Files Modified

| File | Change |
|------|--------|
| `DLMS_IntegrationTest/Program.cs` | Refactor `RunParallelTest` into multi-pass orchestrator. Add `PassConfig`, `PassBudget`, `PassResult`, `MultiPassReport`. Add `RunSinglePass`, `ParallelTcpScan`, `SingleTcpScan`, `ComputeAdaptiveTimeout`, `HasCache`, `PrintPassSummary`, `PrintMultiPassReport`. The `--parallel` path in `Main` switches return type handling from `TestReport` to `MultiPassReport`. `PrintReport` is kept for sequential/single modes. |

No other files modified. The change is self-contained in the integration test.

## What Does NOT Change

- `MeterInfo`, `MeterResult`, `IpGroupResult` — existing data structures
- `ReadSingleMeter()` — per-meter read logic with internal retry on "Lecture impossible"
- `LoadMetersFromDb()` — DB loading
- `RunSingleMeterTest()`, `RunSequentialTest()` — alternative modes
- `AdaptivePool` class, `ConcentratorStats` class — pool and KPI tracking (instances are created per-pass for pool, shared for stats)
- `DLMS_SERVICE/` — production service code (separate optimization later)
- CLI interface: `--parallel [N]` still works, now triggers multi-pass

## Expected Performance

Based on log analysis (5 runs, 300 meters, 33 concentrators):

| Metric | Current (best single-pass) | Expected (multi-pass) |
|--------|---------------------------|----------------------|
| Total duration | 21-80 min | ~43 min (33 min reading + 10 min pause) |
| Success rate | 16-45% | 45-55% |
| Pass 1 throughput | 1.7-5.4/min | 6-8/min (aggressive timeouts + canary) |
| TCP recovery | 0 IPs across run | +2-4 IPs at Pass 2/3 |
| Meters from retry | 0 | +30-50 from Pass 2/3 |

The main gain is in Pass 1 throughput: by ejecting dead IPs in 30s (canary fail) instead of 9 min (3x180s timeout + abandon), pool slots are freed for productive work.
