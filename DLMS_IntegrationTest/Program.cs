using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using DLMS_DAL.Datas;
using DLMS_MODELS;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_SERVICE.Services;
using DLMS_COMMUNICATION.Reader;

namespace DLMS_IntegrationTest;

// ===== Data classes =====

class MeterInfo
{
    public CompteurEquipement CompteurEquipement { get; set; } = null!;
    public string Serial { get; set; } = "";
    public string Ip { get; set; } = "";
    public string Port { get; set; } = "4059";
    public bool HasKeys { get; set; }
    public int Index { get; set; }
}

class MeterResult
{
    public string Serial { get; set; } = "";
    public string Ip { get; set; } = "";
    public string Port { get; set; } = "";
    public long KeysRetrievalMs { get; set; }
    public long TcpMs { get; set; }
    public long HdlcMs { get; set; }
    public long ReadMs { get; set; }
    public bool Success { get; set; }
    public string Error { get; set; } = "";
    public string? ReadData { get; set; }
}

class IpGroupResult
{
    public string Ip { get; set; } = "";
    public string Port { get; set; } = "";
    public long TcpMs { get; set; }
    public bool TcpSuccess { get; set; }
    public string TcpError { get; set; } = "";
    public List<MeterResult> MeterResults { get; set; } = new();
}

class TestReport
{
    public string Mode { get; set; } = "";
    public long DbLoadMs { get; set; }
    public int TotalMeters { get; set; }
    public int TotalIps { get; set; }
    public int MetersWithKeys { get; set; }
    public List<IpGroupResult> IpGroups { get; set; } = new();
    public long TotalElapsedMs { get; set; }

    public int MetersTested => IpGroups.SelectMany(g => g.MeterResults).Count();
    public int MetersSucceeded => IpGroups.SelectMany(g => g.MeterResults).Count(r => r.Success);
    public int MetersFailed => MetersTested - MetersSucceeded;
    public int TcpSucceeded => IpGroups.Count(g => g.TcpSuccess);
    public int TcpFailed => IpGroups.Count(g => !g.TcpSuccess);

    public double AvgHdlcMs => IpGroups.SelectMany(g => g.MeterResults)
        .Where(r => r.Success && r.HdlcMs > 0)
        .Select(r => (double)r.HdlcMs)
        .DefaultIfEmpty(0)
        .Average();

    public double AvgReadMs => IpGroups.SelectMany(g => g.MeterResults)
        .Where(r => r.Success && r.ReadMs > 0)
        .Select(r => (double)r.ReadMs)
        .DefaultIfEmpty(0)
        .Average();

    public double AvgTotalPerMeterMs => IpGroups.SelectMany(g => g.MeterResults)
        .Where(r => r.Success)
        .Select(r => (double)(r.HdlcMs + r.ReadMs + r.KeysRetrievalMs))
        .DefaultIfEmpty(0)
        .Average();

    public ConcentratorStats? Stats { get; set; }
}

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

class PassResult
{
    public int PassNumber;
    public List<MeterResult> Results = new();
    public ConcurrentBag<MeterInfo> DeferredMeters = new();
    public ConcurrentDictionary<string, byte> DeferredIps = new();
    public long ElapsedMs;

    public int Succeeded => Results.Count(r => r.Success);
    public int Failed => Results.Count(r => !r.Success);
    public int DeferredCount => DeferredMeters.Count;
    public int InScope => Results.Count + DeferredCount;

    public void DeferIp(string ipKey) => DeferredIps.TryAdd(ipKey, 0);
    public void DeferMeter(MeterInfo m) => DeferredMeters.Add(m);
    public HashSet<string> GetDeferredIpSet() => DeferredIps.Keys.ToHashSet();
}

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

// ===== KPI monitoring =====

class ConcentratorStats
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

    public Dictionary<string, IpStats> GetAllStats()
    {
        return _stats.ToDictionary(kv => kv.Key, kv => kv.Value);
    }

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

class AdaptivePool
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

// ===== Main program =====

class Program
{
    private static ILoggerFactory _loggerFactory = null!;

    static async Task<int> Main(string[] args)
    {
        var totalSw = Stopwatch.StartNew();

        // Prevent thread pool starvation from synchronous Gurux HDLC calls
        ThreadPool.SetMinThreads(50, 50);

        Console.WriteLine("=== Test d'integration DLMS/COSEM ===");
        Console.WriteLine();

        // Créer le dossier cache pour les association views
        Directory.CreateDirectory("associations");

        // Setup logging — reduce to Warning for batch modes, keep Debug for single meter
        var isBatchMode = args.Length > 0 && (args[0] == "--seq" || args[0] == "--parallel");
        var minLevel = isBatchMode ? Serilog.Events.LogEventLevel.Warning : Serilog.Events.LogEventLevel.Debug;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(minLevel)
            .WriteTo.Console(outputTemplate: "  [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(Log.Logger);
            builder.SetMinimumLevel(isBatchMode ? LogLevel.Warning : LogLevel.Debug);
        });

