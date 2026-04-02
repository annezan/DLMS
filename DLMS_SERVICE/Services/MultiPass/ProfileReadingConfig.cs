using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services.MultiPass;

public class ProfileReadingEntry
{
    public int Priority { get; set; }
    public string ProfileObis { get; set; } = "";
    public int TimeoutSeconds { get; set; }
    public int FallbackMaxHours { get; set; }
}

public interface IProfileReadingConfig
{
    Task<List<ProfileReadingEntry>> GetOrderedProfilesAsync();
}

public class ProfileReadingConfig : IProfileReadingConfig
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProfileReadingConfig> _logger;
    private readonly int _fallbackMaxHoursOverride;
    private readonly int _maxProfilesToRead;
    private List<ProfileReadingEntry>? _cached;

    private static readonly List<ProfileReadingEntry> Defaults = new()
    {
        new() { Priority = 1, ProfileObis = "0.0.98.1.0.255", TimeoutSeconds = 90, FallbackMaxHours = 720 },
        new() { Priority = 2, ProfileObis = "1.0.99.3.0.255", TimeoutSeconds = 90, FallbackMaxHours = 24 },
        new() { Priority = 3, ProfileObis = "1.0.99.1.0.255", TimeoutSeconds = 60, FallbackMaxHours = 24 },
        new() { Priority = 4, ProfileObis = "1.0.99.2.0.255", TimeoutSeconds = 120, FallbackMaxHours = 12 },
        new() { Priority = 5, ProfileObis = "0.0.99.98.0.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 6, ProfileObis = "0.0.99.98.1.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 7, ProfileObis = "0.0.99.98.2.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 8, ProfileObis = "0.0.99.98.3.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 9, ProfileObis = "0.0.99.98.4.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 10, ProfileObis = "0.0.99.98.5.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 11, ProfileObis = "0.0.99.98.6.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
        new() { Priority = 12, ProfileObis = "0.0.99.98.7.255", TimeoutSeconds = 20, FallbackMaxHours = 24 },
    };

    public ProfileReadingConfig(IServiceProvider serviceProvider, ILogger<ProfileReadingConfig> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _fallbackMaxHoursOverride = configuration.GetValue<int>("MultiPass:ProfileFallbackMaxHours", 0);
        _maxProfilesToRead = configuration.GetValue<int>("MultiPass:MaxProfilesToRead", 8);
        if (_fallbackMaxHoursOverride > 0)
        {
            _logger.LogInformation("ProfileFallbackMaxHours override depuis appsettings: {Hours}h", _fallbackMaxHoursOverride);
        }
        if (_maxProfilesToRead != 8)
        {
            _logger.LogInformation("MaxProfilesToRead override depuis appsettings: {Max}", _maxProfilesToRead);
        }
    }

    public async Task<List<ProfileReadingEntry>> GetOrderedProfilesAsync()
    {
        if (_cached != null) return _cached;

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

            var configs = await context.ReadingConfigurations
                .AsNoTracking()
                .Where(c => c.GroupeConfig == "ProfileReading" && !c.IsArchive)
                .ToListAsync();

            if (configs.Count == 0)
            {
                _logger.LogWarning("No ProfileReading config found in DB, using defaults");
                _cached = ApplyFallbackOverride(Defaults.Take(_maxProfilesToRead).ToList());
                return _cached;
            }

            var result = new List<ProfileReadingEntry>();

            // Format DB : profile.N.obis / profile.N.priority / profile.N.timeout_seconds / profile.N.fallback_hours
            var obisConfigs = configs
                .Where(c => c.Cle.EndsWith(".obis"))
                .OrderBy(c => c.Cle)
                .ToList();

            foreach (var oc in obisConfigs)
            {
                // profile.3.obis → prefix = "profile.3"
                var prefix = oc.Cle[..oc.Cle.LastIndexOf('.')];
                var obis = oc.Valeur;

                var priorityConfig = configs.FirstOrDefault(c => c.Cle == $"{prefix}.priority");
                var timeoutConfig = configs.FirstOrDefault(c => c.Cle == $"{prefix}.timeout_seconds");
                var fallbackConfig = configs.FirstOrDefault(c => c.Cle == $"{prefix}.fallback_hours");

                var priorityNum = priorityConfig != null ? int.Parse(priorityConfig.Valeur) : 99;

                result.Add(new ProfileReadingEntry
                {
                    Priority = priorityNum,
                    ProfileObis = obis,
                    TimeoutSeconds = timeoutConfig != null ? int.Parse(timeoutConfig.Valeur) : 30,
                    FallbackMaxHours = fallbackConfig != null ? int.Parse(fallbackConfig.Valeur) : 48,
                });
            }

            if (result.Count == 0)
            {
                _logger.LogWarning("ProfileReading config en base sans profile.N.obis — fallback aux defaults ({Count} profils)", Defaults.Count);
                _cached = ApplyFallbackOverride(Defaults.Take(_maxProfilesToRead).ToList());
                return _cached;
            }

            _cached = ApplyFallbackOverride(result.OrderBy(r => r.Priority).Take(_maxProfilesToRead).ToList());
            return _cached;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ProfileReading config, using defaults");
            _cached = ApplyFallbackOverride(Defaults.Take(_maxProfilesToRead).ToList());
            return _cached;
        }
    }

    private List<ProfileReadingEntry> ApplyFallbackOverride(List<ProfileReadingEntry> entries)
    {
        if (_fallbackMaxHoursOverride <= 0) return entries;
        foreach (var e in entries)
            e.FallbackMaxHours = _fallbackMaxHoursOverride;
        return entries;
    }
}
