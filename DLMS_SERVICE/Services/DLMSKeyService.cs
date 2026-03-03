using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using DLMS_DAL;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using DLMS_DAL.Datas;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSKeyService
    {
        Task<DLMSKeys?> GetKeysAsync(string clientAddress, string serialNumber, string keyType = "read");
        Task SetKeysAsync(string clientAddress, string serialNumber, string keyType, DLMSKeys keys);
        Task ClearCacheAsync();
    }

    public class DLMSKeys
    {
        public string AuthenticationKey { get; set; } = string.Empty;
        public string UnicastKey { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsValid => !string.IsNullOrEmpty(AuthenticationKey) && !string.IsNullOrEmpty(UnicastKey);
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    }

    public class DLMSKeyService : IDLMSKeyService
    {
        private readonly IDbContextFactory<DLMSDBContext> _contextFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DLMSKeyService> _logger;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

        public DLMSKeyService(
            IDbContextFactory<DLMSDBContext> contextFactory,
            IMemoryCache cache,
            ILogger<DLMSKeyService> logger)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DLMSKeys?> GetKeysAsync(string clientAddress, string serialNumber, string keyType = "read")
        {
            try
            {
                var cacheKey = GenerateCacheKey(clientAddress, serialNumber, keyType);

                // Tentative de récupération depuis le cache
                if (_cache.TryGetValue(cacheKey, out DLMSKeys? cachedKeys) && cachedKeys != null)
                {
                    if (!cachedKeys.IsExpired)
                    {
                        _logger.LogDebug("Clés DLMS récupérées depuis le cache pour {ClientAddress}/{SerialNumber}/{KeyType}", 
                            clientAddress, serialNumber, keyType);
                        return cachedKeys;
                    }
                    else
                    {
                        _logger.LogDebug("Clés DLMS expirées dans le cache pour {ClientAddress}/{SerialNumber}/{KeyType}", 
                            clientAddress, serialNumber, keyType);
                        _cache.Remove(cacheKey);
                    }
                }

                // Lecture depuis la base de données avec DbContext unique par appel
                await using var context = await _contextFactory.CreateDbContextAsync();
                
                var authentication = await context.AssociationKeys
                    .Where(x => x.Type == keyType &&
                               x.Keyname == "authentication" &&
                               x.CompteurId.Contains(serialNumber))
                    .FirstOrDefaultAsync();
                    
                var unicast = await context.AssociationKeys
                    .Where(x => x.Type == keyType &&
                               x.Keyname == "unicast" &&
                               x.CompteurId.Contains(serialNumber))
                    .FirstOrDefaultAsync();

                if (authentication == null || unicast == null)
                {
                    return null;
                }

                var keys = new DLMSKeys
                {
                    AuthenticationKey = Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS"),
                    UnicastKey = Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS"),
                    Password = authentication.Pwd,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.Add(_cacheDuration)
                };

                // Mise en cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration,
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    Size = 1
                };

                _cache.Set(cacheKey, keys, cacheOptions);
                
                _logger.LogDebug("Clés DLMS chargées et mises en cache pour {ClientAddress}/{SerialNumber}/{KeyType}", 
                    clientAddress, serialNumber, keyType);

                return keys;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des clés DLMS pour {ClientAddress}/{SerialNumber}/{KeyType}", 
                    clientAddress, serialNumber, keyType);
                return null;
            }
        }

        public async Task SetKeysAsync(string clientAddress, string serialNumber, string keyType, DLMSKeys keys)
        {
            try
            {
                var cacheKey = GenerateCacheKey(clientAddress, serialNumber, keyType);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration,
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    Size = 1
                };

                _cache.Set(cacheKey, keys, cacheOptions);

                _logger.LogDebug("Clés DLMS mises en cache pour {ClientAddress}/{SerialNumber}/{KeyType}", 
                    clientAddress, serialNumber, keyType);
                
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise en cache des clés DLMS pour {ClientAddress}/{SerialNumber}/{KeyType}", 
                    clientAddress, serialNumber, keyType);
                throw;
            }
        }

        public async Task ClearCacheAsync()
        {
            try
            {
                _cache.Remove(string.Empty); // Vide tout le cache
                await Task.CompletedTask;
                
                _logger.LogInformation("Cache des clés DLMS vidé");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du vidage du cache des clés DLMS");
                throw;
            }
        }

        private string GenerateCacheKey(string clientAddress, string serialNumber, string keyType)
        {
            // Création d'une clé de cache unique et stable
            var keyData = $"{clientAddress}|{serialNumber}|{keyType}".ToLowerInvariant();
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(keyData));
            return Convert.ToBase64String(hash)[..16]; // Prend les 16 premiers caractères
        }
    }
}