        try
        {
            // ===== Setup DB =====
            var dbSw = Stopwatch.StartNew();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = GetConnectionString(configuration);
            GlobalVariable.ConString = connectionString;

            var dbOptions = new DbContextOptionsBuilder<DLMSDBContext>()
                .UseSqlServer(connectionString, o => o.CommandTimeout(30))
                .Options;

            // ===== Load meters from DB =====
            var meters = await LoadMetersFromDb(dbOptions, configuration);
            dbSw.Stop();

            if (meters.Count == 0)
            {
                Console.WriteLine("  ERREUR: Aucun compteur avec equipement IP trouve en BDD.");
                return 1;
            }

            // Assign indices and display
            var grouped = meters.GroupBy(m => $"{m.Ip}:{m.Port}").OrderBy(g => g.Key);
            int index = 1;
            foreach (var group in grouped)
            {
                Console.WriteLine($"  IP: {group.Key}");
                foreach (var meter in group)
                {
                    meter.Index = index;
                    var keyStatus = meter.HasKeys ? "OK" : "MANQUANTES";
                    Console.WriteLine($"    - #{index} Compteur {meter.Serial} (cles: {keyStatus})");
                    index++;
                }
            }
            Console.WriteLine();

            var metersWithKeys = meters.Where(m => m.HasKeys).ToList();
            var distinctIps = meters.Select(m => $"{m.Ip}:{m.Port}").Distinct().Count();

            Console.WriteLine($"  Chargement BDD: {dbSw.ElapsedMilliseconds}ms ({meters.Count} compteurs, {distinctIps} IPs, {metersWithKeys.Count} avec cles valides)");
            Console.WriteLine();

            // ===== Route based on CLI arguments =====
            var mode = args.Length > 0 ? args[0] : "";

            if (mode == "--list")
            {
                Console.WriteLine("Mode liste uniquement. Commandes disponibles :");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- <numero>    Test un seul compteur");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- --seq       Lecture sequentielle");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- --parallel [N]  Lecture parallele (pool de N IPs, defaut 10)");
                return 0;
            }

            if (mode == "--seq")
            {
                var report = await RunSequentialTest(metersWithKeys, dbOptions, dbSw.ElapsedMilliseconds, meters.Count, distinctIps);
                report.TotalElapsedMs = totalSw.ElapsedMilliseconds;
                PrintReport(report);
                return 0;
            }

            if (mode == "--parallel")
            {
                int poolSize = 10; // default
                if (args.Length > 1 && int.TryParse(args[1], out int ps) && ps > 0)
                    poolSize = ps;
                await RunParallelTest(metersWithKeys, dbOptions, dbSw.ElapsedMilliseconds, meters.Count, distinctIps, poolSize);
                // Report is printed inside RunParallelTest via PrintMultiPassReport
                return 0;
            }

