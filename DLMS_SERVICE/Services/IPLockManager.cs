using DLMS_MODELS.CompteurEquipementDomain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DLMS_SERVICE.Services
{
    public interface IIPLockManager
    {
        Task<T> ExecuteWithLockAsync<T>(string ip, int port, Func<Task<T>> operation);
        Task ExecuteWithLockAsync(string ip, int port, Func<Task> operation);
        string GetLockKey(string ip, int port);
    }

    public class IPLockManager : IIPLockManager
    {
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _ipLocks = new();
        private readonly ILogger<IPLockManager> _logger;

        public IPLockManager(ILogger<IPLockManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetLockKey(string ip, int port)
        {
            return $"{ip}:{port}";
        }

        public async Task<T> ExecuteWithLockAsync<T>(string ip, int port, Func<Task<T>> operation)
        {
            var lockKey = GetLockKey(ip, port);
            var semaphore = _ipLocks.GetOrAdd(lockKey, _ => new SemaphoreSlim(1, 1));

            _logger.LogDebug("🔒 Attente verrou IP {LockKey}", lockKey);
            await semaphore.WaitAsync();
            
            try
            {
                _logger.LogDebug("✅ Verrou IP acquis {LockKey}", lockKey);
                return await operation();
            }
            finally
            {
                _logger.LogDebug("🔓 Verrou IP libéré {LockKey}", lockKey);
                semaphore.Release();
            }
        }

        public async Task ExecuteWithLockAsync(string ip, int port, Func<Task> operation)
        {
            await ExecuteWithLockAsync<object>(ip, port, async () =>
            {
                await operation();
                return null;
            });
        }
    }
}
