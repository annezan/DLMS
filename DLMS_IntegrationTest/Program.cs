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
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
using Task = System.Threading.Tasks.Task;

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

record CliOptions(string Mode, int PoolSize, string? FilterIp, string? FilterMeter);

class ProfileReadResult
{
    public string ProfileObis { get; set; } = "";
    public string ProfileName { get; set; } = "";
    public bool Success { get; set; }
    public int RowCount { get; set; }
    public long DurationMs { get; set; }
    public string Error { get; set; } = "";
    public string? RawData { get; set; }
    public string[]? Columns { get; set; }
    public List<object[]>? RowsRaw { get; set; }
    public List<object[]>? RowsConverted { get; set; }
    public List<object[]>? RowsWithTcTt { get; set; }
}

class ScalerInfo
{
    public double Scaler { get; set; }
    public string Unit { get; set; } = "";
    public int UnitCode { get; set; }
}

class MeterProfileReport
{
    public string Serial { get; set; } = "";
    public string Ip { get; set; } = "";
    public bool ConnectionSuccess { get; set; }
    public long HdlcMs { get; set; }
    public List<ProfileReadResult> Profiles { get; set; } = new();
    public string Error { get; set; } = "";
    public Dictionary<string, object?> TcTtValues { get; set; } = new();
    public Dictionary<string, ScalerInfo> Scalers { get; set; } = new();
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

    public const int TcpScanTimeoutSeconds = 15;

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

