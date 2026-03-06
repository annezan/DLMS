using DLMS_MODELS;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DLMS_SERVICE.Services
{
    public enum JobType
    {
        Command = 1,    // Priorité maximale
        Hourly = 2,     // Priorité haute
        Missing = 3      // Priorité basse
    }

    public class DLMSJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public JobType Type { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public CompteurEquipement CompteurEquipement { get; set; }
        public List<CompteurEquipement> CompteurEquipements { get; set; }
        public List<MissingReadInfo> MissingReads { get; set; }
        public ActiveCommandInfo Command { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CycleStartTime { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public interface IIPJobQueue
    {
        Task EnqueueAsync(DLMSJob job);
        Task<DLMSJob> DequeueAsync(string ip, int port, CancellationToken ct);
        Task<int> GetQueueCountAsync(string ip, int port);
        Task<List<DLMSJob>> GetAllQueuesStatusAsync();
    }

    public class IPJobQueue : IIPJobQueue
    {
        private readonly ConcurrentDictionary<string, PriorityQueue<DLMSJob, int>> _queues = new();
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _queueLocks = new();
        private readonly ILogger<IPJobQueue> _logger;

        public IPJobQueue(ILogger<IPJobQueue> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetQueueKey(string ip, int port)
        {
            return $"{ip}:{port}";
        }

        public async Task EnqueueAsync(DLMSJob job)
        {
            var queueKey = GetQueueKey(job.IP, job.Port);
            var queueLock = _queueLocks.GetOrAdd(queueKey, _ => new SemaphoreSlim(1, 1));

            await queueLock.WaitAsync();
            try
            {
                var queue = _queues.GetOrAdd(queueKey, _ => new PriorityQueue<DLMSJob, int>());
                queue.Enqueue(job, (int)job.Type);
                
                _logger.LogDebug("📦 Job {JobId} de type {JobType} ajouté à la queue {QueueKey} (priorité: {Priority})", 
                    job.Id, job.Type, queueKey, (int)job.Type);
            }
            finally
            {
                queueLock.Release();
            }
        }

        public async Task<DLMSJob> DequeueAsync(string ip, int port, CancellationToken ct)
        {
            var queueKey = GetQueueKey(ip, port);
            var queueLock = _queueLocks.GetOrAdd(queueKey, _ => new SemaphoreSlim(1, 1));

            await queueLock.WaitAsync(ct);
            try
            {
                if (_queues.TryGetValue(queueKey, out var queue) && queue.Count > 0)
                {
                    var job = queue.Dequeue();
                    job.StartedAt = DateTime.Now;
                    
                    _logger.LogDebug("🎯 Job {JobId} de type {JobType} retiré de la queue {QueueKey}", 
                        job.Id, job.Type, queueKey);
                    
                    return job;
                }
                
                return null;
            }
            finally
            {
                queueLock.Release();
            }
        }

        public async Task<int> GetQueueCountAsync(string ip, int port)
        {
            var queueKey = GetQueueKey(ip, port);
            var queueLock = _queueLocks.GetOrAdd(queueKey, _ => new SemaphoreSlim(1, 1));

            await queueLock.WaitAsync();
            try
            {
                return _queues.TryGetValue(queueKey, out var queue) ? queue.Count : 0;
            }
            finally
            {
                queueLock.Release();
            }
        }

        public async Task<List<DLMSJob>> GetAllQueuesStatusAsync()
        {
            var status = new List<DLMSJob>();
            
            foreach (var queueLock in _queueLocks.Values)
            {
                await queueLock.WaitAsync();
            }
            
            try
            {
                foreach (var kvp in _queues)
                {
                    var jobs = kvp.Value.UnorderedItems.ToList();
                    status.AddRange(jobs.Select(item => item.Element));
                }
            }
            finally
            {
                foreach (var queueLock in _queueLocks.Values)
                {
                    queueLock.Release();
                }
            }
            
            return status;
        }
    }
}
