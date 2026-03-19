using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using DLMS_MODELS;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DLMS_SERVICE.Services.MultiPass;

public interface IReadSessionOrchestrator
{
    Task<ReadSessionReport> RunSessionAsync(
        List<CompteurEquipement> metersToRead,
        MultiPassConfig config,
        int sessionNumber,
        int cycleId,
        CancellationToken ct);
}

public class ReadSessionOrchestrator : IReadSessionOrchestrator
{
    private readonly IDLMSGuruxSessionFactory _sessionFactory;
    private readonly IDLMSParallelReadService _parallelReadService;
    private readonly ITcpScanService _tcpScanService;
    private readonly IMeterHealthTracker _healthTracker;
    private readonly ILogger<ReadSessionOrchestrator> _logger;

    public ReadSessionOrchestrator(
        IDLMSGuruxSessionFactory sessionFactory,
        IDLMSParallelReadService parallelReadService,
        ITcpScanService tcpScanService,
        IMeterHealthTracker healthTracker,
        ILogger<ReadSessionOrchestrator> logger)
    {
        _sessionFactory = sessionFactory;
        _parallelReadService = parallelReadService;
        _tcpScanService = tcpScanService;
        _healthTracker = healthTracker;
        _logger = logger;
    }

    public async Task<ReadSessionReport> RunSessionAsync(
        List<CompteurEquipement> metersToRead,
        MultiPassConfig config,
        int sessionNumber,
        int cycleId,
        CancellationToken ct)
    {
        var report = new ReadSessionReport
        {
            SessionNumber = sessionNumber,
            CycleId = cycleId,
            TotalMetersInScope = metersToRead.Count
        };

        var successfulSerials = new HashSet<string>();
        var totalSw = Stopwatch.StartNew();
        var concentratorStats = new ConcentratorStats();

        _logger.LogInformation(
            "=== DEBUT SESSION #{SessionNumber} (Cycle #{CycleId}) — {Count} compteurs, {PassCount} passes ===",
            sessionNumber, cycleId, metersToRead.Count, config.Passes.Count);

        var currentMeters = metersToRead.ToList();
        HashSet<string>? previousDeferredIps = null;

        // Architecture V4: Pass 1 parallele + Pass RESCUE isolee (TCP frais par compteur)
        // Pas de Pass 2 parallele (rendement trop faible: 5-14 lus en 15 min)
        const int HardCeilingSeconds = 55 * 60; // 55 min hard ceiling

        // === PASS 1: Parallele ===
        var pass1Config = config.Passes.OrderBy(p => p.PassNumber).First();
        if (currentMeters.Count > 0)
        {
            var globalTimeLeft = Math.Min(
                config.GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds,
                HardCeilingSeconds - totalSw.Elapsed.TotalSeconds);

            if (globalTimeLeft >= 60)
            {
                var effectiveBudget = (int)Math.Min(pass1Config.BudgetSeconds, globalTimeLeft);
                var passResult = await RunSinglePassAsync(
                    currentMeters, pass1Config, effectiveBudget, config,
                    concentratorStats, previousDeferredIps, ct);

                report.Passes.Add(passResult);

                successfulSerials.UnionWith(
                    passResult.Results.Where(r => r.Success).Select(r => r.Serial));
                currentMeters = currentMeters
                    .Where(m => !successfulSerials.Contains(m.Compteur?.NumeroCompteur ?? ""))
                    .ToList();

                _logger.LogInformation(
                    "=== PASS 1 TERMINE ({Duration:F1} min) — {OK} lus, {Failed} echoues, {Deferred} differes ===",
                    passResult.ElapsedMs / 60000.0, passResult.Succeeded, passResult.Failed, passResult.DeferredCount);

                // Pause adaptative avant rescue
                if (currentMeters.Count > 0)
                {
                    var uniqueIps = currentMeters
                        .Select(m => m.Equipement?.AdresseIp)
                        .Where(ip => ip != null).Distinct().Count();

                    int pauseSeconds = uniqueIps <= 3 ? 0 : uniqueIps <= 10 ? 120 : pass1Config.PauseAfterSeconds;
                    if (pauseSeconds > 0)
                    {
                        _logger.LogInformation("Pause {PauseSec}s avant Rescue ({Remaining} compteurs, {IpCount} IPs)",
                            pauseSeconds, currentMeters.Count, uniqueIps);
                        await Task.Delay(TimeSpan.FromSeconds(pauseSeconds), ct);
                        report.TotalPauseMs += pauseSeconds * 1000;
                    }
                }
            }
        }

        // === PASS RESCUE: Isolee (TCP frais par compteur) ===
        if (currentMeters.Count > 0)
        {
            var globalTimeLeft = Math.Min(
                config.GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds,
                HardCeilingSeconds - totalSw.Elapsed.TotalSeconds);

            if (globalTimeLeft >= 60)
            {
                var rescueConfig = config.Passes.OrderBy(p => p.PassNumber).Last();
                var rescueBudget = (int)globalTimeLeft;

                var rescueResult = await RunSequentialRescuePassAsync(
                    currentMeters, rescueConfig, rescueBudget, config, report, ct);

                report.Passes.Add(rescueResult);

                successfulSerials.UnionWith(
                    rescueResult.Results.Where(r => r.Success).Select(r => r.Serial));
                currentMeters = currentMeters
                    .Where(m => !successfulSerials.Contains(m.Compteur?.NumeroCompteur ?? ""))
                    .ToList();

                _logger.LogInformation(
                    "=== RESCUE TERMINE ({Duration:F1} min) — {OK} lus, {Failed} echoues ===",
                    rescueResult.ElapsedMs / 60000.0, rescueResult.Succeeded, rescueResult.Failed);
            }
            else
            {
                _logger.LogWarning("Budget global insuffisant pour Rescue ({Elapsed}s)", (int)totalSw.Elapsed.TotalSeconds);
            }
        }

        // Categorize unread meters
        var allAttempted = report.AllResults
            .GroupBy(r => r.Serial)
            .ToDictionary(g => g.Key, g => g.Last());

        foreach (var meter in metersToRead.Where(m =>
            !successfulSerials.Contains(m.Compteur?.NumeroCompteur ?? "")))
        {
            var serial = meter.Compteur?.NumeroCompteur ?? "";
            string reason;
            if (allAttempted.TryGetValue(serial, out var lastResult))
            {
                reason = lastResult.Error switch
                {
                    var e when e.Contains("TCP") || e.Contains("pre-scan") => "IP morte",
                    var e when e.Contains("Abandon") || e.Contains("Ignore") => "Concentrateur instable",
                    var e when e.Contains("Budget") => "Budget expire",
                    var e when e.Contains("Timeout") || e.Contains("timeout") || e.Contains("Canary") => "Timeout",
                    var e when e.Contains("Cle") || e.Contains("cle") => "Cle manquante",
                    _ => "Autre"
                };
            }
            else
            {
                reason = "IP morte";
            }

            if (!report.UnreadByReason.ContainsKey(reason))
                report.UnreadByReason[reason] = new List<string>();
            report.UnreadByReason[reason].Add(serial);
        }

        totalSw.Stop();
        report.TotalElapsedMs = totalSw.ElapsedMilliseconds;
        report.TotalReadingMs = report.Passes.Sum(p => p.ElapsedMs);

        // Compute IP stats from results
        var allIps = metersToRead
            .Where(m => m.Equipement?.AdresseIp != null)
            .Select(m => $"{m.Equipement.AdresseIp}:{(m.Equipement.Port ?? "4059")}")
            .Distinct();
        report.TotalIps = allIps.Count();
        var accessibleIps = report.AllResults.Select(r => $"{r.Ip}:{r.Port}").Distinct();
        report.IpsAccessibles = accessibleIps.Count();

        _logger.LogInformation(
            "=== FIN SESSION #{SessionNumber} — {OK}/{Total} lus ({Rate:F1}%) en {Duration:F1} min ===",
            sessionNumber, report.TotalSucceeded, report.TotalMetersInScope,
            report.TauxReussite, report.TotalElapsedMs / 60000.0);

        return report;
    }

