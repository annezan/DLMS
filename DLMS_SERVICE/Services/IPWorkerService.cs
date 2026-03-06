using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using DLMS_MODELS;
using Microsoft.Extensions.DependencyInjection;

namespace DLMS_SERVICE.Services
{
    public interface IIPWorkerService
    {
        Task StartAsync(CancellationToken stoppingToken);
        Task EnqueueHourlyReadsAsync(List<CompteurEquipement> compteurs, DateTime readTime, DateTime? cycleStartTime = null);
        Task EnqueueMissingReadsAsync(List<MissingReadInfo> missingReads);
        Task EnqueueCommandAsync(ActiveCommandInfo command);
    }

    public class IPWorkerService : BackgroundService, IIPWorkerService
    {
        private readonly IIPJobQueue _jobQueue;
        private readonly IDLMSParallelReadService _parallelReadService;
        private readonly ILogger<IPWorkerService> _logger;
        private readonly ConcurrentDictionary<string, Task> _workerTasks = new();
        private readonly ConcurrentDictionary<string, bool> _activeWorkers = new();
        private readonly SemaphoreSlim _globalWorkerLimit;
        private readonly IServiceScopeFactory _scopeFactory;

        public IPWorkerService(
            IIPJobQueue jobQueue,
            IDLMSParallelReadService parallelReadService,
            ILogger<IPWorkerService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _jobQueue = jobQueue ?? throw new ArgumentNullException(nameof(jobQueue));
            _parallelReadService = parallelReadService ?? throw new ArgumentNullException(nameof(parallelReadService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            
            // Récupérer le nombre d'IP uniques depuis la base de données
            var maxWorkers = GetMaxWorkersFromDatabase().GetAwaiter().GetResult();
            _globalWorkerLimit = new SemaphoreSlim(maxWorkers, maxWorkers);
            _logger.LogInformation("🔧 Limite de workers initialisée à {MaxWorkers} (basée sur les IP uniques)", maxWorkers);
        }

        private async Task<int> GetMaxWorkersFromDatabase()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries.ICompteurEquipementQueryRepository>();
                var uniqueIpCount = await repository.GetUniqueIpCountAsync();
                return Math.Max(1, uniqueIpCount); // Minimum 1 worker
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Impossible de récupérer le nombre d'IP uniques, utilisation de la valeur par défaut 27");
                return 27; // Valeur par défaut actuelle
            }
        }

        public async Task EnqueueHourlyReadsAsync(List<CompteurEquipement> compteurs, DateTime readTime, DateTime? cycleStartTime = null)
        {
            if (compteurs == null || !compteurs.Any())
                return;

            var ip = compteurs.First().Equipement?.AdresseIp;
            var port = int.TryParse(compteurs.First().Equipement?.Port, out var portValue) ? portValue : 0;

            if (string.IsNullOrEmpty(ip))
            {
                _logger.LogWarning("⚠️ Groupe de compteurs sans IP ignoré");
                return;
            }

            var job = new DLMSJob
            {
                Type = JobType.Hourly,
                IP = ip,
                Port = port,
                CompteurEquipements = compteurs,
                CycleStartTime = cycleStartTime ?? DateTime.Now
            };

            await _jobQueue.EnqueueAsync(job);
            _logger.LogDebug("📅 Lectures horaires groupe en queue pour {Count} compteurs (IP: {IP})", 
                compteurs.Count, ip);
        }

        public async Task EnqueueMissingReadsAsync(List<MissingReadInfo> missingReads)
        {
            if (missingReads == null || !missingReads.Any())
                return;

            // Grouper par IP pour optimiser
            var groupedByIp = missingReads
                .GroupBy(m => new { m.AdresseIp, Port = int.TryParse(m.Port, out var portValue) ? portValue : 0 })
                .ToList();

            foreach (var group in groupedByIp)
            {
                var job = new DLMSJob
                {
                    Type = JobType.Missing,
                    IP = group.Key.AdresseIp,
                    Port = group.Key.Port,
                    MissingReads = group.ToList()
                };

                await _jobQueue.EnqueueAsync(job);
                _logger.LogDebug("🔍 Rattrapage en queue pour {Count} lectures (IP: {IP})", 
                    group.Count(), group.Key.AdresseIp);
            }
        }

