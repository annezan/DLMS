using System.Collections.Concurrent;

namespace DLMS_SERVICE.Services
{
    /// <summary>
    /// Service de verrou par IP pour éviter les accès concurrents au même compteur
    /// entre les lectures horaires et les commandes à la demande.
    /// </summary>
    public interface IMeterLockService
    {
        Task<IDisposable> AcquireAsync(string ipAddress, TimeSpan? timeout = null, CancellationToken ct = default);
    }

    public class MeterLockService : IMeterLockService
    {
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public async Task<IDisposable> AcquireAsync(string ipAddress, TimeSpan? timeout = null, CancellationToken ct = default)
        {
            var semaphore = _locks.GetOrAdd(ipAddress, _ => new SemaphoreSlim(1, 1));
            var effectiveTimeout = timeout ?? TimeSpan.FromSeconds(120);

            if (!await semaphore.WaitAsync(effectiveTimeout, ct))
                throw new TimeoutException($"Impossible d'acquérir le verrou pour l'IP {ipAddress} après {effectiveTimeout.TotalSeconds}s");

            return new LockRelease(semaphore);
        }

        private sealed class LockRelease : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;
            private int _disposed;

            public LockRelease(SemaphoreSlim semaphore) => _semaphore = semaphore;

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 0)
                    _semaphore.Release();
            }
        }
    }
}