    private async Task<PassResult> RunSinglePassAsync(
        List<CompteurEquipement> metersToRead,
        PassConfig passConfig,
        int effectiveBudget,
        MultiPassConfig globalConfig,
        ConcentratorStats concentratorStats,
        HashSet<string>? previousDeferredIps,
        CancellationToken ct)
    {
        var passResult = new PassResult
        {
            PassNumber = passConfig.PassNumber,
            DateDebut = DateTime.Now
        };
        var budget = new PassBudget(effectiveBudget);
        var adaptivePool = new AdaptivePool(globalConfig.MaxConcurrentIps, minSize: 3, maxSize: 20);

        _logger.LogInformation(
            "=== PASS {PassNum}/3 ({Budget}s budget, {Count} compteurs) ===",
            passConfig.PassNumber, effectiveBudget, metersToRead.Count);

        // --- Step 1: Group by IP ---
        var ipGroups = metersToRead
            .Where(m => m.Equipement?.AdresseIp != null)
            .GroupBy(m => $"{m.Equipement.AdresseIp}:{(m.Equipement.Port ?? "4059")}")
            .ToList();

        // --- Step 2: TCP scan ---
        var reachableIps = new HashSet<string>();
        var tcpScanTimeout = TimeSpan.FromSeconds(globalConfig.TcpScanTimeoutSeconds);

        if (passConfig.PassNumber == 1 || previousDeferredIps == null)
        {
            // Pass 1: scan all IPs
            var allIps = ipGroups
                .Select(g => (g.First().Equipement.AdresseIp!, g.First().Equipement.Port ?? "4059"))
                .Distinct()
                .ToList();

            _logger.LogInformation("[Pre-scan] Test TCP de {Count} IPs (timeout {Timeout}s)",
                allIps.Count, globalConfig.TcpScanTimeoutSeconds);

            var scanResults = await _tcpScanService.ParallelTcpScanAsync(allIps, tcpScanTimeout);
            reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));