    static CliOptions? ParseArgs(string[] args)
    {
        string? mode = null;
        int poolSize = 10;
        string? filterIp = null;
        string? filterMeter = null;

        int i = 0;
        while (i < args.Length)
        {
            switch (args[i])
            {
                case "--list":
                case "--seq":
                case "--profiles":
                    mode = args[i];
                    break;
                case "--parallel":
                    mode = "--parallel";
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int ps) && ps > 0)
                    {
                        poolSize = ps;
                        i++;
                    }
                    break;
                case "--ip":
                    if (i + 1 >= args.Length || args[i + 1].StartsWith("--"))
                    {
                        Console.WriteLine("  ERREUR: --ip necessite une adresse IP.");
                        Console.WriteLine("  Usage: --ip <ADRESSE_IP>");
                        return null;
                    }
                    filterIp = args[++i];
                    break;
                case "--meter":
                    if (i + 1 >= args.Length || args[i + 1].StartsWith("--"))
                    {
                        Console.WriteLine("  ERREUR: --meter necessite un numero de serie.");
                        Console.WriteLine("  Usage: --meter <NUMERO_SERIE>");
                        return null;
                    }
                    filterMeter = args[++i];
                    break;
                default:
                    // Not a known flag — leave as-is for single meter mode
                    break;
            }
            i++;
        }

        if (filterIp != null && filterMeter != null)
        {
            Console.WriteLine("  ERREUR: --ip et --meter sont mutuellement exclusifs.");
            Console.WriteLine("  Utilisez l'un ou l'autre, pas les deux.");
            return null;
        }

        // Default mode when only a filter is provided
        if (mode == null)
        {
            if (filterIp != null)
                mode = "--parallel";
            else
                mode = "";
        }

        return new CliOptions(mode, poolSize, filterIp, filterMeter);
    }

    static async Task<int> Main(string[] args)
    {
        var totalSw = Stopwatch.StartNew();

        // Prevent thread pool starvation from synchronous Gurux HDLC calls
        ThreadPool.SetMinThreads(50, 50);

        // S'assurer que le dossier cache d'association existe
        Directory.CreateDirectory("associations");

        Console.WriteLine("=== Test d'integration DLMS/COSEM ===");
        Console.WriteLine();

        // Créer le dossier cache pour les association views
        Directory.CreateDirectory("associations");

        // Parse CLI arguments
        var options = ParseArgs(args);
        if (options == null) return 1;

        // Setup logging — reduce to Warning for batch modes, keep Debug for single meter
        var isBatchMode = options.Mode == "--seq" || options.Mode == "--parallel";
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

            // Apply CLI filters
            if (options.FilterIp != null)
            {
                meters = meters.Where(m => m.Ip == options.FilterIp).ToList();
                Console.WriteLine($"  FILTRE: IP = {options.FilterIp} ({meters.Count} compteur(s))");
            }
            else if (options.FilterMeter != null)
            {
                meters = meters.Where(m => m.Serial == options.FilterMeter).ToList();
                Console.WriteLine($"  FILTRE: Compteur = {options.FilterMeter} ({meters.Count} compteur(s))");
            }

            if (meters.Count == 0)
            {
                Console.WriteLine("  ERREUR: Aucun compteur ne correspond au filtre.");
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

            if (options.Mode == "--list")
            {
                Console.WriteLine("Mode liste uniquement. Commandes disponibles :");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- <numero>    Test un seul compteur");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- --seq       Lecture sequentielle");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- --parallel [N]  Lecture parallele (pool de N IPs, defaut 10)");
                Console.WriteLine("  dotnet run --project DLMS_IntegrationTest/ -- --profiles  Lecture des profils (log detaille par compteur)");
                Console.WriteLine();
                Console.WriteLine("  Filtres (combinables avec --seq ou --parallel) :");
                Console.WriteLine("    --ip <ADRESSE_IP>       Tester uniquement les compteurs d'un concentrateur");
                Console.WriteLine("    --meter <NUMERO_SERIE>  Tester un seul compteur par numero de serie");
                Console.WriteLine();
                Console.WriteLine("  Exemples :");
                Console.WriteLine("    DLMS_IntegrationTest.exe --parallel --ip 10.60.8.185");
                Console.WriteLine("    DLMS_IntegrationTest.exe --parallel 10 --ip 10.60.8.185");
                Console.WriteLine("    DLMS_IntegrationTest.exe --seq --meter 58014077");
                Console.WriteLine("    DLMS_IntegrationTest.exe --ip 10.60.8.185");
                Console.WriteLine("    DLMS_IntegrationTest.exe --meter 58014077");
                return 0;
            }

            if (options.Mode == "--profiles")
            {
                await RunProfilesTest(metersWithKeys, dbOptions, configuration);
                return 0;
            }

            if (options.Mode == "--seq")
            {
                var report = await RunSequentialTest(metersWithKeys, dbOptions, dbSw.ElapsedMilliseconds, meters.Count, distinctIps);
                report.TotalElapsedMs = totalSw.ElapsedMilliseconds;
                PrintReport(report);
                return 0;
            }

            if (options.Mode == "--parallel")
            {
                await RunParallelTest(metersWithKeys, dbOptions, dbSw.ElapsedMilliseconds, meters.Count, distinctIps, options.PoolSize);
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
        DLMSKeyService keyService,
        bool noRetry = false)
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
                if (noRetry)
                {
                    result.Error = "Lecture impossible";
                    return result;
                }

                // Retry: disconnect HDLC, re-associate, re-read
                Console.Write($"retry... ");
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

    // ===== RunSequentialTest (mode isolé: TCP frais par compteur) =====

    private static async Task<TestReport> RunSequentialTest(
        List<MeterInfo> meters, DbContextOptions<DLMSDBContext> dbOptions,
        long dbLoadMs, int totalMeters, int totalIps)
    {
        var report = new TestReport
        {
            Mode = "Sequentiel isole (TCP frais par compteur)",
            DbLoadMs = dbLoadMs,
            TotalMeters = totalMeters,
            TotalIps = totalIps,
            MetersWithKeys = meters.Count
        };

        Console.WriteLine("=== Mode SEQUENTIEL ISOLE (TCP frais par compteur) ===");
        Console.WriteLine();

        var contextFactory = new SimpleDbContextFactory(dbOptions);
        var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
        var keyServiceLogger = _loggerFactory.CreateLogger<DLMSKeyService>();
        var keyService = new DLMSKeyService(contextFactory, cache, keyServiceLogger);

        var factoryLogger = _loggerFactory.CreateLogger<DLMSGuruxSessionFactory>();
        var sessionLogger = _loggerFactory.CreateLogger<DLMSGuruxSession>();
        var sessionFactory = new DLMSGuruxSessionFactory(factoryLogger, sessionLogger);

        // Group meters by IP for TCP pre-scan
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

        // Phase 2: Read each meter with its own fresh TCP connection (isolated)
        Console.WriteLine("[Lectures] Mode isole — TCP frais par compteur...");
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

            // Skip unreachable IPs
            if (!reachableIps.Contains(ipKey))
            {
                ipResult.TcpError = "Echec au pre-scan TCP";
                Console.WriteLine($"--- {ipKey} IGNORE (echec pre-scan)");
                report.IpGroups.Add(ipResult);
                continue;
            }

            Console.WriteLine($"--- {ipKey} ({group.Count()} compteurs)");

            foreach (var meter in group)
            {
                Console.Write($"  {meter.Serial} ... ");

                // Fresh TCP connection per meter
                var transportParams = new DLMSConnectionParameters
                {
                    AddressIp = meter.Ip,
                    Port = meter.Port,
                    Trace = TraceLevel.Off
                };

                var session = sessionFactory.CreateSession(transportParams);
                MeterResult meterResult;

                try
                {
                    using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
                    var connected = await session.OpenTransportAsync(tcpCts.Token);

                    if (!connected)
                    {
                        meterResult = new MeterResult
                        {
                            Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                            Error = "TCP echec"
                        };
                    }
                    else
                    {
                        try
                        {
                            meterResult = await ReadSingleMeter(session, meter, keyService, noRetry: false)
                                .WaitAsync(TimeSpan.FromSeconds(120));
                        }
                        catch (TimeoutException)
                        {
                            meterResult = new MeterResult
                            {
                                Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                                Error = "Timeout (120s)"
                            };
                        }
                        finally
                        {
                            try { session.Reader?.Disconnect(); } catch { }
                            try { await session.DisconnectAsync(); } catch { }
                        }
                    }
                }
                catch (Exception ex)
                {
                    meterResult = new MeterResult
                    {
                        Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                        Error = ex.Message
                    };
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

                await Task.Delay(200); // pacing
            }

            Console.WriteLine($"  => {ipKey} : {ipResult.MeterResults.Count(r => r.Success)}/{group.Count()} OK");
            Console.WriteLine();

            report.IpGroups.Add(ipResult);
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
            new PassConfig(1, budget: 1200, canary: 60,  cached: 180, uncached: 300,
                           maxFails: 2, cooldownCount: 0, cooldownSeconds: 0,  pause: 300),
            new PassConfig(2, budget: 900,  canary: 90,  cached: 240, uncached: 360,
                           maxFails: 3, cooldownCount: 1, cooldownSeconds: 15, pause: 300),
            new PassConfig(3, budget: 600,  canary: 90,  cached: 300, uncached: 420,
                           maxFails: 5, cooldownCount: 1, cooldownSeconds: 30, pause: 0),
        };

        // Architecture V4: Pass 1 parallele + Pass RESCUE isolee (TCP frais par compteur)
        var metersToRead = meters.ToList();
        const int HardCeilingSeconds = 55 * 60;
        var pass1Config = passConfigs[0];

        // === PASS 1: Parallele ===
        if (metersToRead.Count > 0)
        {
            var globalTimeLeft = Math.Min(
                GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds,
                HardCeilingSeconds - totalSw.Elapsed.TotalSeconds);

            if (globalTimeLeft >= 60)
            {
                var effectiveBudget = (int)Math.Min(pass1Config.BudgetSeconds, globalTimeLeft);
                var passResult = await RunSinglePass(
                    metersToRead, pass1Config, effectiveBudget,
                    sessionFactory, keyService, maxConcurrentIps,
                    concentratorStats, null);

                multiPassReport.Passes.Add(passResult);
                PrintPassSummary(passResult);

                successfulSerials.UnionWith(
                    passResult.Results.Where(r => r.Success).Select(r => r.Serial));
                metersToRead = metersToRead
                    .Where(m => !successfulSerials.Contains(m.Serial)).ToList();

                // Pause adaptative
                if (metersToRead.Count > 0)
                {
                    var uniqueIps = metersToRead.Select(m => m.Ip).Distinct().Count();
                    int pauseSeconds = uniqueIps <= 3 ? 0 : uniqueIps <= 10 ? 120 : pass1Config.PauseAfterSeconds;
                    if (pauseSeconds > 0)
                    {
                        Console.WriteLine($"  Pause {pauseSeconds}s avant Rescue ({metersToRead.Count} compteurs, {uniqueIps} IPs)...");
                        Console.WriteLine();
                        await Task.Delay(TimeSpan.FromSeconds(pauseSeconds));
                        multiPassReport.TotalPauseMs += pauseSeconds * 1000;
                    }
                }
            }
        }

        // === PASS RESCUE: Isolee (TCP frais par compteur) ===
        if (metersToRead.Count > 0)
        {
            var globalTimeLeft = Math.Min(
                GlobalCeilingSeconds - totalSw.Elapsed.TotalSeconds,
                HardCeilingSeconds - totalSw.Elapsed.TotalSeconds);

            if (globalTimeLeft >= 60)
            {
                var rescueBudget = (int)globalTimeLeft;
                var rescueResult = await RunIsolatedRescuePass(
                    metersToRead, rescueBudget,
                    sessionFactory, keyService, multiPassReport);

                multiPassReport.Passes.Add(rescueResult);
                PrintPassSummary(rescueResult);

                successfulSerials.UnionWith(
                    rescueResult.Results.Where(r => r.Success).Select(r => r.Serial));
                metersToRead = metersToRead
                    .Where(m => !successfulSerials.Contains(m.Serial)).ToList();
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


    // ===== Multi-pass: isolated rescue pass (fresh TCP per meter) =====

    private static async Task<PassResult> RunIsolatedRescuePass(
        List<MeterInfo> metersToRead,
        int effectiveBudget,
        DLMSGuruxSessionFactory sessionFactory,
        DLMSKeyService keyService,
        MultiPassReport previousReport)
    {
        var passResult = new PassResult { PassNumber = 2 };
        var budget = new PassBudget(effectiveBudget);
        const int CachedTimeoutSeconds = 30;
        const int UncachedTimeoutSeconds = 120;

        Console.WriteLine($"=== PASS RESCUE ISOLEE ({effectiveBudget}s budget, {metersToRead.Count} compteurs, TCP frais par compteur, cache-building) ===");
        Console.WriteLine();

        // TCP scan to identify reachable IPs
        var ipGroups = metersToRead.GroupBy(m => $"{m.Ip}:{m.Port}").ToList();
        var allIps = ipGroups.Select(g => (g.First().Ip, g.First().Port)).Distinct().ToList();

        Console.WriteLine($"[Rescue] Test TCP de {allIps.Count} IPs (timeout {PassConfig.TcpScanTimeoutSeconds}s)...");
        var scanResults = await ParallelTcpScan(allIps, TimeSpan.FromSeconds(PassConfig.TcpScanTimeoutSeconds));
        var reachableIps = new HashSet<string>(scanResults.Where(r => r.Reachable).Select(r => r.Key));
        Console.WriteLine($"  Resultats: {reachableIps.Count}/{allIps.Count} IPs accessibles");

        // Compute IP success rates for sorting
        var ipSuccessRates = previousReport.AllResults
            .GroupBy(r => $"{r.Ip}:{r.Port}")
            .Where(g => !string.IsNullOrEmpty(g.Key) && g.Key != ":")
            .ToDictionary(g => g.Key,
                g => g.Count() > 0 ? (double)g.Count(r => r.Success) / g.Count() * 100 : 0);

        // Sort: cached meters first (quick), then uncached by IP success rate, dead IPs last
        var sortedMeters = metersToRead
            .Select(m => new
            {
                Meter = m,
                HasCache = File.Exists(Path.Combine("associations", $"{m.Serial}_Read.xml")),
                IpReachable = reachableIps.Contains($"{m.Ip}:{m.Port}"),
                IpRate = ipSuccessRates.TryGetValue($"{m.Ip}:{m.Port}", out var r) ? r : 0
            })
            .OrderByDescending(m => m.IpReachable ? 1 : 0)
            .ThenByDescending(m => m.HasCache ? 1 : 0)
            .ThenByDescending(m => m.IpRate)
            .ToList();

        var cachedCount = sortedMeters.Count(m => m.HasCache && m.IpReachable);
        var uncachedCount = sortedMeters.Count(m => !m.HasCache && m.IpReachable);
        Console.WriteLine($"  {cachedCount} avec cache (timeout {CachedTimeoutSeconds}s), {uncachedCount} sans cache (timeout {UncachedTimeoutSeconds}s, cache-building)");
        Console.WriteLine();

        int okCount = 0, failCount = 0, skipCount = 0, cacheBuilt = 0;

        // Read each meter with FULLY ISOLATED session
        foreach (var entry in sortedMeters)
        {
            var meter = entry.Meter;

            if (budget.IsExpired)
            {
                passResult.Results.Add(new MeterResult
                {
                    Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                    Error = "Budget expire (rescue)"
                });
                skipCount++;
                continue;
            }

            if (!entry.IpReachable)
            {
                passResult.Results.Add(new MeterResult
                {
                    Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                    Error = "IP inaccessible (rescue)"
                });
                skipCount++;
                continue;
            }

            var timeout = entry.HasCache ? CachedTimeoutSeconds : UncachedTimeoutSeconds;
            Console.Write($"    [Rescue] {meter.Serial} ({meter.Ip}, {(entry.HasCache ? "cache" : "no-cache")}, {timeout}s) ... ");

            // Fresh TCP connection per meter
            var transportParams = new DLMSConnectionParameters
            {
                AddressIp = meter.Ip, Port = meter.Port, Trace = TraceLevel.Off
            };
            var session = sessionFactory.CreateSession(transportParams);

            MeterResult meterResult;
            try
            {
                using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
                var tcpOk = await session.OpenTransportAsync(tcpCts.Token);
                if (!tcpOk)
                {
                    meterResult = new MeterResult
                    {
                        Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                        Error = "TCP echec (rescue isolee)"
                    };
                }
                else
                {
                    try
                    {
                        // Cached: noRetry (lecture rapide). Uncached: allow retry (cache-building)
                        meterResult = await ReadSingleMeter(session, meter, keyService, noRetry: entry.HasCache)
                            .WaitAsync(TimeSpan.FromSeconds(timeout));
                    }
                    catch (TimeoutException)
                    {
                        meterResult = new MeterResult
                        {
                            Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                            Error = $"Timeout ({timeout}s)"
                        };
                    }
                    finally
                    {
                        try { session.Reader?.Disconnect(); } catch { }
                        try { await session.DisconnectAsync(); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                meterResult = new MeterResult
                {
                    Serial = meter.Serial, Ip = meter.Ip, Port = meter.Port,
                    Error = ex.Message
                };
            }

            passResult.Results.Add(meterResult);

            if (meterResult.Success)
            {
                okCount++;
                var cacheFile = Path.Combine("associations", $"{meter.Serial}_Read.xml");
                if (!entry.HasCache && File.Exists(cacheFile))
                {
                    cacheBuilt++;
                    Console.WriteLine($"OK + CACHE CONSTITUE (HDLC:{meterResult.HdlcMs}ms, Lecture:{meterResult.ReadMs}ms)");
                }
                else
                {
                    Console.WriteLine($"OK (HDLC:{meterResult.HdlcMs}ms, Lecture:{meterResult.ReadMs}ms){(entry.HasCache ? " [cache]" : "")}");
                }
            }
            else
            {
                failCount++;
                Console.WriteLine($"ECHEC - {meterResult.Error}");
            }

            await Task.Delay(200); // pacing
        }

        Console.WriteLine();
        Console.WriteLine($"  [Rescue] Resultat: {okCount} lus, {failCount} echoues, {skipCount} non traites, {cacheBuilt} caches constitues");

        passResult.ElapsedMs = budget.ElapsedMs;
        return passResult;
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

    // ===== RunProfilesTest — lecture des profils avec log détaillé =====

    private static readonly (string Obis, string Name, int TimeoutSeconds)[] ProfileDefinitions = new[]
    {
        ("1.0.99.1.0.255", "Load Profile 1 (horaire)", 60),
        ("1.0.99.2.0.255", "Load Profile 2 (5 min)", 120),
        ("1.0.99.3.0.255", "Load Profile 3 (journalier)", 90),
        ("0.0.98.1.0.255", "Billing Profile", 20),
        ("0.0.99.98.0.255", "Event Log 0", 20),
        ("0.0.99.98.1.255", "Event Log 1", 20),
        ("0.0.99.98.2.255", "Event Log 2", 20),
        ("0.0.99.98.3.255", "Event Log 3", 20),
    };

    private static readonly string[] TcTtObis = new[]
    {
        "1.0.0.4.2.255",  // CT ratio numerator
        "1.0.0.4.3.255",  // CT ratio denominator / VT
        "1.0.0.4.5.255",  // VT ratio numerator
        "1.0.0.4.6.255",  // VT ratio denominator
    };

    private static async Task RunProfilesTest(
        List<MeterInfo> meters, DbContextOptions<DLMSDBContext> dbOptions, IConfiguration configuration)
    {
        Console.WriteLine("=== Mode LECTURE DES PROFILS (log detaille) ===");
        Console.WriteLine();

        var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profile_logs");
        Directory.CreateDirectory(outputDir);

        var contextFactory = new SimpleDbContextFactory(dbOptions);
        var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });
        var keyServiceLogger = _loggerFactory.CreateLogger<DLMSKeyService>();
        var keyService = new DLMSKeyService(contextFactory, cache, keyServiceLogger);
        var factoryLogger = _loggerFactory.CreateLogger<DLMSGuruxSessionFactory>();
        var sessionLogger = _loggerFactory.CreateLogger<DLMSGuruxSession>();
        var sessionFactory = new DLMSGuruxSessionFactory(factoryLogger, sessionLogger);

        // Plage de lecture configurable via appsettings.json (clé "ProfileReadHours", défaut 6)
        var profileReadHours = int.TryParse(configuration["ProfileReadHours"], out var h) ? h : 6;
        var dateEnd = DateTime.UtcNow;
        var dateStart = dateEnd.AddHours(-profileReadHours);
        Console.WriteLine($"  Plage de lecture (UTC): {dateStart:yyyy-MM-dd HH:mm} -> {dateEnd:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Logs dans: {outputDir}");
        Console.WriteLine();

        var allReports = new List<MeterProfileReport>();

        foreach (var meter in meters)
        {
            var report = new MeterProfileReport { Serial = meter.Serial, Ip = meter.Ip };
            Console.WriteLine($"--- {meter.Serial} ({meter.Ip}:{meter.Port}) ---");

            // 1. Fresh TCP connection
            var transportParams = new DLMSConnectionParameters
            {
                AddressIp = meter.Ip,
                Port = meter.Port,
                Trace = TraceLevel.Off
            };

            var session = sessionFactory.CreateSession(transportParams);
            try
            {
                using var tcpCts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
                var connected = await session.OpenTransportAsync(tcpCts.Token);
                if (!connected)
                {
                    report.Error = "TCP echec";
                    Console.WriteLine($"  TCP ECHEC");
                    allReports.Add(report);
                    continue;
                }

                // 2. HDLC association
                var keys = await keyService.GetKeysAsync("read", meter.Serial, "read");
                if (keys == null || !keys.IsValid)
                {
                    report.Error = "Cles invalides";
                    Console.WriteLine($"  CLES INVALIDES");
                    await session.DisconnectAsync();
                    allReports.Add(report);
                    continue;
                }

                var meterParams = new DLMSConnectionParameters
                {
                    AddressIp = meter.Ip, Port = meter.Port,
                    ClientAddress = "read", SerialNumber = meter.Serial,
                    InterfaceType = "HDLC",
                    Password = keys.Password ?? "",
                    AuthenticationKey = keys.AuthenticationKey,
                    UnicastKey = keys.UnicastKey,
                    Trace = TraceLevel.Off, UseGbt = true
                };

                session.InitializeMeterClient(meterParams, waitTime: 5000, retryCount: 1);

                var hdlcSw = Stopwatch.StartNew();
                session.Reader!.InitializeConnection();
                hdlcSw.Stop();
                report.HdlcMs = hdlcSw.ElapsedMilliseconds;
                report.ConnectionSuccess = true;
                Console.WriteLine($"  HDLC OK ({hdlcSw.ElapsedMilliseconds}ms)");

                // 3. Lire chaque profil (avec reconnexion HDLC après échec)
                // NOTE: ReadRowsByRangeAsync charge l'association + scalers au premier appel
                bool needsReconnect = false;
                foreach (var (obis, name, timeout) in ProfileDefinitions)
                {
                    var profileResult = new ProfileReadResult { ProfileObis = obis, ProfileName = name };
                    Console.Write($"  {name} ({obis}) ... ");

                    // Reconnexion HDLC si le profil précédent a échoué
                    if (needsReconnect)
                    {
                        try
                        {
                            Console.Write("[reconnexion HDLC] ");
                            try { session.Reader?.Disconnect(); } catch { }
                            session.AssociationLoaded = false;
                            session.ScalersLoaded = false;
                            session.InitializeMeterClient(meterParams, waitTime: 5000, retryCount: 1);
                            session.Reader!.InitializeConnection();
                            needsReconnect = false;
                        }
                        catch (Exception reconnEx)
                        {
                            profileResult.Error = $"Reconnexion HDLC echouee: {reconnEx.Message}";
                            profileResult.DurationMs = 0;
                            Console.WriteLine($"RECONNEXION ECHOUEE — arret profils");
                            report.Profiles.Add(profileResult);
                            break;
                        }
                    }

                    session.ReadObjects.Clear();
                    session.ReadObjects.Add(new KeyValuePair<string, int>(obis, 2));

                    var reader = new NonStaticReaderCommunication();
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = await Task.Run(() =>
                            reader.ReadRowsByRangeAsync(session,
                                dateStart.ToString("yyyy-MM-dd HH:mm:ss"),
                                dateEnd.ToString("yyyy-MM-dd HH:mm:ss")))
                            .WaitAsync(TimeSpan.FromSeconds(timeout));

                        sw.Stop();
                        profileResult.DurationMs = sw.ElapsedMilliseconds;

                        if (string.IsNullOrEmpty(result) || result == "Lecture impossible")
                        {
                            profileResult.Error = "Lecture impossible";
                            Console.WriteLine($"ECHEC ({sw.ElapsedMilliseconds}ms)");
                            needsReconnect = true;
                        }
                        else
                        {
                            profileResult.Success = true;
                            profileResult.RawData = result;

                            // Deserialiser : extraire colonnes et rows bruts
                            // Les conversions (scaled, TC*TT) seront appliquées après la boucle
                            var entries = Newtonsoft.Json.JsonConvert.DeserializeObject<
                                List<KeyValuePair<Newtonsoft.Json.Linq.JArray, object[]>>>(result);

                            if (entries != null && entries.Count > 0)
                            {
                                var entry = entries[0];
                                profileResult.Columns = entry.Value.Select(c => c?.ToString() ?? "").ToArray();

                                profileResult.RowsRaw = new List<object[]>();
                                foreach (var rowToken in entry.Key)
                                {
                                    if (rowToken is Newtonsoft.Json.Linq.JArray rowArr)
                                        profileResult.RowsRaw.Add(rowArr.Select(t => (object)t).ToArray());
                                }
                                profileResult.RowCount = profileResult.RowsRaw.Count;
                            }
                            else
                            {
                                profileResult.RowCount = 0;
                            }

                            Console.WriteLine($"OK — {profileResult.RowCount} lignes ({sw.ElapsedMilliseconds}ms)");

                            var fileName = $"{meter.Serial}_{obis.Replace(".", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                            var filePath = Path.Combine(outputDir, fileName);
                            await File.WriteAllTextAsync(filePath, result);
                        }
                    }
                    catch (TimeoutException)
                    {
                        sw.Stop();
                        profileResult.DurationMs = sw.ElapsedMilliseconds;
                        profileResult.Error = $"Timeout ({timeout}s)";
                        Console.WriteLine($"TIMEOUT ({timeout}s)");
                        needsReconnect = true;
                    }
                    catch (Exception ex)
                    {
                        sw.Stop();
                        profileResult.DurationMs = sw.ElapsedMilliseconds;
                        profileResult.Error = ex.Message;
                        Console.WriteLine($"ERREUR — {ex.Message}");
                        needsReconnect = true;
                    }

                    report.Profiles.Add(profileResult);
                }

                // 4. Extraire scalers et TC/TT (après lecture profils, objets correctement chargés)
                double tcVal = 1.0, ttVal = 1.0;
                try
                {
                    var regObjects = session.Client!.Objects.GetObjects(
                        new ObjectType[] { ObjectType.Register, ObjectType.ExtendedRegister, ObjectType.DemandRegister });

                    foreach (GXDLMSObject obj in regObjects)
                    {
                        double scaler = 1;
                        int unitCode = 0;
                        if (obj is GXDLMSRegister reg)
                        {
                            scaler = reg.Scaler;
                            unitCode = (int)reg.Unit;
                        }
                        else if (obj is GXDLMSExtendedRegister ereg)
                        {
                            scaler = ereg.Scaler;
                            unitCode = (int)ereg.Unit;
                        }
                        else if (obj is GXDLMSDemandRegister dreg)
                        {
                            scaler = dreg.Scaler;
                            unitCode = (int)dreg.Unit;
                        }
                        report.Scalers[obj.LogicalName] = new ScalerInfo
                        {
                            Scaler = scaler,
                            Unit = ((Unit)unitCode).ToString(),
                            UnitCode = unitCode
                        };
                    }
                    Console.WriteLine($"  Scalers: {report.Scalers.Count} registres");
                    // Afficher les scalers des registres énergie/puissance/courant/tension
                    foreach (var ks in report.Scalers
                        .Where(s => s.Key.StartsWith("1.0.") && (
                            s.Key.Contains(".8.0.") || s.Key.Contains(".7.0.") ||
                            s.Key.StartsWith("1.0.31.") || s.Key.StartsWith("1.0.32.") ||
                            s.Key.StartsWith("1.0.51.") || s.Key.StartsWith("1.0.52.") ||
                            s.Key.StartsWith("1.0.71.") || s.Key.StartsWith("1.0.72.") ||
                            s.Key.StartsWith("1.0.14.")))
                        .OrderBy(s => s.Key).Take(12))
                        Console.WriteLine($"    {ks.Key}: scaler={ks.Value.Scaler}, unit={ks.Value.Unit}");

                    // Lire TC/TT
                    foreach (var obisCode in TcTtObis)
                    {
                        try
                        {
                            var obj = session.Client.Objects.FindByLN(ObjectType.None, obisCode);
                            if (obj != null)
                            {
                                var val = session.Reader!.Read(obj, 2);
                                report.TcTtValues[obisCode] = val;
                            }
                            else
                            {
                                report.TcTtValues[obisCode] = null;
                            }
                        }
                        catch (Exception tcEx)
                        {
                            report.TcTtValues[obisCode] = $"ERR:{tcEx.Message}";
                        }
                    }

                    // CT ratio = numerator(4.2) / denominator(4.5), VT ratio = numerator(4.3) / denominator(4.6)
                    double ctNum = 1, ctDen = 1, vtNum = 1, vtDen = 1;
                    if (report.TcTtValues.TryGetValue("1.0.0.4.2.255", out var v42) && v42 != null)
                        double.TryParse(v42.ToString(), out ctNum);
                    if (report.TcTtValues.TryGetValue("1.0.0.4.5.255", out var v45) && v45 != null)
                        double.TryParse(v45.ToString(), out ctDen);
                    if (report.TcTtValues.TryGetValue("1.0.0.4.3.255", out var v43) && v43 != null)
                        double.TryParse(v43.ToString(), out vtNum);
                    if (report.TcTtValues.TryGetValue("1.0.0.4.6.255", out var v46) && v46 != null)
                        double.TryParse(v46.ToString(), out vtDen);

                    tcVal = ctDen > 0 ? ctNum / ctDen : 1.0;  // CT = 150/5 = 30
                    ttVal = vtDen > 0 ? vtNum / vtDen : 1.0;  // VT = 330/1 = 330
                    Console.WriteLine($"  CT={ctNum}/{ctDen}={tcVal}, VT={vtNum}/{vtDen}={ttVal}, CT*VT={tcVal * ttVal}");

                    // Appliquer conversions aux données déjà lues
                    // NOTE: les valeurs "brut" sont déjà scalées par Gurux (GetScalersAndUnits appliqué)
                    foreach (var p in report.Profiles.Where(p => p.Success && p.RowsRaw != null && p.Columns != null))
                    {
                        var colTcTtFactors = new double[p.Columns!.Length];
                        var colConvFactors = new double[p.Columns.Length]; // ÷1000 pour énergie/puissance
                        for (int ci = 0; ci < p.Columns.Length; ci++)
                        {
                            colTcTtFactors[ci] = GetTcTtFactor(p.Columns[ci], tcVal, ttVal).factor;
                            colConvFactors[ci] = GetDisplayConversionFactor(p.Columns[ci]);
                        }

                        p.RowsConverted = new List<object[]>();
                        p.RowsWithTcTt = new List<object[]>();
                        foreach (var rawRow in p.RowsRaw!)
                        {
                            var convRow = new object[rawRow.Length];
                            var tcttRow = new object[rawRow.Length];
                            for (int ci = 0; ci < rawRow.Length; ci++)
                            {
                                if (ci < p.Columns.Length && rawRow[ci] is IConvertible conv)
                                {
                                    try
                                    {
                                        double val = conv.ToDouble(null);
                                        convRow[ci] = Math.Round(val * colConvFactors[ci], 1);
                                        tcttRow[ci] = val * colTcTtFactors[ci];
                                    }
                                    catch { convRow[ci] = rawRow[ci]; tcttRow[ci] = rawRow[ci]; }
                                }
                                else { convRow[ci] = rawRow[ci]; tcttRow[ci] = rawRow[ci]; }
                            }
                            p.RowsConverted.Add(convRow);
                            p.RowsWithTcTt.Add(tcttRow);
                        }
                    }
                }
                catch (Exception metaEx)
                {
                    Console.WriteLine($"  WARN: Extraction scalers/TC_TT: {metaEx.Message}");
                }

                // 5. Sauvegarder JSON enrichi par compteur
                try
                {
                    var enrichedReport = new
                    {
                        serial = meter.Serial,
                        ip = meter.Ip,
                        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        tc_tt = report.TcTtValues.ToDictionary(
                            kv => kv.Key,
                            kv => kv.Value?.ToString() ?? "null"),
                        scalers = report.Scalers.ToDictionary(
                            kv => kv.Key,
                            kv => new { kv.Value.Scaler, kv.Value.Unit, kv.Value.UnitCode }),
                        profiles = report.Profiles.Select(p => new
                        {
                            obis = p.ProfileObis,
                            name = p.ProfileName,
                            columns = p.Columns,
                            rows_count = p.RowCount,
                            duration_ms = p.DurationMs,
                            success = p.Success,
                            error = p.Error,
                            first_rows_comparison = (p.RowsRaw != null && p.RowsRaw.Count > 0)
                                ? p.RowsRaw.Take(3).Select((row, ri) => new
                                {
                                    brut = row,
                                    converti = p.RowsConverted?.ElementAtOrDefault(ri),
                                    avec_ct_vt = p.RowsWithTcTt?.ElementAtOrDefault(ri)
                                }).ToArray()
                                : null
                        }).ToArray()
                    };

                    var enrichedJson = Newtonsoft.Json.JsonConvert.SerializeObject(enrichedReport, Newtonsoft.Json.Formatting.Indented);
                    var enrichedFileName = $"{meter.Serial}_enriched_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    var enrichedFilePath = Path.Combine(outputDir, enrichedFileName);
                    await File.WriteAllTextAsync(enrichedFilePath, enrichedJson);
                    Console.WriteLine($"  JSON enrichi: {enrichedFileName}");
                }
                catch (Exception enrichEx)
                {
                    Console.WriteLine($"  WARN: JSON enrichi non sauvegarde: {enrichEx.Message}");
                }

                // Disconnect
                try { session.Reader?.Disconnect(); } catch { }
                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                report.Error = ex.Message;
                Console.WriteLine($"  ERREUR: {ex.Message}");
                try { await session.DisconnectAsync(); } catch { }
            }

            allReports.Add(report);
            Console.WriteLine();
        }

        // === Rapport final ===
        Console.WriteLine("=== RAPPORT LECTURE PROFILS ===");
        Console.WriteLine();
        Console.WriteLine($"  {"Compteur",-12} {"HDLC",-8} {"Prof1",-10} {"Prof2",-10} {"Prof3",-10} {"Billing",-10} {"Events",-10}");
        Console.WriteLine($"  {new string('-', 70)}");

        foreach (var r in allReports)
        {
            if (!r.ConnectionSuccess)
            {
                Console.WriteLine($"  {r.Serial,-12} {r.Error}");
                continue;
            }

            var prof1 = r.Profiles.FirstOrDefault(p => p.ProfileObis == "1.0.99.1.0.255");
            var prof2 = r.Profiles.FirstOrDefault(p => p.ProfileObis == "1.0.99.2.0.255");
            var prof3 = r.Profiles.FirstOrDefault(p => p.ProfileObis == "1.0.99.3.0.255");
            var billing = r.Profiles.FirstOrDefault(p => p.ProfileObis == "0.0.98.1.0.255");
            var events = r.Profiles.Where(p => p.ProfileObis.StartsWith("0.0.99.98."));

            string Fmt(ProfileReadResult? p) => p == null ? "-" :
                p.Success ? $"{p.RowCount}r/{p.DurationMs}ms" : $"ERR";

            var eventSummary = events.Any()
                ? $"{events.Count(p => p.Success)}/{events.Count()} OK"
                : "-";

            Console.WriteLine($"  {r.Serial,-12} {r.HdlcMs + "ms",-8} {Fmt(prof1),-10} {Fmt(prof2),-10} {Fmt(prof3),-10} {Fmt(billing),-10} {eventSummary,-10}");
        }

        Console.WriteLine();
        Console.WriteLine($"  Fichiers JSON sauvegardes dans: {outputDir}");
        Console.WriteLine();

        // Detail enrichi des profils par compteur
        foreach (var r in allReports.Where(r => r.ConnectionSuccess))
        {
            Console.WriteLine($"  === {r.Serial} ({r.Ip}) ===");

            // TC/TT values
            if (r.TcTtValues.Count > 0)
            {
                Console.WriteLine($"    TC/TT:");
                foreach (var kv in r.TcTtValues)
                    Console.WriteLine($"      {kv.Key} = {kv.Value ?? "null"}");
            }

            // Key scalers (energy, power, current, voltage, frequency)
            var keyScalerObis = r.Scalers
                .Where(s => s.Key.StartsWith("1.0.") && (
                    s.Key.Contains(".8.0.") || s.Key.Contains(".7.0.") ||
                    s.Key.StartsWith("1.0.31.") || s.Key.StartsWith("1.0.32.") ||
                    s.Key.StartsWith("1.0.51.") || s.Key.StartsWith("1.0.52.") ||
                    s.Key.StartsWith("1.0.71.") || s.Key.StartsWith("1.0.72.") ||
                    s.Key.StartsWith("1.0.14.")))
                .OrderBy(s => s.Key)
                .Take(12);
            if (keyScalerObis.Any())
            {
                Console.WriteLine($"    Scalers cles:");
                foreach (var ks in keyScalerObis)
                    Console.WriteLine($"      {ks.Key}: scaler={ks.Value.Scaler}, unit={ks.Value.Unit}");
            }

            // Per-profile detail
            foreach (var p in r.Profiles)
            {
                var status = p.Success ? $"{p.RowCount} lignes en {p.DurationMs}ms" : $"ECHEC: {p.Error}";
                Console.WriteLine($"    {p.ProfileName,-30} {status}");

                // Show first 2 rows in 3 views for successful profiles
                if (p.Success && p.RowsRaw != null && p.RowsRaw.Count > 0 && p.Columns != null)
                {
                    // Abbreviated column headers: C.D from OBIS (e.g., "1.0.1.8.0.255" -> "1.8")
                    var colHeaders = p.Columns.Select(col =>
                    {
                        var parts = col.Split('.');
                        return parts.Length >= 6 ? $"{parts[2]}.{parts[3]}" : col;
                    }).ToArray();
                    Console.WriteLine($"      Colonnes: [{string.Join(", ", colHeaders)}]");

                    var rowsToShow = Math.Min(2, p.RowsRaw.Count);
                    for (int ri = 0; ri < rowsToShow; ri++)
                    {
                        string FmtRow(object[]? row) => row == null ? "null"
                            : string.Join(", ", row.Select(v =>
                            {
                                var s = v?.ToString() ?? "null";
                                return s.Length > 16 ? s.Substring(0, 16) + ".." : s;
                            }));

                        Console.WriteLine($"      Ligne {ri}: brut=[{FmtRow(p.RowsRaw[ri])}]");
                        Console.WriteLine($"              conv=[{FmtRow(p.RowsConverted?.ElementAtOrDefault(ri))}]");
                        Console.WriteLine($"              ct*vt=[{FmtRow(p.RowsWithTcTt?.ElementAtOrDefault(ri))}]");
                    }
                }
            }
            Console.WriteLine();
        }
    }

    private static (double factor, string label) GetTcTtFactor(string obisCode, double tc, double tt)
    {
        var parts = obisCode.Split('.');
        if (parts.Length < 6) return (1.0, "x1");
        if (!int.TryParse(parts[2], out int c) || !int.TryParse(parts[3], out int d))
            return (1.0, "x1");

        // Energy registers (D=8): xTCxTT
        if (d == 8) return (tc * tt, $"xTCxTT({tc * tt})");

        // Power registers (D=7):
        if (d == 7)
        {
            if (c == 31 || c == 51 || c == 71) return (tc, $"xTC({tc})");
            if (c == 32 || c == 52 || c == 72) return (tt, $"xTT({tt})");
            if (c == 14 || c == 81) return (1.0, "x1");
            return (tc * tt, $"xTCxTT({tc * tt})");
        }

        return (1.0, "x1");
    }

    /// <summary>
    /// Facteur de conversion d'affichage : ÷1000 pour énergie (Wh→kWh) et puissance (W→kW),
    /// ×1 pour tension, courant, fréquence, angle.
    /// </summary>
    private static double GetDisplayConversionFactor(string obisCode)
    {
        var parts = obisCode.Split('.');
        if (parts.Length < 6) return 1.0;
        if (!int.TryParse(parts[3], out int d)) return 1.0;

        // D=8 (énergie) ou D=7 (puissance) : ÷1000 sauf courant/tension/fréquence/angle
        if (d == 8) return 0.001; // Wh → kWh, varh → kvarh

        if (d == 7 && int.TryParse(parts[2], out int c))
        {
            if (c == 31 || c == 51 || c == 71) return 1.0; // Courant (A)
            if (c == 32 || c == 52 || c == 72) return 1.0; // Tension (V)
            if (c == 14) return 1.0; // Fréquence (Hz)
            if (c == 81) return 1.0; // Angle (°)
            return 0.001; // Autres puissances W → kW, var → kvar
        }

        return 1.0;
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