        public async Task EnqueueCommandAsync(ActiveCommandInfo command)
        {
            var ip = command.AdresseIp;
            var port = int.TryParse(command.Port, out var portValue) ? portValue : 0;

            if (string.IsNullOrEmpty(ip))
            {
                _logger.LogWarning("⚠️ Commande sans IP ignorée: {CommandId}", command.CommandId);
                return;
            }

            var job = new DLMSJob
            {
                Type = JobType.Command,
                IP = ip,
                Port = port,
                Command = command
            };

            await _jobQueue.EnqueueAsync(job);
            _logger.LogDebug("⚡ Commande en queue pour {CommandId} (IP: {IP})", 
                command.CommandId, ip);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Démarrage du service de workers IP");

            // Démarrer les workers pour chaque IP connue
            // En pratique, on pourrait découvrir les IP dynamiquement
            var knownIPs = new HashSet<string>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Récupérer le statut de toutes les queues
                    var allJobs = await _jobQueue.GetAllQueuesStatusAsync();
                    var activeQueues = allJobs
                        .GroupBy(j => $"{j.IP}:{j.Port}")
                        .Where(g => g.Any())
                        .ToList();

                    foreach (var queueGroup in activeQueues)
                    {
                        var queueKey = queueGroup.Key;
                        var parts = queueKey.Split(':');
                        var ip = parts[0];
                        var port = int.Parse(parts[1]);

                        // Démarrer un worker pour cette queue si nécessaire
                        if (!_activeWorkers.ContainsKey(queueKey))
                        {
                            await StartWorkerForQueueAsync(ip, port, stoppingToken);
                        }
                    }

                    // Nettoyer les workers terminés
                    var completedWorkers = _workerTasks
                        .Where(kvp => kvp.Value.IsCompleted)
                        .ToList();

                    foreach (var completed in completedWorkers)
                    {
                        _workerTasks.TryRemove(completed.Key, out _);
                        _activeWorkers.TryRemove(completed.Key, out _);
                    }

                    await Task.Delay(5000, stoppingToken); // Vérifier toutes les 5 secondes
                }
                catch (Exception ex) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("🛑 Arrêt du service de workers IP");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erreur dans la boucle principale des workers IP");
                    await Task.Delay(10000, stoppingToken);
                }
            }

            // Attendre que tous les workers se terminent
            if (_workerTasks.Any())
            {
                _logger.LogInformation("⏳ Attente de la fin des workers...");
                await Task.WhenAll(_workerTasks.Values);
            }
        }

        private async Task StartWorkerForQueueAsync(string ip, int port, CancellationToken stoppingToken)
        {
            var queueKey = $"{ip}:{port}";
            
            await _globalWorkerLimit.WaitAsync(stoppingToken);
            try
            {
                var workerTask = Task.Run(async () =>
                {
                    _logger.LogInformation("🔧 Démarrage worker pour {QueueKey}", queueKey);
                    _activeWorkers.TryAdd(queueKey, true);

                    try
                    {
                        await WorkerLoopAsync(ip, port, stoppingToken);
                    }
                    finally
                    {
                        _activeWorkers.TryRemove(queueKey, out _);
                        _logger.LogInformation("🔧 Arrêt worker pour {QueueKey}", queueKey);
                    }
                }, stoppingToken);

                _workerTasks.TryAdd(queueKey, workerTask);
            }
            finally
            {
                _globalWorkerLimit.Release();
            }
        }

        private async Task WorkerLoopAsync(string ip, int port, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var job = await _jobQueue.DequeueAsync(ip, port, stoppingToken);
                    if (job == null)
                    {
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }

                    await ExecuteJobAsync(job, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erreur dans le worker pour {IP}:{Port}", ip, port);
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task ExecuteJobAsync(DLMSJob job, CancellationToken ct)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation("🎯 Exécution job {JobType} pour {IP}:{Port}", 
                    job.Type, job.IP, job.Port);

                switch (job.Type)
                {
                    case JobType.Hourly:
                        // Utiliser CompteurEquipements si disponible, sinon CompteurEquipement (compatibilité)
                        var meters = job.CompteurEquipements ?? new List<CompteurEquipement> { job.CompteurEquipement };
                        await _parallelReadService.ProcessUmadGroupAsync(
                            job.IP, job.Port, meters, ct, job.CycleStartTime);
                        break;
                        
                    case JobType.Missing:
                        await _parallelReadService.ProcessUmadMissingReadsGroupAsync(
                            job.IP, job.Port, job.MissingReads, ct);
                        break;
                        
                    case JobType.Command:
                        await _parallelReadService.ProcessUmadCommandGroupAsync(
                            job.IP, job.Port, new List<ActiveCommandInfo> { job.Command }, ct);
                        break;
                }

                job.CompletedAt = DateTime.Now;
                stopwatch.Stop();
                
                _logger.LogInformation("✅ Job {JobType} terminé en {ElapsedMs}ms pour {IP}:{Port}", 
                    job.Type, stopwatch.ElapsedMilliseconds, job.IP, job.Port);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                job.CompletedAt = DateTime.Now;
                
                _logger.LogError(ex, "❌ Échec job {JobType} pour {IP}:{Port} après {ElapsedMs}ms", 
                    job.Type, job.IP, job.Port, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