            // Retry failed (up to 10)
            var failedScan = scanResults.Where(r => !r.Reachable).ToList();
            if (failedScan.Count > 0 && failedScan.Count <= 10)
            {
                _logger.LogDebug("[Pre-scan] Retry de {Count} IPs echouees", failedScan.Count);
                foreach (var f in failedScan)
                {
                    var parts = f.Key.Split(':');
                    var retry = await _tcpScanService.SingleTcpScanAsync(parts[0], parts[1], tcpScanTimeout);
                    if (retry.Reachable) reachableIps.Add(retry.Key);
                }
            }
        }
        else
        {
            // Pass 2/3: only re-scan deferred IPs
            var ipsNeedingScan = ipGroups
                .Where(g => previousDeferredIps.Contains($"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}"))
                .Select(g => (g.First().Equipement.AdresseIp!, g.First().Equipement.Port ?? "4059"))
                .Distinct()
                .ToList();

            if (ipsNeedingScan.Count > 0)
            {
                _logger.LogInformation("[Re-scan] Test TCP de {Count} IPs differees", ipsNeedingScan.Count);
                var scanResults = await _tcpScanService.ParallelTcpScanAsync(ipsNeedingScan, tcpScanTimeout);
                reachableIps.UnionWith(scanResults.Where(r => r.Reachable).Select(r => r.Key));
            }

            // Previously reachable IPs don't need re-scan
            var previouslyReachableIps = ipGroups
                .Select(g => $"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}")
                .Where(ip => !previousDeferredIps.Contains(ip));
            reachableIps.UnionWith(previouslyReachableIps);
        }

        // Defer unreachable IPs
        foreach (var g in ipGroups.Where(g =>
            !reachableIps.Contains($"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}")))
        {
            var ipKey = $"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}";
            passResult.DeferIp(ipKey);
            foreach (var m in g) passResult.DeferMeter(m);
        }

        _logger.LogInformation("Pre-scan: {Reachable}/{Total} IPs accessibles",
            reachableIps.Count, ipGroups.Count);

        // --- Step 3: Sort reachable IPs ---
        var reachableGroups = ipGroups
            .Where(g => reachableIps.Contains($"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}"))
            .Select(g => (
                Key: $"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}",
                Meters: g.OrderByDescending(m => HasAssociationCache(m) ? 1 : 0).ToList()))
            .OrderBy(g => g.Meters.Count <= 5 ? 0 : 1)
            .ThenByDescending(g => g.Meters.Count(m => HasAssociationCache(m)))
            .ThenBy(g => g.Meters.Count)
            .ToList();

        // --- Step 4: Process each IP in parallel ---
        int completedIps = 0;

        var tasks = reachableGroups.Select(entry => Task.Run(async () =>
        {
            await adaptivePool.WaitAsync();
            try
            {
                if (budget.IsExpired)
                {
                    _logger.LogDebug("[{IpKey}] Budget expire, {Count} compteurs differes",
                        entry.Key, entry.Meters.Count);

                    if (passConfig.PassNumber >= 3)
                    {
                        foreach (var m in entry.Meters)
                        {
                            passResult.Results.Add(new MeterReadOutcome
                            {
                                Serial = m.Compteur?.NumeroCompteur ?? "",
                                Ip = m.Equipement?.AdresseIp ?? "",
                                Port = m.Equipement?.Port ?? "",
                                CompteurEquipementId = m.Id,
                                Error = "Budget expired",
                                ResultCategory = MeterReadingResult.BudgetExpire
                            });
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

                // Open TCP transport
                var transportParams = new DLMSConnectionParameters
                {
                    AddressIp = firstMeter.Equipement.AdresseIp!,
                    Port = firstMeter.Equipement.Port ?? "4059",
                    InterfaceType = "HDLC"
                };

                var session = _sessionFactory.CreateSession(transportParams);
                using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                using var tcpLinked = CancellationTokenSource.CreateLinkedTokenSource(ct, tcpCts.Token);

                var tcpOk = await session.OpenTransportAsync(tcpLinked.Token);
                if (!tcpOk)
                {
                    _logger.LogDebug("[{IpKey}] TCP echec, {Count} compteurs differes",
                        ipKey, meterList.Count);
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

                    _logger.LogDebug("[{IpKey}] Canary {Serial} (timeout:{Timeout}s)",
                        ipKey, canaryMeter.Compteur?.NumeroCompteur, canaryTimeout);

                    var canarySw = Stopwatch.StartNew();
                    MeterReadOutcome canaryResult;
                    try
                    {
                        canaryResult = await _parallelReadService.ReadSingleMeterOnSessionAsync(
                            session, canaryMeter, TimeSpan.FromSeconds(canaryTimeout), ct);
                    }
                    catch (Exception ex)
                    {
                        canaryResult = new MeterReadOutcome
                        {
                            Serial = canaryMeter.Compteur?.NumeroCompteur ?? "",
                            Ip = canaryMeter.Equipement?.AdresseIp ?? "",
                            Port = canaryMeter.Equipement?.Port ?? "",
                            CompteurEquipementId = canaryMeter.Id,
                            Error = $"Canary exception: {ex.Message}",
                            ResultCategory = MeterReadingResult.EchecLecture
                        };
                    }
                    canarySw.Stop();

                    passResult.Results.Add(canaryResult);
                    concentratorStats.Record(ipKey, canaryResult.Success, canaryResult.TotalMs);

                    if (!canaryResult.Success)
                    {
                        if (passConfig.PassNumber < 3)
                        {
                            _logger.LogDebug("[{IpKey}] Canary ECHEC — {Count} compteurs differes",
                                ipKey, meterList.Count - 1);
                            for (int i = 1; i < meterList.Count; i++)
                                passResult.DeferMeter(meterList[i]);
                            passResult.DeferIp(ipKey);
                        }
                        else
                        {
                            _logger.LogDebug("[{IpKey}] Canary ECHEC Pass 3 — abandon definitif",
                                ipKey);
                            for (int i = 1; i < meterList.Count; i++)
                            {
                                passResult.Results.Add(new MeterReadOutcome
                                {
                                    Serial = meterList[i].Compteur?.NumeroCompteur ?? "",
                                    Ip = meterList[i].Equipement?.AdresseIp ?? "",
                                    Port = meterList[i].Equipement?.Port ?? "",
                                    CompteurEquipementId = meterList[i].Id,
                                    Error = "Abandon definitif (canary echec Pass 3)",
                                    ResultCategory = MeterReadingResult.AbandonDefinitif
                                });
                            }
                        }
                        return;
                    }

                    var canaryLatencyMs = canarySw.ElapsedMilliseconds;
                    _logger.LogDebug("[{IpKey}] Canary OK ({LatencyMs}ms) — lecture de {Count} compteurs restants",
                        ipKey, canaryLatencyMs, meterList.Count - 1);

                    // --- Read remaining meters ---
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

                        var hasCacheFile = HasAssociationCache(meter);
                        var adaptiveTimeout = ComputeAdaptiveTimeout(
                            canaryLatencyMs, hasCacheFile, passConfig, globalConfig);
                        var clampedTimeout = budget.ClampTimeout(adaptiveTimeout);
                        if (clampedTimeout == -1)
                        {
                            for (int j = i; j < meterList.Count; j++)
                                passResult.DeferMeter(meterList[j]);
                            break;
                        }

                        MeterReadOutcome meterResult;
                        try
                        {
                            meterResult = await _parallelReadService.ReadSingleMeterOnSessionAsync(
                                session, meter, TimeSpan.FromSeconds(clampedTimeout), ct);
                        }
                        catch (Exception ex)
                        {
                            meterResult = new MeterReadOutcome
                            {
                                Serial = meter.Compteur?.NumeroCompteur ?? "",
                                Ip = meter.Equipement?.AdresseIp ?? "",
                                Port = meter.Equipement?.Port ?? "",
                                CompteurEquipementId = meter.Id,
                                Error = ex.Message,
                                ResultCategory = MeterReadingResult.EchecLecture
                            };
                        }

                        passResult.Results.Add(meterResult);
                        concentratorStats.Record(ipKey, meterResult.Success, meterResult.TotalMs);

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
                                    _logger.LogDebug("[{IpKey}] Cooldown {CooldownS}s apres {MaxFails} echecs",
                                        ipKey, passConfig.CooldownSeconds, passConfig.MaxConsecutiveFailures);
                                    await Task.Delay(TimeSpan.FromSeconds(passConfig.CooldownSeconds), ct);
                                    consecutiveFailures = 0;
                                    cooldownsUsed++;
                                }
                                else if (passConfig.PassNumber < 3)
                                {
                                    _logger.LogDebug("[{IpKey}] IP differee apres {MaxFails} echecs consecutifs",
                                        ipKey, passConfig.MaxConsecutiveFailures);
                                    for (int j = i + 1; j < meterList.Count; j++)
                                        passResult.DeferMeter(meterList[j]);
                                    passResult.DeferIp(ipKey);
                                    break;
                                }
                                else
                                {
                                    _logger.LogDebug("[{IpKey}] ABANDON DEFINITIF apres {MaxFails} echecs consecutifs",
                                        ipKey, passConfig.MaxConsecutiveFailures);
                                    for (int j = i + 1; j < meterList.Count; j++)
                                    {
                                        passResult.Results.Add(new MeterReadOutcome
                                        {
                                            Serial = meterList[j].Compteur?.NumeroCompteur ?? "",
                                            Ip = meterList[j].Equipement?.AdresseIp ?? "",
                                            Port = meterList[j].Equipement?.Port ?? "",
                                            CompteurEquipementId = meterList[j].Id,
                                            Error = "Abandon definitif",
                                            ResultCategory = MeterReadingResult.AbandonDefinitif
                                        });
                                    }
                                    break;
                                }
                            }
                        }

                        // Pacing delay
                        await Task.Delay(globalConfig.PacingDelayMs, ct);
                    }
                }
                finally
                {
                    await session.DisconnectAsync();
                }

                // Log summary for this IP
                var ok = passResult.Results.Count(r =>
                    r.Success && r.Ip == firstMeter.Equipement?.AdresseIp);
                var ipNum = Interlocked.Increment(ref completedIps);
                _logger.LogDebug("[IP {Num}/{Total}] {IpKey} : {OK}/{Count} OK",
                    ipNum, reachableGroups.Count, ipKey, ok, meterList.Count);

                // Adaptive pool adjustment every 5 completed IPs
                if (ipNum % 5 == 0)
                {
                    var (successRate, avgLatencyMs, total) = concentratorStats.GetGlobalStats();
                    if (successRate > 80 && avgLatencyMs < 60000)
                        adaptivePool.Expand(2);
                    else if (successRate < 50 || avgLatencyMs > 120000)
                        adaptivePool.Shrink(2);
                }
            }
            finally
            {
                adaptivePool.Release();
            }
        }, ct)).ToList();

        await Task.WhenAll(tasks);

        passResult.ElapsedMs = budget.ElapsedMs;
        passResult.DateFin = DateTime.Now;
        return passResult;
    }

    /// <summary>
    /// Pass RESCUE : lecture isolee de chaque compteur — connexion TCP+HDLC fraiche par compteur.
    /// Identique au comportement de --seq --meter qui lit 95% des compteurs "difficiles".
    /// Pas de canary, pas d'abandon, pas de retry, pas de session partagee.
    /// </summary>
    private async Task<PassResult> RunSequentialRescuePassAsync(
        List<CompteurEquipement> metersToRead,
        PassConfig passConfig,
        int effectiveBudget,
        MultiPassConfig globalConfig,
        ReadSessionReport sessionReport,
        CancellationToken ct)
    {
        var passResult = new PassResult
        {
            PassNumber = passConfig.PassNumber,
            DateDebut = DateTime.Now
        };
        var budget = new PassBudget(effectiveBudget);
        const int CachedTimeoutSeconds = 30;    // Compteur avec cache: lecture rapide
        const int UncachedTimeoutSeconds = 120;  // Compteur sans cache: investir du temps pour constituer le cache

        _logger.LogInformation(
            "=== PASS RESCUE ISOLEE ({Budget}s budget, {Count} compteurs, TCP frais par compteur, cache-building) ===",
            effectiveBudget, metersToRead.Count);

        // TCP scan to identify reachable IPs (avoid wasting time on dead IPs)
        var ipGroups = metersToRead
            .Where(m => m.Equipement?.AdresseIp != null)
            .GroupBy(m => $"{m.Equipement.AdresseIp}:{(m.Equipement.Port ?? "4059")}")
            .ToList();

        var allIps = ipGroups
            .Select(g => (g.First().Equipement.AdresseIp!, g.First().Equipement.Port ?? "4059"))
            .Distinct().ToList();

        var tcpScanTimeout = TimeSpan.FromSeconds(globalConfig.TcpScanTimeoutSeconds);
        _logger.LogInformation("[Rescue] Test TCP de {Count} IPs", allIps.Count);
        var scanResults = await _tcpScanService.ParallelTcpScanAsync(allIps, tcpScanTimeout);
        var reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));
        _logger.LogInformation("[Rescue] {Reachable}/{Total} IPs accessibles", reachableIps.Count, allIps.Count);

        // Build flat list: cached meters first (quick reads), uncached after (cache-building)
        var ipSuccessRates = sessionReport.AllResults
            .GroupBy(r => $"{r.Ip}:{r.Port}")
            .Where(g => !string.IsNullOrEmpty(g.Key) && g.Key != ":")
            .ToDictionary(g => g.Key,
                g => g.Count() > 0 ? (double)g.Count(r => r.Success) / g.Count() * 100 : 0);

        var sortedMeters = metersToRead
            .Where(m => m.Equipement?.AdresseIp != null)
            .Select(m => new
            {
                Meter = m,
                Serial = m.Compteur?.NumeroCompteur ?? "",
                IpKey = $"{m.Equipement.AdresseIp}:{(m.Equipement.Port ?? "4059")}",
                HasCache = File.Exists(Path.Combine("associations", $"{m.Compteur?.NumeroCompteur}_Read.xml")),
                IpReachable = reachableIps.Contains($"{m.Equipement.AdresseIp}:{(m.Equipement.Port ?? "4059")}")
            })
            .OrderByDescending(m => m.IpReachable ? 1 : 0)      // Reachable first
            .ThenByDescending(m => m.HasCache ? 1 : 0)            // Cached first (quick reads)
            .ThenByDescending(m => ipSuccessRates.TryGetValue(m.IpKey, out var r) ? r : 0) // Best IPs first
            .ToList();

        var cachedCount = sortedMeters.Count(m => m.HasCache && m.IpReachable);
        var uncachedCount = sortedMeters.Count(m => !m.HasCache && m.IpReachable);
        var deadCount = sortedMeters.Count(m => !m.IpReachable);
        _logger.LogInformation(
            "[Rescue] {Cached} avec cache (timeout {CachedT}s), {Uncached} sans cache (timeout {UncachedT}s, cache-building), {Dead} IP mortes",
            cachedCount, CachedTimeoutSeconds, uncachedCount, UncachedTimeoutSeconds, deadCount);

        int okCount = 0;
        int failCount = 0;
        int skipCount = 0;
        int cacheBuilt = 0;

        // Process each meter with a FULLY ISOLATED session (fresh TCP + HDLC per meter)
        foreach (var entry in sortedMeters)
        {
            var meter = entry.Meter;
            var serial = entry.Serial;

            // Budget check
            if (budget.IsExpired)
            {
                passResult.Results.Add(new MeterReadOutcome
                {
                    Serial = serial, Ip = meter.Equipement?.AdresseIp ?? "",
                    Port = meter.Equipement?.Port ?? "", CompteurEquipementId = meter.Id,
                    Error = "Budget expire (rescue)", ResultCategory = MeterReadingResult.BudgetExpire
                });
                skipCount++;
                continue;
            }

            // Skip TCP-dead IPs instantly
            if (!entry.IpReachable)
            {
                passResult.Results.Add(new MeterReadOutcome
                {
                    Serial = serial, Ip = meter.Equipement?.AdresseIp ?? "",
                    Port = meter.Equipement?.Port ?? "", CompteurEquipementId = meter.Id,
                    Error = "IP inaccessible (rescue)", ResultCategory = MeterReadingResult.IpMorte
                });
                skipCount++;
                continue;
            }

            // Timeout adaptatif: court si cache existe, long sinon (investissement cache-building)
            var timeout = entry.HasCache ? CachedTimeoutSeconds : UncachedTimeoutSeconds;

            // Read meter in FULLY ISOLATED mode
            var meterResult = await _parallelReadService.ReadMeterIsolatedAsync(meter, timeout);
            passResult.Results.Add(meterResult);

            if (meterResult.Success)
            {
                okCount++;
                // Vérifier si un cache a été constitué (nouveau fichier créé)
                var cacheFile = Path.Combine("associations", $"{serial}_Read.xml");
                if (!entry.HasCache && File.Exists(cacheFile))
                {
                    cacheBuilt++;
                    _logger.LogInformation("[Rescue] {Serial} OK + CACHE CONSTITUE ({TotalMs}ms) — les prochaines lectures seront rapides",
                        serial, meterResult.TotalMs);
                }
                else
                {
                    _logger.LogDebug("[Rescue] {Serial} OK ({TotalMs}ms){CacheTag}",
                        serial, meterResult.TotalMs, entry.HasCache ? " [cache]" : "");
                }
            }
            else
            {
                failCount++;
                _logger.LogDebug("[Rescue] {Serial} ECHEC — {Error} ({TotalMs}ms, timeout={Timeout}s{CacheTag})",
                    serial, meterResult.Error, meterResult.TotalMs, timeout, entry.HasCache ? ", cache" : ", no-cache");
            }

            // Pacing between meters (let concentrator breathe)
            await Task.Delay(200, ct);
        }

        _logger.LogInformation(
            "[Rescue] Resultat: {OK} lus, {Fail} echoues, {Skip} non traites, {CacheBuilt} caches constitues",
            okCount, failCount, skipCount, cacheBuilt);

        passResult.ElapsedMs = budget.ElapsedMs;
        passResult.DateFin = DateTime.Now;
        return passResult;
    }

    private bool HasAssociationCache(CompteurEquipement meter)
    {
        var serial = meter.Compteur?.NumeroCompteur;
        if (string.IsNullOrEmpty(serial)) return false;
        return _healthTracker.GetCategory(serial) == MeterPerformanceCategory.Fast
            || _healthTracker.GetCategory(serial) == MeterPerformanceCategory.Medium;
    }

    private int ComputeAdaptiveTimeout(
        long canaryLatencyMs, bool hasCacheFile,
        PassConfig passConfig, MultiPassConfig globalConfig)
    {
        var raw = (int)(canaryLatencyMs / 1000.0 * globalConfig.AdaptiveTimeoutMultiplier);
        var ceiling = hasCacheFile ? passConfig.CachedTimeoutSeconds : passConfig.UncachedTimeoutSeconds;
        return Math.Clamp(raw, globalConfig.MinAdaptiveTimeoutSeconds, ceiling);
    }

}
