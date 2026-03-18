using System.Collections.Concurrent;
using System.Diagnostics;
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

        // Passes 1 & 2: parallel. Pass 3: sequential rescue (no canary, no abandon)
        var orderedPasses = config.Passes.OrderBy(p => p.PassNumber).ToList();

        foreach (var passConfig in orderedPasses)
        {
            if (currentMeters.Count == 0)
            {
                _logger.LogInformation("Tous les compteurs ont ete lus, arret anticipe");
                break;
            }

            // Check global budget (hard ceiling 55 min to leave margin)
            const int HardCeilingSeconds = 55 * 60; // 55 min = 3300s
            var globalTimeLeft = Math.Min(
                config.GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds,
                HardCeilingSeconds - totalSw.Elapsed.TotalSeconds);
            if (globalTimeLeft < 60)
            {
                _logger.LogWarning("Budget global expire ({Elapsed}s), arret des passes",
                    (int)totalSw.Elapsed.TotalSeconds);
                break;
            }
            var effectiveBudget = (int)Math.Min(passConfig.BudgetSeconds, globalTimeLeft);

            PassResult passResult;

            if (passConfig.PassNumber >= 3)
            {
                // Pass 3+: sequential rescue — use ALL remaining global time (up to 55 min hard ceiling)
                var rescueBudget = (int)globalTimeLeft;
                passResult = await RunSequentialRescuePassAsync(
                    currentMeters, passConfig, rescueBudget, config, report, ct);
            }
            else
            {
                // Pass 1-2: parallel with canary and IP logic
                passResult = await RunSinglePassAsync(
                    currentMeters, passConfig, effectiveBudget, config,
                    concentratorStats, previousDeferredIps, ct);
            }

            report.Passes.Add(passResult);

            // Remove successes, keep failures for next pass
            successfulSerials.UnionWith(
                passResult.Results.Where(r => r.Success).Select(r => r.Serial));
            currentMeters = currentMeters
                .Where(m => !successfulSerials.Contains(m.Compteur?.NumeroCompteur ?? ""))
                .ToList();
            previousDeferredIps = passResult.GetDeferredIpSet();

            var passLabel = passConfig.PassNumber >= 3 ? "RESCUE" : $"PASS {passConfig.PassNumber}";
            _logger.LogInformation(
                "=== {Label}/{Total} TERMINE ({Duration:F1} min) — {OK} lus, {Failed} echoues, {Deferred} differes ===",
                passLabel, orderedPasses.Count, passResult.ElapsedMs / 60000.0,
                passResult.Succeeded, passResult.Failed, passResult.DeferredCount);

            // Inter-pass pause (adaptive: skip if few IPs)
            if (passConfig.PauseAfterSeconds > 0 && currentMeters.Count > 0)
            {
                var uniqueIps = currentMeters
                    .Select(m => m.Equipement?.AdresseIp)
                    .Where(ip => ip != null)
                    .Distinct().Count();

                int actualPauseSeconds;
                if (uniqueIps <= 3)
                    actualPauseSeconds = 0;
                else if (uniqueIps <= 10)
                    actualPauseSeconds = 120;
                else
                    actualPauseSeconds = passConfig.PauseAfterSeconds;

                if (actualPauseSeconds > 0)
                {
                    _logger.LogInformation("Pause {PauseSec}s avant Pass {NextPass} ({Remaining} compteurs, {IpCount} IPs)",
                        actualPauseSeconds, passConfig.PassNumber + 1, currentMeters.Count, uniqueIps);
                    await Task.Delay(TimeSpan.FromSeconds(actualPauseSeconds), ct);
                    report.TotalPauseMs += actualPauseSeconds * 1000;
                }
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
    /// Pass 3 "rescue" : lecture sequentielle de tous les compteurs restants.
    /// Pas de canary, pas d'abandon d'IP, pas de logique d'echecs consecutifs.
    /// Chaque compteur a sa chance avec un timeout individuel de 180s.
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
        const int MeterTimeoutSeconds = 60; // Réduit: si un compteur ne répond pas en 60s, inutile d'attendre 180s

        _logger.LogInformation(
            "=== PASS RESCUE ({Budget}s budget, {Count} compteurs, mode sequentiel) ===",
            effectiveBudget, metersToRead.Count);

        // Group by IP
        var ipGroups = metersToRead
            .Where(m => m.Equipement?.AdresseIp != null)
            .GroupBy(m => $"{m.Equipement.AdresseIp}:{(m.Equipement.Port ?? "4059")}")
            .ToList();

        // Compute IP success rate from previous passes to sort rescue order
        var ipSuccessRates = sessionReport.AllResults
            .GroupBy(r => $"{r.Ip}:{r.Port}")
            .Where(g => !string.IsNullOrEmpty(g.Key) && g.Key != ":")
            .ToDictionary(
                g => g.Key,
                g => g.Count() > 0 ? (double)g.Count(r => r.Success) / g.Count() * 100 : 0);

        // TCP scan all IPs
        var allIps = ipGroups
            .Select(g => (g.First().Equipement.AdresseIp!, g.First().Equipement.Port ?? "4059"))
            .Distinct()
            .ToList();

        var tcpScanTimeout = TimeSpan.FromSeconds(globalConfig.TcpScanTimeoutSeconds);
        _logger.LogInformation("[Rescue] Test TCP de {Count} IPs", allIps.Count);
        var scanResults = await _tcpScanService.ParallelTcpScanAsync(allIps, tcpScanTimeout);
        var reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));

        _logger.LogInformation("[Rescue] {Reachable}/{Total} IPs accessibles", reachableIps.Count, allIps.Count);

        // Mark unreachable meters
        foreach (var g in ipGroups.Where(g =>
            !reachableIps.Contains($"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}")))
        {
            foreach (var m in g)
            {
                passResult.Results.Add(new MeterReadOutcome
                {
                    Serial = m.Compteur?.NumeroCompteur ?? "",
                    Ip = m.Equipement?.AdresseIp ?? "",
                    Port = m.Equipement?.Port ?? "",
                    CompteurEquipementId = m.Id,
                    Error = "IP inaccessible (rescue)",
                    ResultCategory = MeterReadingResult.EchecLecture
                });
            }
        }

        // Process reachable IPs sequentially — sorted by success rate (best first, dead last)
        var sortedReachableGroups = ipGroups
            .Where(g => reachableIps.Contains($"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}"))
            .OrderByDescending(g =>
            {
                var ipKey = $"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}";
                return ipSuccessRates.TryGetValue(ipKey, out var rate) ? rate : -1;
            })
            .ToList();

        _logger.LogInformation("[Rescue] Ordre de traitement: {Order}",
            string.Join(", ", sortedReachableGroups.Select(g =>
            {
                var ipKey = $"{g.First().Equipement.AdresseIp}:{(g.First().Equipement.Port ?? "4059")}";
                var rate = ipSuccessRates.TryGetValue(ipKey, out var r) ? r : 0;
                return $"{ipKey}({rate:F0}%)";
            })));

        foreach (var group in sortedReachableGroups)
        {
            if (budget.IsExpired)
            {
                _logger.LogWarning("[Rescue] Budget expire, {Count} compteurs restants non traites",
                    group.Count());
                foreach (var m in group)
                {
                    passResult.Results.Add(new MeterReadOutcome
                    {
                        Serial = m.Compteur?.NumeroCompteur ?? "",
                        Ip = m.Equipement?.AdresseIp ?? "",
                        Port = m.Equipement?.Port ?? "",
                        CompteurEquipementId = m.Id,
                        Error = "Budget expire (rescue)",
                        ResultCategory = MeterReadingResult.BudgetExpire
                    });
                }
                continue;
            }

            var firstMeter = group.First();
            var ipKey = $"{firstMeter.Equipement.AdresseIp}:{(firstMeter.Equipement.Port ?? "4059")}";

            // Open TCP transport
            var transportParams = new DLMSConnectionParameters
            {
                AddressIp = firstMeter.Equipement.AdresseIp!,
                Port = firstMeter.Equipement.Port ?? "4059",
                InterfaceType = "HDLC"
            };

            var session = _sessionFactory.CreateSession(transportParams);
            using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            using var tcpLinked = CancellationTokenSource.CreateLinkedTokenSource(ct, tcpCts.Token);

            var tcpOk = await session.OpenTransportAsync(tcpLinked.Token);
            if (!tcpOk)
            {
                _logger.LogWarning("[Rescue] TCP {IpKey} echec, {Count} compteurs non lus", ipKey, group.Count());
                foreach (var m in group)
                {
                    passResult.Results.Add(new MeterReadOutcome
                    {
                        Serial = m.Compteur?.NumeroCompteur ?? "",
                        Ip = m.Equipement?.AdresseIp ?? "",
                        Port = m.Equipement?.Port ?? "",
                        CompteurEquipementId = m.Id,
                        Error = "TCP echec (rescue)",
                        ResultCategory = MeterReadingResult.EchecLecture
                    });
                }
                continue;
            }

            _logger.LogInformation("[Rescue] {IpKey} connecte — lecture de {Count} compteurs", ipKey, group.Count());

            int okCount = 0;
            foreach (var meter in group)
            {
                if (budget.IsExpired)
                {
                    passResult.Results.Add(new MeterReadOutcome
                    {
                        Serial = meter.Compteur?.NumeroCompteur ?? "",
                        Ip = meter.Equipement?.AdresseIp ?? "",
                        Port = meter.Equipement?.Port ?? "",
                        CompteurEquipementId = meter.Id,
                        Error = "Budget expire (rescue)",
                        ResultCategory = MeterReadingResult.BudgetExpire
                    });
                    continue;
                }

                var serial = meter.Compteur?.NumeroCompteur ?? "";
                MeterReadOutcome meterResult;
                try
                {
                    meterResult = await _parallelReadService.ReadSingleMeterOnSessionAsync(
                        session, meter, TimeSpan.FromSeconds(MeterTimeoutSeconds), ct);
                }
                catch (Exception ex)
                {
                    meterResult = new MeterReadOutcome
                    {
                        Serial = serial,
                        Ip = meter.Equipement?.AdresseIp ?? "",
                        Port = meter.Equipement?.Port ?? "",
                        CompteurEquipementId = meter.Id,
                        Error = ex.Message,
                        ResultCategory = MeterReadingResult.EchecLecture
                    };
                }

                passResult.Results.Add(meterResult);

                if (meterResult.Success)
                {
                    okCount++;
                    _logger.LogDebug("[Rescue] {Serial} OK ({TotalMs}ms)", serial, meterResult.TotalMs);
                }
                else
                {
                    _logger.LogDebug("[Rescue] {Serial} ECHEC — {Error}", serial, meterResult.Error);

                    // Reconnexion TCP complète après échec pour éviter la cascade
                    // (la session HDLC est corrompue après un timeout GetAssociationView)
                    try { await session.DisconnectAsync(); } catch { }
                    await Task.Delay(500, ct); // laisser le concentrateur respirer

                    session = _sessionFactory.CreateSession(transportParams);
                    using var reconnCts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
                    using var reconnLinked = CancellationTokenSource.CreateLinkedTokenSource(ct, reconnCts.Token);
                    var reconnOk = await session.OpenTransportAsync(reconnLinked.Token);
                    if (!reconnOk)
                    {
                        _logger.LogWarning("[Rescue] Reconnexion TCP {IpKey} echouee, skip {Count} compteurs restants",
                            ipKey, group.Count() - okCount - passResult.Results.Count(r => r.Ip == firstMeter.Equipement?.AdresseIp && !r.Success));

                        // Marquer les compteurs restants non encore traités
                        foreach (var remaining in group.Where(m =>
                            !passResult.Results.Any(r => r.Serial == (m.Compteur?.NumeroCompteur ?? ""))))
                        {
                            passResult.Results.Add(new MeterReadOutcome
                            {
                                Serial = remaining.Compteur?.NumeroCompteur ?? "",
                                Ip = remaining.Equipement?.AdresseIp ?? "",
                                Port = remaining.Equipement?.Port ?? "",
                                CompteurEquipementId = remaining.Id,
                                Error = "Reconnexion TCP echouee (rescue)",
                                ResultCategory = MeterReadingResult.EchecLecture
                            });
                        }
                        break; // Passer à l'IP suivante
                    }
                    _logger.LogDebug("[Rescue] Reconnexion TCP {IpKey} OK", ipKey);
                }

                // Pacing delay
                await Task.Delay(globalConfig.PacingDelayMs, ct);
            }

            // Disconnect final
            try { await session.DisconnectAsync(); } catch { }

            _logger.LogInformation("[Rescue] {IpKey} : {OK}/{Total} OK",
                ipKey, okCount, group.Count());
        }

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
