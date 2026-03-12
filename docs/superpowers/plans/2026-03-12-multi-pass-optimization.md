# Multi-Pass DLMS Meter Reading Optimization — Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Refactor the parallel integration test to use a 3-pass strategy (Fast Harvest / Recovery / Last Chance) with canary tests, adaptive timeouts, and progressive IP abandonment within a 45-minute budget.

**Architecture:** All changes in `DLMS_IntegrationTest/Program.cs`. The current `RunParallelTest` is replaced by a multi-pass orchestrator that calls `RunSinglePass` three times with progressively relaxed configurations. New data classes (`PassConfig`, `PassBudget`, `PassResult`, `MultiPassReport`) and helpers (`ParallelTcpScan`, `SingleTcpScan`, `ComputeAdaptiveTimeout`, `HasCache`) support the new flow.

**Tech Stack:** C# / .NET, Gurux DLMS, Entity Framework Core

**Spec:** `docs/superpowers/specs/2026-03-12-multi-pass-optimization-design.md`

---

## File Structure

All changes target a single file:

| File | Action | Responsibility |
|------|--------|----------------|
| `DLMS_IntegrationTest/Program.cs` | Modify | Add new data classes, refactor `RunParallelTest`, add `RunSinglePass` + helpers + report methods |

### Code sections within `Program.cs` (current line references):

| Section | Current Lines | Change |
|---------|---------------|--------|
| Data classes (`MeterInfo`, `MeterResult`, etc.) | 17-88 | Keep. Add `PassConfig`, `PassBudget`, `PassResult`, `MultiPassReport` after line 88 |
| `ConcentratorStats`, `AdaptivePool` | 90-200 | Keep as-is |
| `Main` method | 208-329 | Modify `--parallel` branch to use `MultiPassReport` return |
| `ReadSingleMeter` | 379-479 | Keep as-is |
| `RunSequentialTest` | 483-653 | Keep as-is |
| `RunParallelTest` | 657-1013 | **Replace entirely** with multi-pass orchestrator |
| `PrintReport` | 1017-1108 | Keep for seq/single. Add `PrintPassSummary` + `PrintMultiPassReport` |
| `RunSingleMeterTest` | 1113-1308 | Keep as-is |
| Helpers (end of file) | 1310-1373 | Add `ParallelTcpScan`, `SingleTcpScan`, `ComputeAdaptiveTimeout`, `HasCache` |

---

## Chunk 1: Data Structures + Helpers

### Task 1: Add PassConfig class

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — insert after line 88 (after `TestReport` class)

- [ ] **Step 1: Add PassConfig class**

Insert after the closing brace of `TestReport` (line 88):

```csharp
// ===== Multi-pass configuration =====

class PassConfig
{
    public int PassNumber;
    public int BudgetSeconds;
    public int CanaryTimeoutSeconds;
    public int CachedTimeoutSeconds;
    public int UncachedTimeoutSeconds;
    public int MaxConsecutiveFailures;
    public int CooldownCount;
    public int CooldownSeconds;
    public int PauseAfterSeconds;

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

- [ ] **Step 2: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: add PassConfig class for multi-pass DLMS reading"
```

---

### Task 2: Add PassBudget class

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — insert after `PassConfig`

- [ ] **Step 1: Add PassBudget class**

Insert directly after `PassConfig`:

```csharp
class PassBudget
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private readonly int _budgetSeconds;

    public PassBudget(int budgetSeconds) => _budgetSeconds = budgetSeconds;

    public double TimeLeftSeconds => _budgetSeconds - _sw.Elapsed.TotalSeconds;
    public bool IsExpired => TimeLeftSeconds < 30;
    public long ElapsedMs => _sw.ElapsedMilliseconds;

    public int ClampTimeout(int desiredTimeout)
    {
        var maxAllowed = (int)TimeLeftSeconds - 5;
        if (maxAllowed < 15) return -1;
        return Math.Min(desiredTimeout, maxAllowed);
    }
}
```

- [ ] **Step 2: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: add PassBudget class for per-pass time management"
```

---

### Task 3: Add PassResult and MultiPassReport classes

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — insert after `PassBudget`

- [ ] **Step 1: Add PassResult class**

```csharp
class PassResult
{
    public int PassNumber;
    public List<MeterResult> Results = new();                       // lock before Add (parallel tasks)
    public ConcurrentBag<MeterInfo> DeferredMeters = new();         // thread-safe: concurrent Add from parallel tasks
    public ConcurrentDictionary<string, byte> DeferredIps = new();  // thread-safe: concurrent Add from parallel tasks
    public long ElapsedMs;