            // ===== Single meter mode (existing behavior) =====
            return await RunSingleMeterTest(args, meters, dbOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"  ERREUR FATALE: {ex.Message}");
            Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"  Inner: {ex.InnerException.Message}");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // ===== LoadMetersFromDb =====

    private static async Task<List<MeterInfo>> LoadMetersFromDb(
        DbContextOptions<DLMSDBContext> dbOptions, IConfiguration configuration)
    {
        Console.WriteLine("[1] Connexion BDD...");

        using var context = new DLMSDBContext(dbOptions);
        await context.Database.CanConnectAsync();
        var dbName = configuration["DB"];
        Console.WriteLine($"    OK ({dbName})");
        Console.WriteLine();

        var compteurEquipements = await context.CompteurEquipement
            .Include(ce => ce.Compteur)
            .Include(ce => ce.Equipement)
            .Where(ce => ce.Equipement.AdresseIp != null && ce.Equipement.AdresseIp != "")
            .ToListAsync();

        // Bulk load all read association keys in ONE query (eliminates N+1)
        var allReadKeys = await context.AssociationKeys
            .Where(k => k.Type == "read" && (k.Keyname == "authentication" || k.Keyname == "unicast"))
            .Select(k => new { k.CompteurId, k.Keyname })
            .ToListAsync();

        var meters = new List<MeterInfo>();
        foreach (var ce in compteurEquipements)
        {
            var serial = ce.Compteur.NumeroCompteur;
            var hasAuthKey = allReadKeys.Any(k => k.Keyname == "authentication" && k.CompteurId.Contains(serial));
            var hasUnicastKey = allReadKeys.Any(k => k.Keyname == "unicast" && k.CompteurId.Contains(serial));

            meters.Add(new MeterInfo
            {
                CompteurEquipement = ce,
                Serial = serial,
                Ip = ce.Equipement.AdresseIp!,
                Port = ce.Equipement.Port ?? "4059",
                HasKeys = hasAuthKey && hasUnicastKey
            });
        }

        Console.WriteLine("[2] Compteurs disponibles :");
        return meters;
    }

    // ===== ReadSingleMeter on existing session =====

    private static async Task<MeterResult> ReadSingleMeter(
        IDLMSCommunicationSession session,
        MeterInfo meter,
        DLMSKeyService keyService)
    {
        var result = new MeterResult
        {
            Serial = meter.Serial,
            Ip = meter.Ip,
            Port = meter.Port
        };

        try
        {
            // 1. Retrieve keys
            var keySw = Stopwatch.StartNew();
            var keys = await keyService.GetKeysAsync("read", meter.Serial, "read");
            keySw.Stop();
            result.KeysRetrievalMs = keySw.ElapsedMilliseconds;

            if (keys == null || !keys.IsValid)
            {
                result.Error = "Cles DLMS invalides";
                return result;
            }

            // 2. Initialize meter client on existing transport
            var meterParams = new DLMSConnectionParameters
            {
                AddressIp = meter.Ip,
                Port = meter.Port,
                ClientAddress = "read",
                SerialNumber = meter.Serial,
                InterfaceType = "HDLC",
                Password = keys.Password ?? "",
                AuthenticationKey = keys.AuthenticationKey,
                UnicastKey = keys.UnicastKey,
                Trace = TraceLevel.Off,
                UseGbt = true
            };

            session.InitializeMeterClient(meterParams, waitTime: 3000, retryCount: 1);

            // 3. HDLC association
            var hdlcSw = Stopwatch.StartNew();
            session.Reader!.InitializeConnection();
            hdlcSw.Stop();
            result.HdlcMs = hdlcSw.ElapsedMilliseconds;

            // 4. Read data
            session.ReadObjects.Clear();
            session.ReadObjects.Add(new KeyValuePair<string, int>("0.0.1.0.0.255", 2));   // Clock
            session.ReadObjects.Add(new KeyValuePair<string, int>("0.0.42.0.0.255", 2));  // Logical device name

            var readerLogger = _loggerFactory.CreateLogger<NonStaticReaderCommunication>();
            var reader = new NonStaticReaderCommunication(readerLogger);

            var readSw = Stopwatch.StartNew();
            var readResult = await reader.ReadListAsync(session);
            readSw.Stop();
            result.ReadMs = readSw.ElapsedMilliseconds;

            if (readResult == "Lecture impossible")
            {
                // Retry: disconnect HDLC, re-associate, re-read
                Console.WriteLine($"    [{meter.Ip}] {meter.Serial} retry apres echec lecture");
                try { session.Reader?.Disconnect(); } catch { }

                session.AssociationLoaded = false;
                session.InitializeMeterClient(meterParams, waitTime: 5000, retryCount: 2);

                var retrySw = Stopwatch.StartNew();
                session.Reader!.InitializeConnection();
                var retryResult = await reader.ReadListAsync(session);
                retrySw.Stop();
                result.ReadMs += retrySw.ElapsedMilliseconds;

                if (retryResult == "Lecture impossible")
                {
                    result.Error = "Lecture impossible (apres retry)";
                    return result;
                }

                readResult = retryResult;
            }

            result.Success = true;
            result.ReadData = readResult;
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
        }
        finally
        {
            // Release HDLC association, keep TCP open for next meter
            try { session.Reader?.Disconnect(); } catch { }
        }

        return result;
    }

    // ===== RunSequentialTest =====

    private static async Task<TestReport> RunSequentialTest(
        List<MeterInfo> meters, DbContextOptions<DLMSDBContext> dbOptions,
        long dbLoadMs, int totalMeters, int totalIps)
    {
        var report = new TestReport
        {
            Mode = "Sequentiel",
            DbLoadMs = dbLoadMs,
            TotalMeters = totalMeters,
            TotalIps = totalIps,
            MetersWithKeys = meters.Count
        };

        Console.WriteLine("=== Mode SEQUENTIEL ===");
        Console.WriteLine();

        var contextFactory = new SimpleDbContextFactory(dbOptions);
        var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
        var keyServiceLogger = _loggerFactory.CreateLogger<DLMSKeyService>();
        var keyService = new DLMSKeyService(contextFactory, cache, keyServiceLogger);

        var factoryLogger = _loggerFactory.CreateLogger<DLMSGuruxSessionFactory>();
        var sessionLogger = _loggerFactory.CreateLogger<DLMSGuruxSession>();
        var sessionFactory = new DLMSGuruxSessionFactory(factoryLogger, sessionLogger);

        // Group meters by IP
        var ipGroups = meters.GroupBy(m => $"{m.Ip}:{m.Port}").OrderBy(g => g.Key).ToList();

        // Phase 1: Parallel TCP pre-scan (5s timeout) to identify reachable IPs
        Console.WriteLine($"[Pre-scan] Test TCP parallele de {ipGroups.Count} IPs (timeout 5s)...");
        var scanLock = new object();
        int scannedIps = 0;

        var scanTasks = ipGroups.Select(group =>
        {
            var firstMeter = group.First();
            return Task.Run(async () =>
            {
                var key = $"{firstMeter.Ip}:{firstMeter.Port}";
                try
                {
                    using var client = new System.Net.Sockets.TcpClient();
                    using var scanCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    var sw = Stopwatch.StartNew();
                    await client.ConnectAsync(firstMeter.Ip, int.Parse(firstMeter.Port), scanCts.Token);
                    sw.Stop();
                    var num = Interlocked.Increment(ref scannedIps);
                    lock (scanLock)
                    {
                        Console.WriteLine($"  [{num}/{ipGroups.Count}] {key} : OK ({sw.ElapsedMilliseconds}ms)");
                    }
                    return (Key: key, Reachable: true, Ms: sw.ElapsedMilliseconds, Error: "");
                }
                catch (Exception ex)
                {
                    var num = Interlocked.Increment(ref scannedIps);
                    lock (scanLock)
                    {
                        Console.WriteLine($"  [{num}/{ipGroups.Count}] {key} : ECHEC ({ex.Message})");
                    }
                    return (Key: key, Reachable: false, Ms: 5000L, Error: ex.Message);
                }
            });
        }).ToList();

        var scanResults = await Task.WhenAll(scanTasks);
        var reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));

        Console.WriteLine($"  Resultats: {reachableIps.Count}/{ipGroups.Count} IPs accessibles");
        Console.WriteLine();

        // Phase 2: Sequential reads only on reachable IPs
        Console.WriteLine("[Lectures] Traitement des IPs accessibles...");
        Console.WriteLine();

        foreach (var group in ipGroups)
        {
            var firstMeter = group.First();
            var ipKey = $"{firstMeter.Ip}:{firstMeter.Port}";
            var ipResult = new IpGroupResult
            {
                Ip = firstMeter.Ip,
                Port = firstMeter.Port
            };

            // Skip unreachable IPs (already detected by pre-scan)
            if (!reachableIps.Contains(ipKey))
            {
                ipResult.TcpError = "Echec au pre-scan TCP";
                Console.WriteLine($"--- TCP {ipKey} ... IGNORE (echec pre-scan)");
                report.IpGroups.Add(ipResult);
                continue;
            }

            Console.Write($"--- TCP {firstMeter.Ip}:{firstMeter.Port} ... ");

            // Open TCP connection (5s timeout for batch mode)
            var transportParams = new DLMSConnectionParameters
            {
                AddressIp = firstMeter.Ip,
                Port = firstMeter.Port,
                Trace = TraceLevel.Off
            };

            var session = sessionFactory.CreateSession(transportParams);

            var tcpSw = Stopwatch.StartNew();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var connected = await session.OpenTransportAsync(cts.Token);
            tcpSw.Stop();
            ipResult.TcpMs = tcpSw.ElapsedMilliseconds;

            if (!connected)
            {
                ipResult.TcpError = "Timeout TCP";
                Console.WriteLine($"ECHEC ({tcpSw.ElapsedMilliseconds}ms)");
                report.IpGroups.Add(ipResult);
                continue;
            }

            ipResult.TcpSuccess = true;
            Console.WriteLine($"OK ({tcpSw.ElapsedMilliseconds}ms)");

            try
            {
                // Read each meter on this IP sequentially (180s global timeout per meter)
                foreach (var meter in group)
                {
                    Console.Write($"  Compteur {meter.Serial} ... ");

                    MeterResult meterResult;
                    try
                    {
                        meterResult = await ReadSingleMeter(session, meter, keyService)
                            .WaitAsync(TimeSpan.FromSeconds(180));
                    }
                    catch (TimeoutException)
                    {
                        meterResult = new MeterResult
                        {
                            Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                            Error = "Timeout global (180s)"
                        };
                        try { session.Reader?.Disconnect(); } catch { }
                    }
                    ipResult.MeterResults.Add(meterResult);

                    if (meterResult.Success)
                    {
                        Console.WriteLine($"OK (HDLC:{meterResult.HdlcMs}ms, Lecture:{meterResult.ReadMs}ms)");
                    }
                    else
                    {
                        Console.WriteLine($"ECHEC - {meterResult.Error}");
                    }

                    // 100ms pacing delay between meters
                    await Task.Delay(100);
                }
            }
            finally
            {
                await session.DisconnectAsync();
            }

            report.IpGroups.Add(ipResult);
            Console.WriteLine();
        }

        return report;
    }

    // ===== RunParallelTest (Multi-pass orchestrator) =====

    private static async Task<MultiPassReport> RunParallelTest(
        List<MeterInfo> meters, DbContextOptions<DLMSDBContext> dbOptions,
        long dbLoadMs, int totalMeters, int totalIps, int maxConcurrentIps = 8)
    {
        var multiPassReport = new MultiPassReport { TotalMeters = meters.Count };
        var successfulSerials = new HashSet<string>();
        var totalSw = Stopwatch.StartNew();
        const int GlobalCeilingSeconds = 3600; // 60 minutes hard ceiling

        Console.WriteLine($"=== Mode MULTI-PASS (pool={maxConcurrentIps} IPs, 3 passes, budget 60 min) ===");
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
            new PassConfig(1, budget: 1200, canary: 180, cached: 180, uncached: 300,
                           maxFails: 2, cooldownCount: 0, cooldownSeconds: 0,  pause: 300),
            new PassConfig(2, budget: 900,  canary: 300, cached: 240, uncached: 360,
                           maxFails: 3, cooldownCount: 1, cooldownSeconds: 15, pause: 300),
            new PassConfig(3, budget: 600,  canary: 420, cached: 300, uncached: 420,
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
                    // --- Canary test (rotating: try up to 3 different meters) ---
                    var maxCanaryAttempts = Math.Min(3, meterList.Count);
                    MeterResult? canaryResult = null;
                    int canaryIndex = -1;
                    long canaryLatencyMs = 0;

                    for (int c = 0; c < maxCanaryAttempts; c++)
                    {
                        var canaryMeter = meterList[c];
                        var canaryTimeout = budget.ClampTimeout(passConfig.CanaryTimeoutSeconds);
                        if (canaryTimeout == -1)
                        {
                            foreach (var m in meterList) passResult.DeferMeter(m);
                            passResult.DeferIp(ipKey);
                            return;
                        }

                        lock (consoleLock) { Console.WriteLine($"    [{ipKey}] Canary {canaryMeter.Serial} (timeout:{canaryTimeout}s){(c > 0 ? $" [tentative {c + 1}/{maxCanaryAttempts}]" : "")}"); }

                        MeterResult result;
                        var canarySw = Stopwatch.StartNew();
                        try
                        {
                            result = await ReadSingleMeter(session, canaryMeter, keyService)
                                .WaitAsync(TimeSpan.FromSeconds(canaryTimeout));
                        }
                        catch (TimeoutException)
                        {
                            result = new MeterResult
                            {
                                Serial = canaryMeter.Serial, Ip = canaryMeter.Ip, Port = canaryMeter.Port,
                                Error = $"Canary timeout ({canaryTimeout}s)"
                            };
                            try { session.Reader?.Disconnect(); } catch { }
                        }
                        canarySw.Stop();

                        // Record this canary attempt
                        lock (passResult.Results) { passResult.Results.Add(result); }
                        concentratorStats.Record(ipKey, result.Success,
                            result.HdlcMs + result.ReadMs + result.KeysRetrievalMs);

                        if (result.Success)
                        {
                            canaryResult = result;
                            canaryIndex = c;
                            canaryLatencyMs = canarySw.ElapsedMilliseconds;
                            break;
                        }

                        lock (consoleLock) { Console.WriteLine($"    [{ipKey}] Canary {canaryMeter.Serial} ECHEC - {result.Error}"); }
                    }

                    // All canary attempts failed
                    if (canaryResult == null || !canaryResult.Success)
                    {
                        try { session.Reader?.Disconnect(); } catch { }
                        var remainingStart = maxCanaryAttempts;
                        var remainingCount = meterList.Count - remainingStart;

                        if (passConfig.PassNumber < 3)
                        {
                            lock (consoleLock) { Console.WriteLine($"    [{ipKey}] Canary ECHEC ({maxCanaryAttempts} tentatives) — {remainingCount} compteurs differes"); }
                            for (int i = remainingStart; i < meterList.Count; i++)
                                passResult.DeferMeter(meterList[i]);
                            passResult.DeferIp(ipKey);
                        }
                        else
                        {
                            lock (consoleLock) { Console.WriteLine($"    [{ipKey}] Canary ECHEC Pass 3 ({maxCanaryAttempts} tentatives) — {remainingCount} compteurs en abandon definitif"); }
                            for (int i = remainingStart; i < meterList.Count; i++)
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

                    // Canary succeeded — skip meters already tried as canary (index 0..canaryIndex)
                    var readStartIndex = canaryIndex + 1;
                    var metersToReadCount = meterList.Count - readStartIndex;
                    lock (consoleLock)
                    {
                        Console.WriteLine($"    [{ipKey}] Canary OK ({canaryLatencyMs}ms) — lecture de {metersToReadCount} compteurs restants");
                    }

                    // --- Read remaining meters (skip canary candidates 0..canaryIndex) ---
                    int consecutiveFailures = 0;
                    int cooldownsUsed = 0;

                    for (int i = readStartIndex; i < meterList.Count; i++)
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

    // ===== PrintReport =====

    private static void PrintReport(TestReport report)
    {
        Console.WriteLine();
        Console.WriteLine("=== RAPPORT DE TEST D'INTEGRATION DLMS ===");
        Console.WriteLine();
        Console.WriteLine($"  Mode: {report.Mode}");
        Console.WriteLine($"  Chargement BDD: {report.DbLoadMs}ms ({report.TotalMeters} compteurs, {report.TotalIps} IPs, {report.MetersWithKeys} avec cles valides)");
        Console.WriteLine();
        Console.WriteLine("=== STATISTIQUES ===");
        Console.WriteLine($"  Total compteurs testes : {report.MetersTested}/{report.MetersWithKeys}");

        if (report.MetersTested > 0)
        {
            var successPct = (double)report.MetersSucceeded / report.MetersTested * 100;
            var failPct = (double)report.MetersFailed / report.MetersTested * 100;

            Console.WriteLine($"  Reussis                : {report.MetersSucceeded} ({successPct:F1}%)");
            Console.WriteLine($"  Echecs                 : {report.MetersFailed} ({failPct:F1}%)");
        }
        else
        {
            Console.WriteLine($"  Reussis                : 0");
            Console.WriteLine($"  Echecs                 : 0");
        }

        Console.WriteLine($"  Temps total            : {report.TotalElapsedMs / 1000.0:F1}s");

        if (report.MetersSucceeded > 0)
        {
            Console.WriteLine($"  Temps moyen par compteur (OK) : {report.AvgTotalPerMeterMs:F0}ms");
            Console.WriteLine($"  Temps moyen HDLC       : {report.AvgHdlcMs:F0}ms");
            Console.WriteLine($"  Temps moyen lecture     : {report.AvgReadMs:F0}ms");
        }

        Console.WriteLine($"  TCP reussis            : {report.TcpSucceeded}/{report.IpGroups.Count} IPs");
        Console.WriteLine();

        // Detailed per-IP breakdown
        Console.WriteLine("=== DETAIL PAR IP ===");
        foreach (var ipResult in report.IpGroups)
        {
            var tcpStatus = ipResult.TcpSuccess
                ? $"TCP: {ipResult.TcpMs}ms"
                : $"TCP: ECHEC - {ipResult.TcpError}";

            Console.WriteLine($"--- {ipResult.Ip}:{ipResult.Port} ({tcpStatus}) ---");

            if (!ipResult.TcpSuccess)
            {
                Console.WriteLine("  (tous les compteurs ignores)");
            }
            else
            {
                foreach (var mr in ipResult.MeterResults)
                {
                    if (mr.Success)
                    {
                        Console.WriteLine($"  {mr.Serial}  | Cles: {mr.KeysRetrievalMs}ms | HDLC: {mr.HdlcMs}ms | Lecture: {mr.ReadMs}ms | OK");
                    }
                    else
                    {
                        Console.WriteLine($"  {mr.Serial}  | Cles: {mr.KeysRetrievalMs}ms | HDLC: {(mr.HdlcMs > 0 ? $"{mr.HdlcMs}ms" : "-")} | Err: {mr.Error}");
                    }
                }
            }
        }
        Console.WriteLine();

        // Statistiques par concentrateur (si disponibles)
        if (report.Stats != null)
        {
            var allStats = report.Stats.GetAllStats();
            if (allStats.Count > 0)
            {
                Console.WriteLine("=== STATISTIQUES PAR CONCENTRATEUR ===");
                Console.WriteLine($"  {"IP",-25} {"Total",6} {"OK",6} {"Echec",6} {"Taux",6} {"Lat Moy",10}");
                Console.WriteLine($"  {new string('-', 59)}");

                foreach (var kv in allStats.OrderBy(k => k.Key))
                {
                    var s = kv.Value;
                    Console.WriteLine($"  {kv.Key,-25} {s.TotalAttempts,6} {s.Successes,6} {s.Failures,6} {s.SuccessRate,5:F0}% {s.AvgLatencyMs,8:F0}ms");
                }

                var (globalRate, globalAvgLat, globalTotal) = report.Stats.GetGlobalStats();
                var globalSuccesses = allStats.Values.Sum(s => s.Successes);
                var globalFailures = allStats.Values.Sum(s => s.Failures);
                Console.WriteLine($"  {new string('-', 59)}");
                Console.WriteLine($"  {"GLOBAL",-25} {globalTotal,6} {globalSuccesses,6} {globalFailures,6} {globalRate,5:F0}% {globalAvgLat,8:F0}ms");
                Console.WriteLine();
            }
        }
    }

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

        // Per-IP concentrator stats (deduplicated by serial — keep last result per meter)
        Console.WriteLine("=== STATISTIQUES PAR CONCENTRATEUR ===");
        var dedupResults = report.AllResults
            .GroupBy(r => r.Serial)
            .Select(g => g.Last())
            .ToList();
        var ipStats = dedupResults
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

    // ===== RunSingleMeterTest (original behavior) =====

    private static async Task<int> RunSingleMeterTest(
        string[] args, List<MeterInfo> meters, DbContextOptions<DLMSDBContext> dbOptions)
    {
        var totalSw = Stopwatch.StartNew();

        // Select meter
        MeterInfo? selected = null;
        if (args.Length > 0 && int.TryParse(args[0], out int argIndex))
        {
            selected = meters.FirstOrDefault(m => m.Index == argIndex);
        }

        if (selected == null)
        {
            Console.Write($"Selectionnez un compteur (1-{meters.Count}) : ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                selected = meters.FirstOrDefault(m => m.Index == choice);
            }
        }

        if (selected == null)
        {
            Console.WriteLine("  ERREUR: Selection invalide.");
            return 1;
        }

        if (!selected.HasKeys)
        {
            Console.WriteLine($"  ERREUR: Cles DLMS manquantes pour le compteur {selected.Serial}.");
            return 1;
        }

        Console.WriteLine($"  -> Compteur {selected.Serial} sur {selected.Ip}:{selected.Port}");
        Console.WriteLine();

        // Retrieve DLMS keys via DLMSKeyService
        var contextFactory = new SimpleDbContextFactory(dbOptions);
        var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
        var keyServiceLogger = _loggerFactory.CreateLogger<DLMSKeyService>();
        var keyService = new DLMSKeyService(contextFactory, cache, keyServiceLogger);

        var keys = await keyService.GetKeysAsync("read", selected.Serial, "read");
        if (keys == null || !keys.IsValid)
        {
            Console.WriteLine("  ERREUR: Impossible de recuperer les cles DLMS valides.");
            return 1;
        }

        Console.WriteLine($"    Cles recuperees: Auth={keys.AuthenticationKey[..8]}..., Unicast={keys.UnicastKey[..8]}..., Pwd={(!string.IsNullOrEmpty(keys.Password) ? "OK" : "vide")}");
        Console.WriteLine();

        // ===== ETAPE 3 : Test connectivite TCP =====
        Console.Write($"[3] Test TCP vers {selected.Ip}:{selected.Port}...");

        var transportParams = new DLMSConnectionParameters
        {
            AddressIp = selected.Ip,
            Port = selected.Port,
            Trace = TraceLevel.Info
        };

        var factoryLogger = _loggerFactory.CreateLogger<DLMSGuruxSessionFactory>();
        var sessionLogger = _loggerFactory.CreateLogger<DLMSGuruxSession>();
        var sessionFactory = new DLMSGuruxSessionFactory(factoryLogger, sessionLogger);

        var session = sessionFactory.CreateSession(transportParams);

        var tcpSw = Stopwatch.StartNew();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var connected = await session.OpenTransportAsync(cts.Token);
        tcpSw.Stop();

        if (!connected)
        {
            Console.WriteLine($" ECHEC ({tcpSw.ElapsedMilliseconds}ms)");
            Console.WriteLine();
            Console.WriteLine("  DIAGNOSTIC:");
            Console.WriteLine("    - Verifiez que FortiClient VPN est connecte");
            Console.WriteLine($"    - Testez: ping {selected.Ip}");
            Console.WriteLine($"    - Testez: telnet {selected.Ip} {selected.Port}");
            Console.WriteLine("    - Le concentrateur (UMAD) est peut-etre hors ligne");
            return 1;
        }

        Console.WriteLine($" OK ({tcpSw.ElapsedMilliseconds}ms)");
        Console.WriteLine();

        // ===== ETAPE 4 : Association HDLC & authentification =====
        Console.Write($"[4] Association HDLC avec compteur {selected.Serial}...");

        var meterParams = new DLMSConnectionParameters
        {
            AddressIp = selected.Ip,
            Port = selected.Port,
            ClientAddress = "read",
            SerialNumber = selected.Serial,
            InterfaceType = "HDLC",
            Password = keys.Password ?? "",
            AuthenticationKey = keys.AuthenticationKey,
            UnicastKey = keys.UnicastKey,
            Trace = TraceLevel.Info,
            UseGbt = true
        };

        session.InitializeMeterClient(meterParams, waitTime: 5000, retryCount: 2);

        Console.WriteLine();
        Console.WriteLine($"    Client: read (2), Server: calcule depuis {selected.Serial}, Auth: HighGMAC");

        var hdlcSw = Stopwatch.StartNew();
        try
        {
            session.Reader!.InitializeConnection();
            hdlcSw.Stop();
            Console.WriteLine($"    ... OK ({hdlcSw.ElapsedMilliseconds}ms)");
        }
        catch (Exception ex)
        {
            hdlcSw.Stop();
            Console.WriteLine($"    ... ECHEC ({hdlcSw.ElapsedMilliseconds}ms)");
            Console.WriteLine($"    Erreur: {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("  DIAGNOSTIC:");
            Console.WriteLine("    - Verifiez les cles d'authentification (authentication + unicast)");
            Console.WriteLine("    - Verifiez que le type de client est 'read'");
            Console.WriteLine("    - Le compteur est peut-etre hors ligne derriere le concentrateur");
            await session.DisconnectAsync();
            return 1;
        }
        Console.WriteLine();

        // ===== ETAPE 5 : Lecture de donnees =====
        Console.WriteLine("[5] Lecture des donnees :");

        // Configure OBIS codes to read
        session.ReadObjects.Clear();
        session.ReadObjects.Add(new KeyValuePair<string, int>("0.0.1.0.0.255", 2));
        session.ReadObjects.Add(new KeyValuePair<string, int>("0.0.42.0.0.255", 2));

        var readerLogger = _loggerFactory.CreateLogger<NonStaticReaderCommunication>();
        var reader = new NonStaticReaderCommunication(readerLogger);

        var readSw = Stopwatch.StartNew();
        var result = await reader.ReadAsync(session);
        readSw.Stop();

        if (result == "Lecture impossible")
        {
            Console.WriteLine("    ECHEC: Lecture impossible");
            Console.WriteLine();
            Console.WriteLine("  DIAGNOSTIC:");
            Console.WriteLine("    - L'association a pu reussir mais les OBIS codes ne sont pas disponibles");
            Console.WriteLine("    - Essayez d'autres OBIS codes (ex: 0.0.96.1.0.255:2 pour le numero de serie)");
            Console.WriteLine("    - Le timeout de lecture est peut-etre trop court");
        }
        else
        {
            Console.WriteLine($"    Resultat ({readSw.ElapsedMilliseconds}ms) :");
            try
            {
                var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (parsed != null)
                {
                    foreach (var kvp in parsed)
                    {
                        var label = kvp.Key switch
                        {
                            "0.0.1.0.0.255" => "Horloge",
                            "0.0.42.0.0.255" => "Nom logique",
                            _ => kvp.Key
                        };
                        Console.WriteLine($"    {label} ({kvp.Key}): {kvp.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"    {result}");
                }
            }
            catch
            {
                Console.WriteLine($"    {result}");
            }
        }

        Console.WriteLine();

        // Cleanup
        await session.DisconnectAsync();

        totalSw.Stop();
        Console.WriteLine($"=== TEST REUSSI en {totalSw.Elapsed.TotalSeconds:F1}s ===");
        return 0;
    }

    // ===== Multi-pass helpers =====

    private static bool HasCache(MeterInfo m) =>
        File.Exists(Path.Combine("associations", $"{m.Serial}_Read.xml"));

    private static int ComputeAdaptiveTimeout(long canaryLatencyMs, bool hasCacheFile, PassConfig config)
    {
        var raw = (int)(canaryLatencyMs / 1000.0 * 2.5);
        var ceiling = hasCacheFile ? config.CachedTimeoutSeconds : config.UncachedTimeoutSeconds;
        return Math.Clamp(raw, 90, ceiling);
    }

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

    // ===== GetConnectionString (unchanged) =====

    private static string GetConnectionString(IConfiguration configuration)
    {
        var serveur = configuration["Serveur"];
        var db = configuration["DB"];
        var env = configuration["Env"];

        Console.WriteLine($"    Config: Env={env}, Serveur={serveur}, DB={db}");

        if (string.IsNullOrEmpty(serveur) || string.IsNullOrEmpty(db))
            throw new InvalidOperationException("Configuration BDD incomplete: Serveur ou DB manquant dans appsettings.json");

        var user = configuration["DbUser"];
        var pass = configuration["DbPass"];

        if (string.IsNullOrEmpty(user))
        {
            try
            {
                var ascuser = new asc_connection.connection();
                user = ascuser.asc_user;
                pass = ascuser.asc_pass;
                Console.WriteLine($"    Source credentials: asc_connection.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    asc_connection.dll erreur: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"    Source credentials: appsettings.json");
        }

        Console.WriteLine($"    Credentials: user='{user}', pass='{(string.IsNullOrEmpty(pass) ? "(vide)" : pass[..Math.Min(3, pass.Length)] + "***")}'");

        if (string.IsNullOrEmpty(user))
        {
            Console.WriteLine("    ATTENTION: Aucun credential DB. Remplissez DbUser/DbPass dans appsettings.json");
        }

        var connStr = $"Server={serveur};Database={db};Trusted_Connection=false;TrustServerCertificate=true;MultipleActiveResultSets=true;user id={user};password={pass};";
        Console.WriteLine($"    ConnectionString: Server={serveur};Database={db};user id={user}");

        return connStr;
    }
}

class SimpleDbContextFactory : IDbContextFactory<DLMSDBContext>
{
    private readonly DbContextOptions<DLMSDBContext> _options;

    public SimpleDbContextFactory(DbContextOptions<DLMSDBContext> options)
    {
        _options = options;
    }

    public DLMSDBContext CreateDbContext()
    {
        return new DLMSDBContext(_options);
    }
}
