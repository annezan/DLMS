using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using DLMS_MODELS;
using System.ServiceProcess;
using DLMS_DAL;
using Microsoft.EntityFrameworkCore;
using DLMS_UTILITIES;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS.Infrastructure;
using DLMS_DAL.Datas;
using Microsoft.Extensions.Configuration;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.EventDomainDal.Repositories.Queries;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using DLMS_SERVICE.Services;
using DLMS_SERVICE.Services.MultiPass;
using Microsoft.Extensions.Caching.Memory;

namespace DLMS_SERVICE
{
    public class Program
    {
        public const string ServiceName = "ReadDb";

        public static void Main(string[] args)
        {
            // Prevent thread pool starvation from synchronous Gurux HDLC calls
            ThreadPool.SetMinThreads(50, 50);

            ConfigureSerilog();

            try
            {
                Log.Information("=== Démarrage du service DLMS ===");

                if (Environment.UserInteractive)
                {
                    Log.Information("Mode: Console interactif");
                }
                else
                {
                    Log.Information("Mode: Service Windows");
                }


                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Le service s'est arrêté suite à une erreur fatale");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseWindowsService()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                });

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Log.Information("Configuration des services...");

            try
            {
                // Configuration de la base de données
                var connectionString = GetConnectionString(configuration);
                GlobalVariable.ConString = connectionString;

                services.AddDbContextFactory<DLMSDBContext>(opts =>
                    opts.UseSqlServer(connectionString, options =>
                    {
                        options.CommandTimeout(120);
                    }));

                // Infrastructure et repositories
                services.AddInfrastructure();
                
                // Services spécialisés
                services.AddSingleton<IDLMSKeyService, DLMSKeyService>();
                services.AddTransient<IEventQueryRepository, EventQueryRepository>();
                
                // Configuration du cache mémoire
                services.AddMemoryCache(options =>
                {
                    options.SizeLimit = 1000; // Limite de 1000 éléments
                });
                
                // === NOUVELLE ARCHITECTURE PERFORMANCE DLMS ===
                // IP Lock Manager - Évite les collisions IP
                services.AddSingleton<IIPLockManager, IPLockManager>();
                
                // IP Job Queue - Files priorisées par IP
                services.AddSingleton<IIPJobQueue, IPJobQueue>();
                
                // IP Worker Service - Workers configurables via appsettings
                services.AddHostedService<IPWorkerService>();
                services.AddSingleton<IIPWorkerService, IPWorkerService>();
                
                // DLMS Metrics Service - Monitoring performance
                services.AddSingleton<IDLMSMetricsService, DLMSMetricsService>();
                services.AddHostedService<MetricsReportingService>();

                // Meter Health Tracker - Suivi santé compteurs pour timeout adaptatif et tri intelligent
                services.AddSingleton<IMeterHealthTracker, MeterHealthTracker>();
                
                // Workers dédiés pour chaque type de tâche (mis à jour pour nouvelle architecture)
                services.AddHostedService<HourlyReadsWorker>();
                services.AddHostedService<MissingReadsWorker>();
                //services.AddHostedService<ActiveCommandsWorker>();
                
                // Factory pour sessions Gurux thread-safe
                services.AddSingleton<IDLMSGuruxSessionFactory, DLMSGuruxSessionFactory>();
                
                // Services métier spécialisés (architecture propre)
                services.AddTransient<IDLMSHardwareService, DLMSHardwareService>();
                services.AddTransient<IDLMSParallelReadService, DLMSParallelReadService>();
                services.AddTransient<IDLMSMissingReadService, DLMSMissingReadService>();
                services.AddTransient<IDLMSCommandProcessorService, DLMSCommandProcessorService>();
                
                // === MULTI-PASS ORCHESTRATION ===
                services.Configure<MultiPassConfig>(configuration.GetSection("MultiPass"));
                services.AddSingleton<ITcpScanService, TcpScanService>();
                services.AddTransient<IReadSessionOrchestrator, ReadSessionOrchestrator>();
                services.AddTransient<ISessionReportService, SessionReportService>();
                services.AddTransient<IReadingCycleManager, ReadingCycleManager>();

                // DataProcessingService en dernier pour éviter les dépendances circulaires
                services.AddScoped<IDataProcessingService, DataProcessingService>();
                
                // Factory pour DataProcessingService pour les services Singleton
                services.AddSingleton<IDataProcessingServiceFactory, DataProcessingServiceFactory>();
                
                services.AddTransient<CompteurEquipementUtilities>();

                // Configuration JSON
                services.Configure<JsonOptions>(options =>
                {
                    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

                
                Log.Information("Services configurés avec succès - Architecture parallèle DLMS avec gestion par IP activée");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Erreur lors de la configuration des services");
                throw;
            }
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var serveur = configuration.GetSection("Serveur").Value;
            var db = configuration.GetSection("DB").Value;
            var env = configuration.GetSection("env").Value;

            Log.Information("Environnement: {Environment}, Serveur: {Serveur}, Database: {Database}", 
                env, serveur, db);

            var ascuser = new asc_connection.connection();
            var user = ascuser.asc_user;
            var pass = ascuser.asc_pass;
            Log.Information("Credentials récupérés depuis asc_connection");
              
            if (string.IsNullOrEmpty(serveur) || string.IsNullOrEmpty(db))
            {
                throw new InvalidOperationException("Configuration de base de données incomplète: Serveur ou DB manquant");
            }

            return $"Server={serveur};Database={db};Trusted_Connection=false;TrustServerCertificate=true;MultipleActiveResultSets=true;user id={user};password={pass};";
        }

        private static void ConfigureSerilog()
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", ServiceName)
                .Enrich.WithProperty("Machine", Environment.MachineName)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                // Logs principaux du service
                .WriteTo.File(
                    path: Path.Combine(logPath, "service-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    shared: true)
                // Logs d'erreurs généraux
                .WriteTo.File(
                    path: Path.Combine(logPath, "errors-.log"),
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 90,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    shared: true)
                // Logs dédiés pour HourlyReadsWorker
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext") && 
                        e.Properties["SourceContext"].ToString().Contains("HourlyReadsWorker"))
                    .WriteTo.File(
                        path: Path.Combine(logPath, "hourly-reads-worker-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                        shared: true,
                        restrictedToMinimumLevel: LogEventLevel.Information))
                // Logs dédiés pour MissingReadsWorker
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext") && 
                        e.Properties["SourceContext"].ToString().Contains("MissingReadsWorker"))
                    .WriteTo.File(
                        path: Path.Combine(logPath, "missing-reads-worker-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                        shared: true,
                        restrictedToMinimumLevel: LogEventLevel.Information))
                // Logs dédiés pour ActiveCommandsWorker
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext") && 
                        e.Properties["SourceContext"].ToString().Contains("ActiveCommandsWorker"))
                    .WriteTo.File(
                        path: Path.Combine(logPath, "active-commands-worker-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                        shared: true,
                        restrictedToMinimumLevel: LogEventLevel.Information))
                // Logs dédiés pour ReadSessionOrchestrator (multi-pass)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext") &&
                        (e.Properties["SourceContext"].ToString().Contains("ReadSessionOrchestrator") ||
                         e.Properties["SourceContext"].ToString().Contains("ReadingCycleManager") ||
                         e.Properties["SourceContext"].ToString().Contains("SessionReportService")))
                    .WriteTo.File(
                        path: Path.Combine(logPath, "multi-pass-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                        shared: true,
                        restrictedToMinimumLevel: LogEventLevel.Information))
                // Logs dédiés pour IPWorkerService
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext") &&
                        e.Properties["SourceContext"].ToString().Contains("IPWorkerService"))
                    .WriteTo.File(
                        path: Path.Combine(logPath, "ip-worker-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                        shared: true,
                        restrictedToMinimumLevel: LogEventLevel.Information))
                .CreateLogger();
        }
    }
}