    public int Succeeded => Results.Count(r => r.Success);
    public int Failed => Results.Count(r => !r.Success);
    public int DeferredCount => DeferredMeters.Count;
    public int InScope => Results.Count + DeferredCount;

    public void DeferIp(string ipKey) => DeferredIps.TryAdd(ipKey, 0);
    public void DeferMeter(MeterInfo m) => DeferredMeters.Add(m);
    public HashSet<string> GetDeferredIpSet() => DeferredIps.Keys.ToHashSet();
}
```

- [ ] **Step 2: Add MultiPassReport class**

```csharp
class MultiPassReport
{
    public List<PassResult> Passes = new();
    public long TotalElapsedMs;
    public long TotalReadingMs;
    public long TotalPauseMs;
    public int TotalMeters;

    public int TotalSucceeded => Passes.Sum(p => p.Succeeded);
    public int TotalFailed => TotalMeters - TotalSucceeded;

    public List<MeterResult> AllResults => Passes.SelectMany(p => p.Results).ToList();

    public Dictionary<string, List<string>> UnreadByReason = new();
}
```

- [ ] **Step 3: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded

- [ ] **Step 4: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: add PassResult and MultiPassReport data classes"
```

---

### Task 4: Add static helper methods

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — add as static methods in `Program` class, before `GetConnectionString` (line ~1310)

- [ ] **Step 1: Add HasCache helper**

Insert before `GetConnectionString`:

```csharp
    // ===== Multi-pass helpers =====

    private static bool HasCache(MeterInfo m) =>
        File.Exists(Path.Combine("associations", $"{m.Serial}_Read.xml"));

    private static int ComputeAdaptiveTimeout(long canaryLatencyMs, bool hasCacheFile, PassConfig config)
    {
        var raw = (int)(canaryLatencyMs / 1000.0 * 2.5);
        var ceiling = hasCacheFile ? config.CachedTimeoutSeconds : config.UncachedTimeoutSeconds;
        return Math.Clamp(raw, 45, ceiling);
    }
```

- [ ] **Step 2: Add ParallelTcpScan helper**

```csharp
    private static async Task<List<(string Key, bool Reachable, long Ms)>> ParallelTcpScan(
        IEnumerable<(string Ip, string Port)> targets, TimeSpan timeout)
    {
        var scanLock = new object();
        int scanned = 0;
        var targetList = targets.ToList();

        var tasks = targetList.Select(t => Task.Run(async () =>
        {
            var key = $"{t.Ip}:{t.Port}";
            try
            {
                using var client = new TcpClient();
                using var cts = new CancellationTokenSource(timeout);
                var sw = Stopwatch.StartNew();
                await client.ConnectAsync(t.Ip, int.Parse(t.Port), cts.Token);
                sw.Stop();
                var num = Interlocked.Increment(ref scanned);
                lock (scanLock) { Console.WriteLine($"  [{num}/{targetList.Count}] {key} : OK ({sw.ElapsedMilliseconds}ms)"); }
                return (Key: key, Reachable: true, Ms: sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                var num = Interlocked.Increment(ref scanned);
                lock (scanLock) { Console.WriteLine($"  [{num}/{targetList.Count}] {key} : ECHEC ({ex.Message})"); }
                return (Key: key, Reachable: false, Ms: (long)timeout.TotalMilliseconds);
            }
        }));
        return (await Task.WhenAll(tasks)).ToList();
    }
```

- [ ] **Step 3: Add SingleTcpScan helper**

```csharp
    private static async Task<(string Key, bool Reachable, long Ms)> SingleTcpScan(
        string ip, string port, TimeSpan timeout)
    {
        var key = $"{ip}:{port}";
        try
        {
            using var client = new TcpClient();
            using var cts = new CancellationTokenSource(timeout);
            var sw = Stopwatch.StartNew();
            await client.ConnectAsync(ip, int.Parse(port), cts.Token);
            sw.Stop();
            Console.WriteLine($"  [retry] {key} : OK ({sw.ElapsedMilliseconds}ms)");
            return (key, true, sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [retry] {key} : ECHEC ({ex.Message})");
            return (key, false, (long)timeout.TotalMilliseconds);
        }
    }
```

- [ ] **Step 4: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded

- [ ] **Step 5: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: add TCP scan and adaptive timeout helpers"
```

---

## Chunk 2: RunSinglePass Method

### Task 5: Implement RunSinglePass

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — add new method in `Program` class, between the current `RunParallelTest` and `PrintReport`

This is the core method. It implements one full pass: TCP scan → sort IPs → canary per IP → read meters → abandon/defer logic.

- [ ] **Step 1: Add RunSinglePass method signature and TCP scan phase**

Add after the current `RunParallelTest` method (we will replace `RunParallelTest` later; for now add alongside):

```csharp
    // ===== Multi-pass: single pass execution =====

    private static async Task<PassResult> RunSinglePass(
        List<MeterInfo> metersToRead,
        PassConfig passConfig,
        int effectiveBudget,
        DLMSGuruxSessionFactory sessionFactory,
        DLMSKeyService keyService,
        int maxConcurrentIps,
        ConcentratorStats concentratorStats,
        HashSet<string>? previousDeferredIps)
    {
        var passResult = new PassResult { PassNumber = passConfig.PassNumber };
        var budget = new PassBudget(effectiveBudget);
        var adaptivePool = new AdaptivePool(maxConcurrentIps, minSize: 5, maxSize: 20);

        Console.WriteLine($"=== PASS {passConfig.PassNumber}/3 ({effectiveBudget}s budget, {metersToRead.Count} compteurs) ===");
        Console.WriteLine();

        // --- Step 1: TCP scan ---
        var ipGroups = metersToRead.GroupBy(m => $"{m.Ip}:{m.Port}").ToList();
        var reachableIps = new HashSet<string>();

        if (passConfig.PassNumber == 1 || previousDeferredIps == null)
        {
            // Pass 1: scan all IPs
            Console.WriteLine($"[Pre-scan] Test TCP parallele de {ipGroups.Count} IPs (timeout {PassConfig.TcpScanTimeoutSeconds}s)...");
            var allIps = ipGroups.Select(g => (g.First().Ip, g.First().Port)).Distinct().ToList();
            var scanResults = await ParallelTcpScan(allIps, TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));

            reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));

            // Individual retry fallback for failures
            var failedScan = scanResults.Where(r => !r.Reachable).ToList();
            if (failedScan.Count > 0 && failedScan.Count <= 10)
            {
                Console.WriteLine($"  [Pre-scan] Retry individuel de {failedScan.Count} IPs echouees...");
                foreach (var f in failedScan)
                {
                    var parts = f.Key.Split(':');
                    var retry = await SingleTcpScan(parts[0], parts[1], TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));
                    if (retry.Reachable) reachableIps.Add(retry.Key);
                }
            }
        }
        else
        {
            // Pass 2/3: only re-scan deferred IPs
            var ipsNeedingScan = ipGroups
                .Where(g => previousDeferredIps.Contains($"{g.First().Ip}:{g.First().Port}"))
                .Select(g => (g.First().Ip, g.First().Port)).Distinct().ToList();

            if (ipsNeedingScan.Count > 0)
            {
                Console.WriteLine($"[Re-scan] Test TCP de {ipsNeedingScan.Count} IPs differees (timeout {PassConfig.TcpScanTimeoutSeconds}s)...");
                var scanResults = await ParallelTcpScan(ipsNeedingScan, TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));
                var newlyReachable = scanResults.Where(r => r.Reachable).Select(r => r.Key);
                reachableIps.UnionWith(newlyReachable);
            }

            // IPs with retry meters that were reachable before: no re-scan needed
            var previouslyReachableIps = ipGroups
                .Select(g => $"{g.First().Ip}:{g.First().Port}")
                .Where(ip => !previousDeferredIps.Contains(ip));
            reachableIps.UnionWith(previouslyReachableIps);
        }

        // Defer unreachable IPs
        foreach (var g in ipGroups.Where(g => !reachableIps.Contains($"{g.First().Ip}:{g.First().Port}")))
        {
            var ipKey = $"{g.First().Ip}:{g.First().Port}";
            passResult.DeferIp(ipKey);
            foreach (var m in g) passResult.DeferMeter(m);
        }

        Console.WriteLine($"  Resultats: {reachableIps.Count}/{ipGroups.Count} IPs accessibles");
        Console.WriteLine();

        // --- Step 2: Sort reachable IPs ---
        var reachableGroups = ipGroups
            .Where(g => reachableIps.Contains($"{g.First().Ip}:{g.First().Port}"))
            .Select(g => (Key: $"{g.First().Ip}:{g.First().Port}",
                          Meters: g.OrderByDescending(m => HasCache(m) ? 1 : 0).ToList()))
            .OrderBy(g => g.Meters.Count <= 5 ? 0 : 1)
            .ThenByDescending(g => g.Meters.Count(m => HasCache(m)))
            .ThenBy(g => g.Meters.Count)
            .ToList();

        var totalReachableMeters = reachableGroups.Sum(g => g.Meters.Count);
        Console.WriteLine($"  Pool: {maxConcurrentIps} IPs max en parallele");
        Console.WriteLine($"  Lancement de {totalReachableMeters} lectures sur {reachableGroups.Count} IPs...");
        Console.WriteLine();

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
                    lock (consoleLock) { Console.WriteLine($"    [{entry.Key}] Budget expire, {entry.Meters.Count} compteurs differes"); }
                    // Record budget-expired meters with error (for categorization) if last pass
                    if (passConfig.PassNumber >= 3)
                    {
                        foreach (var m in entry.Meters)
                        {
                            lock (passResult.Results)
                            {
                                passResult.Results.Add(new MeterResult
                                {
                                    Serial = m.Serial, Ip = m.Ip, Port = m.Port,
                                    Error = "Budget expired"
                                });
                            }
                        }
                    }
                    else
                    {
                        foreach (var m in entry.Meters) passResult.DeferMeter(m);
                        passResult.DeferIp(entry.Key);
                    }
                    return;
                }

                var meterList = entry.Meters;
                var firstMeter = meterList[0];
                var ipKey = entry.Key;

                // Open TCP
                var transportParams = new DLMSConnectionParameters
                {
                    AddressIp = firstMeter.Ip,
                    Port = firstMeter.Port,
                    Trace = TraceLevel.Off
                };
                var session = sessionFactory.CreateSession(transportParams);

                using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var tcpOk = await session.OpenTransportAsync(tcpCts.Token);
                if (!tcpOk)
                {
                    lock (consoleLock) { Console.WriteLine($"    [{ipKey}] TCP echec, {meterList.Count} compteurs differes"); }
                    passResult.DeferIp(ipKey);
                    foreach (var m in meterList) passResult.DeferMeter(m);
                    return;
                }

                try
                {
                    // --- Canary test ---
                    var canaryMeter = meterList[0];
                    var canaryTimeout = budget.ClampTimeout(passConfig.CanaryTimeoutSeconds);
                    if (canaryTimeout == -1)
                    {
                        foreach (var m in meterList) passResult.DeferMeter(m);
                        passResult.DeferIp(ipKey);
                        return;
                    }

                    Console.WriteLine($"    [{ipKey}] Canary {canaryMeter.Serial} (timeout:{canaryTimeout}s)");

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
                        try { session.Reader?.Disconnect(); } catch { }
                    }
                    canarySw.Stop();

                    // Record canary result
                    lock (passResult.Results) { passResult.Results.Add(canaryResult); }
                    concentratorStats.Record(ipKey, canaryResult.Success,
                        canaryResult.HdlcMs + canaryResult.ReadMs + canaryResult.KeysRetrievalMs);

                    if (!canaryResult.Success)
                    {
                        try { session.Reader?.Disconnect(); } catch { }
                        if (passConfig.PassNumber < 3)
                        {
                            // Pass 1/2: defer remaining meters to next pass
                            lock (consoleLock) { Console.WriteLine($"    [{ipKey}] Canary ECHEC — {meterList.Count - 1} compteurs differes"); }
                            for (int i = 1; i < meterList.Count; i++)
                                passResult.DeferMeter(meterList[i]);
                            passResult.DeferIp(ipKey);
                        }
                        else
                        {
                            // Pass 3: no next pass — record remaining as definitively failed
                            lock (consoleLock) { Console.WriteLine($"    [{ipKey}] Canary ECHEC Pass 3 — {meterList.Count - 1} compteurs en abandon definitif"); }
                            for (int i = 1; i < meterList.Count; i++)
                            {
                                lock (passResult.Results)
                                {
                                    passResult.Results.Add(new MeterResult
                                    {
                                        Serial = meterList[i].Serial, Ip = meterList[i].Ip, Port = meterList[i].Port,
                                        Error = "Abandon definitif (canary echec Pass 3)"
                                    });
                                }
                            }
                        }
                        return;
                    }

                    var canaryLatencyMs = canarySw.ElapsedMilliseconds;
                    lock (consoleLock)
                    {
                        Console.WriteLine($"    [{ipKey}] Canary OK ({canaryLatencyMs}ms) — lecture de {meterList.Count - 1} compteurs restants");
                    }

                    // --- Read remaining meters (skip index 0 = canary) ---
                    int consecutiveFailures = 0;
                    int cooldownsUsed = 0;

                    for (int i = 1; i < meterList.Count; i++)
                    {
                        var meter = meterList[i];

                        // Budget check
                        if (budget.IsExpired)
                        {
                            for (int j = i; j < meterList.Count; j++)
                                passResult.DeferMeter(meterList[j]);
                            break;
                        }

                        var hasCacheFile = HasCache(meter);
                        var adaptiveTimeout = ComputeAdaptiveTimeout(canaryLatencyMs, hasCacheFile, passConfig);
                        var clampedTimeout = budget.ClampTimeout(adaptiveTimeout);
                        if (clampedTimeout == -1)
                        {
                            for (int j = i; j < meterList.Count; j++)
                                passResult.DeferMeter(meterList[j]);
                            break;
                        }

                        Console.WriteLine($"    [{ipKey}] Debut lecture {meter.Serial} (timeout:{clampedTimeout}s)");

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
                        concentratorStats.Record(ipKey, meterResult.Success,
                            meterResult.HdlcMs + meterResult.ReadMs + meterResult.KeysRetrievalMs);

                        if (meterResult.Success)
                        {
                            Console.WriteLine($"    [{ipKey}] {meter.Serial} OK (HDLC:{meterResult.HdlcMs}ms, Lecture:{meterResult.ReadMs}ms)");
                            consecutiveFailures = 0;
                        }
                        else
                        {
                            Console.WriteLine($"    [{ipKey}] {meter.Serial} ECHEC - {meterResult.Error}");
                            consecutiveFailures++;
                            if (consecutiveFailures >= passConfig.MaxConsecutiveFailures)
                            {
                                if (cooldownsUsed < passConfig.CooldownCount)
                                {
                                    Console.WriteLine($"    [{ipKey}] Cooldown {passConfig.CooldownSeconds}s apres {passConfig.MaxConsecutiveFailures} echecs consecutifs");
                                    await Task.Delay(TimeSpan.FromSeconds(passConfig.CooldownSeconds));
                                    consecutiveFailures = 0;
                                    cooldownsUsed++;
                                }
                                else if (passConfig.PassNumber < 3)
                                {
                                    Console.WriteLine($"    [{ipKey}] IP differee apres {passConfig.MaxConsecutiveFailures} echecs consecutifs");
                                    for (int j = i + 1; j < meterList.Count; j++)
                                        passResult.DeferMeter(meterList[j]);
                                    passResult.DeferIp(ipKey);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($"    [{ipKey}] ABANDON DEFINITIF apres {passConfig.MaxConsecutiveFailures} echecs consecutifs");
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

                // Log summary for this IP
                var ok = passResult.Results.Count(r => r.Success && r.Ip == firstMeter.Ip);
                var ipNum = Interlocked.Increment(ref completedIps);
                lock (consoleLock)
                {
                    Console.WriteLine($"  [IP {ipNum}/{reachableGroups.Count}] {ipKey} : {ok}/{meterList.Count} OK");
                }

                // Adaptive pool adjustment every 5 completed IPs
                if (ipNum % 5 == 0)
                {
                    var (successRate, avgLatencyMs, total) = concentratorStats.GetGlobalStats();
                    if (successRate > 80 && avgLatencyMs < 60000)
                    {
                        adaptivePool.Expand(2);
                        lock (consoleLock) { Console.WriteLine($"  [POOL] Expanded to {adaptivePool.CurrentSize} (success={successRate:F0}%, lat={avgLatencyMs:F0}ms)"); }
                    }
                    else if (successRate < 50 || avgLatencyMs > 120000)
                    {
                        adaptivePool.Shrink(2);
                        lock (consoleLock) { Console.WriteLine($"  [POOL] Shrunk to {adaptivePool.CurrentSize} (success={successRate:F0}%, lat={avgLatencyMs:F0}ms)"); }
                    }
                }
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

- [ ] **Step 2: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded (method not yet called, but must compile)

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: add RunSinglePass method with canary, adaptive timeout, and progressive abandon"
```

---

## Chunk 3: Orchestrator + Reports + Wiring

### Task 6: Add PrintPassSummary and PrintMultiPassReport methods

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — add after `PrintReport` method

- [ ] **Step 1: Add PrintPassSummary**

Insert after the existing `PrintReport` method:

```csharp
    // ===== Multi-pass report methods =====

    private static void PrintPassSummary(PassResult pass)
    {
        var durationMin = pass.ElapsedMs / 60000.0;
        var throughput = durationMin > 0 ? pass.Succeeded / durationMin : 0;
        var rate = pass.Results.Count > 0 ? (double)pass.Succeeded / pass.Results.Count * 100 : 0;

        Console.WriteLine();
        Console.WriteLine($"=== PASS {pass.PassNumber}/3 TERMINE ({durationMin:F1} min) ===");
        Console.WriteLine($"  In scope       : {pass.InScope} compteurs");
        Console.WriteLine($"  Lectures OK    : {pass.Succeeded} ({rate:F1}%)");
        Console.WriteLine($"  Echecs         : {pass.Failed}");
        Console.WriteLine($"  Differes       : {pass.DeferredCount} (canary/abandon/budget)");
        Console.WriteLine($"  IPs differees  : {pass.DeferredIps.Count}");
        Console.WriteLine($"  Debit          : {throughput:F1} compteurs/min");
        Console.WriteLine();
    }

    private static void PrintMultiPassReport(MultiPassReport report)
    {
        Console.WriteLine();
        Console.WriteLine($"=== RAPPORT MULTI-PASS ({report.Passes.Count} passes, {report.TotalElapsedMs / 60000.0:F1} min) ===");
        Console.WriteLine();

        // Per-pass table
        Console.WriteLine($"  {"Pass",-8} {"Duree",-10} {"In Scope",-10} {"OK",-6} {"Taux",-8} {"Debit",-12}");
        Console.WriteLine($"  {new string('-', 54)}");

        foreach (var pass in report.Passes)
        {
            var durationMin = pass.ElapsedMs / 60000.0;
            var throughput = durationMin > 0 ? pass.Succeeded / durationMin : 0;
            var rate = pass.Results.Count > 0 ? (double)pass.Succeeded / pass.Results.Count * 100 : 0;

            Console.WriteLine($"  Pass {pass.PassNumber,-3} {durationMin,-9:F1}m {pass.InScope,-10} {pass.Succeeded,-6} {rate,-7:F1}% {throughput,-11:F1}/min");
        }

        Console.WriteLine($"  {new string('-', 54)}");
        var totalMin = report.TotalReadingMs / 60000.0;
        var totalThroughput = totalMin > 0 ? report.TotalSucceeded / totalMin : 0;
        var totalRate = report.TotalMeters > 0 ? (double)report.TotalSucceeded / report.TotalMeters * 100 : 0;
        Console.WriteLine($"  {"TOTAL",-8} {totalMin,-9:F1}m {report.TotalMeters,-10} {report.TotalSucceeded,-6} {totalRate,-7:F1}% {totalThroughput,-11:F1}/min");
        Console.WriteLine($"  {"Pauses",-8} +{report.TotalPauseMs / 60000.0:F1}m");
        Console.WriteLine();

        // Unread breakdown
        if (report.UnreadByReason.Count > 0)
        {
            var totalUnread = report.TotalFailed;
            Console.WriteLine($"  Compteurs non lus ({totalUnread}) :");
            foreach (var kv in report.UnreadByReason.OrderByDescending(kv => kv.Value.Count))
            {
                var pct = report.TotalMeters > 0 ? (double)kv.Value.Count / report.TotalMeters * 100 : 0;
                Console.WriteLine($"    {kv.Key,-25} : {kv.Value.Count,4} ({pct:F1}%)");
            }
            Console.WriteLine();
        }

        // Per-IP concentrator stats
        Console.WriteLine("=== STATISTIQUES PAR CONCENTRATEUR ===");
        var allResults = report.AllResults;
        var ipStats = allResults
            .GroupBy(r => $"{r.Ip}:{r.Port}")
            .Select(g => new
            {
                Ip = g.Key,
                Total = g.Count(),
                Ok = g.Count(r => r.Success),
                Fail = g.Count(r => !r.Success),
                AvgReadMs = g.Where(r => r.Success && r.ReadMs > 0).Select(r => (double)r.ReadMs).DefaultIfEmpty(0).Average()
            })
            .OrderBy(s => s.Ip);

        Console.WriteLine($"  {"IP",-25} {"Total",6} {"OK",6} {"Echec",6} {"Taux",6} {"Lecture Moy",12}");
        Console.WriteLine($"  {new string('-', 61)}");
        foreach (var s in ipStats)
        {
            var rate2 = s.Total > 0 ? (double)s.Ok / s.Total * 100 : 0;
            Console.WriteLine($"  {s.Ip,-25} {s.Total,6} {s.Ok,6} {s.Fail,6} {rate2,5:F0}% {s.AvgReadMs,10:F0}ms");
        }
        Console.WriteLine();
    }
```

- [ ] **Step 2: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: add PrintPassSummary and PrintMultiPassReport display methods"
```

---

### Task 7: Replace RunParallelTest with multi-pass orchestrator

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — replace `RunParallelTest` method (lines ~657-1013)

This is the critical task. The current `RunParallelTest` is replaced with the multi-pass orchestrator that calls `RunSinglePass` three times.

- [ ] **Step 1: Replace RunParallelTest**

Delete the entire current `RunParallelTest` method (from its signature to its closing brace) and replace with:

```csharp
    // ===== RunParallelTest (Multi-pass orchestrator) =====

    private static async Task<MultiPassReport> RunParallelTest(
        List<MeterInfo> meters, DbContextOptions<DLMSDBContext> dbOptions,
        long dbLoadMs, int totalMeters, int totalIps, int maxConcurrentIps = 10)
    {
        var multiPassReport = new MultiPassReport { TotalMeters = meters.Count };
        var successfulSerials = new HashSet<string>();
        var totalSw = Stopwatch.StartNew();
        const int GlobalCeilingSeconds = 3000; // 50 minutes hard ceiling

        Console.WriteLine($"=== Mode MULTI-PASS (pool={maxConcurrentIps} IPs, 3 passes, budget 50 min) ===");
        Console.WriteLine();

        var contextFactory = new SimpleDbContextFactory(dbOptions);
        var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
        var keyServiceLogger = _loggerFactory.CreateLogger<DLMSKeyService>();
        var keyService = new DLMSKeyService(contextFactory, cache, keyServiceLogger);

        var factoryLogger = _loggerFactory.CreateLogger<DLMSGuruxSessionFactory>();
        var sessionLogger = _loggerFactory.CreateLogger<DLMSGuruxSession>();
        var sessionFactory = new DLMSGuruxSessionFactory(factoryLogger, sessionLogger);

        var concentratorStats = new ConcentratorStats();

        var passConfigs = new[]
        {
            new PassConfig(1, budget: 900,  canary: 30, cached: 60,  uncached: 90,
                           maxFails: 2, cooldownCount: 0, cooldownSeconds: 0,  pause: 300),
            new PassConfig(2, budget: 600,  canary: 45, cached: 90,  uncached: 120,
                           maxFails: 3, cooldownCount: 1, cooldownSeconds: 15, pause: 300),
            new PassConfig(3, budget: 480,  canary: 60, cached: 120, uncached: 180,
                           maxFails: 5, cooldownCount: 1, cooldownSeconds: 30, pause: 0),
        };

        var metersToRead = meters.ToList();
        HashSet<string>? previousDeferredIps = null;

        foreach (var passConfig in passConfigs)
        {
            if (metersToRead.Count == 0)
            {
                Console.WriteLine("  Tous les compteurs ont ete lus, arret anticipe.");
                break;
            }

            // Check global budget
            var globalTimeLeft = GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds;
            if (globalTimeLeft < 60)
            {
                Console.WriteLine("  Budget global expire, arret des passes.");
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
            previousDeferredIps = passResult.GetDeferredIpSet();

            // Inter-pass pause
            if (passConfig.PauseAfterSeconds > 0 && metersToRead.Count > 0)
            {
                var pauseMin = passConfig.PauseAfterSeconds / 60;
                Console.WriteLine($"  Pause {pauseMin} min avant Pass {passConfig.PassNumber + 1} ({metersToRead.Count} compteurs restants)...");
                Console.WriteLine();
                await Task.Delay(TimeSpan.FromSeconds(passConfig.PauseAfterSeconds));
                multiPassReport.TotalPauseMs += passConfig.PauseAfterSeconds * 1000;
            }
        }

        // Categorize unread meters (fix: use GroupBy to handle cross-pass duplicates)
        var allAttempted = multiPassReport.AllResults
            .GroupBy(r => r.Serial)
            .ToDictionary(g => g.Key, g => g.Last());

        foreach (var meter in meters.Where(m => !successfulSerials.Contains(m.Serial)))
        {
            string reason;
            if (allAttempted.TryGetValue(meter.Serial, out var lastResult))
            {
                reason = lastResult.Error switch
                {
                    var e when e.Contains("TCP") || e.Contains("pre-scan") => "TCP unreachable",
                    var e when e.Contains("Abandon") || e.Contains("Ignore") => "Abandon definitif",
                    var e when e.Contains("Budget") => "Budget expired",
                    var e when e.Contains("Timeout") || e.Contains("timeout") || e.Contains("Canary") => "Timeout",
                    _ => "Autre"
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

- [ ] **Step 2: Update Main method to handle new return type**

In `Main` (around line 302-310), the `--parallel` branch currently does:

```csharp
var report = await RunParallelTest(...);
report.TotalElapsedMs = totalSw.ElapsedMilliseconds;
PrintReport(report);
```

Replace with:

```csharp
if (mode == "--parallel")
{
    int poolSize = 10; // default
    if (args.Length > 1 && int.TryParse(args[1], out int ps) && ps > 0)
        poolSize = ps;
    await RunParallelTest(metersWithKeys, dbOptions, dbSw.ElapsedMilliseconds, meters.Count, distinctIps, poolSize);
    // Report is printed inside RunParallelTest via PrintMultiPassReport
    return 0;
}
```

- [ ] **Step 3: Verify it compiles**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded with no errors

- [ ] **Step 4: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat: replace RunParallelTest with multi-pass orchestrator (3 passes, canary, adaptive timeouts)"
```

---

### Task 8: Smoke test and cleanup

**Files:**
- Verify: `DLMS_IntegrationTest/Program.cs`

- [ ] **Step 1: Verify full build**

Run: `dotnet build DLMS_IntegrationTest/`
Expected: Build succeeded, 0 errors, 0 warnings (or only pre-existing warnings)

- [ ] **Step 2: Verify --list mode still works**

Run: `dotnet run --project DLMS_IntegrationTest/ -- --list`
Expected: Displays help text with available modes (no crash)

- [ ] **Step 3: Review the final file for consistency**

Check that:
- `TestReport` class and `PrintReport` method still exist (used by `RunSequentialTest`)
- `ReadSingleMeter` is unchanged
- `RunSequentialTest` is unchanged
- `RunSingleMeterTest` is unchanged
- All new classes (`PassConfig`, `PassBudget`, `PassResult`, `MultiPassReport`) are present
- All new methods (`RunSinglePass`, `ParallelTcpScan`, `SingleTcpScan`, `ComputeAdaptiveTimeout`, `HasCache`, `PrintPassSummary`, `PrintMultiPassReport`) are present

- [ ] **Step 4: Final commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "chore: verify multi-pass integration test builds and existing modes unaffected"
```

---

## Summary of Tasks

| Task | Description | Estimated Size |
|------|-------------|---------------|
| 1 | Add `PassConfig` class | Small (20 lines) |
| 2 | Add `PassBudget` class | Small (15 lines) |
| 3 | Add `PassResult` + `MultiPassReport` | Small (25 lines) |
| 4 | Add static helpers (`HasCache`, `ComputeAdaptiveTimeout`, `ParallelTcpScan`, `SingleTcpScan`) | Medium (70 lines) |
| 5 | Implement `RunSinglePass` | Large (~200 lines) |
| 6 | Add `PrintPassSummary` + `PrintMultiPassReport` | Medium (80 lines) |
| 7 | Replace `RunParallelTest` orchestrator + wire Main | Medium (90 lines) |
| 8 | Smoke test and cleanup | Verification only |

**Total new/modified code:** ~500 lines (replacing ~350 lines of current `RunParallelTest`)
