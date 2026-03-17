using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
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
    private List<ProfileReadingEntry>? _cached;

    private static readonly List<ProfileReadingEntry> Defaults = new()
    {
        new() { Priority = 1, ProfileObis = "1.0.99.3.0.255", TimeoutSeconds = 30, FallbackMaxHours = 48 },
        new() { Priority = 2, ProfileObis = "1.0.99.1.0.255", TimeoutSeconds = 60, FallbackMaxHours = 48 },
        new() { Priority = 3, ProfileObis = "1.0.99.2.0.255", TimeoutSeconds = 120, FallbackMaxHours = 12 },
        new() { Priority = 4, ProfileObis = "0.0.98.1.0.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 5, ProfileObis = "0.0.99.98.0.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 6, ProfileObis = "0.0.99.98.1.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 7, ProfileObis = "0.0.99.98.2.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 8, ProfileObis = "0.0.99.98.3.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 9, ProfileObis = "0.0.99.98.4.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 10, ProfileObis = "0.0.99.98.5.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 11, ProfileObis = "0.0.99.98.6.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 12, ProfileObis = "0.0.99.98.7.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
    };

    public ProfileReadingConfig(IServiceProvider serviceProvider, ILogger<ProfileReadingConfig> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
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
                _cached = Defaults;
                return _cached;
            }

            var result = new List<ProfileReadingEntry>();
            var priorityConfigs = configs.Where(c => c.Cle.StartsWith("ProfilePriority_")).OrderBy(c => c.Cle);

            foreach (var pc in priorityConfigs)
            {
                var obis = pc.Valeur;
                var priorityNum = int.Parse(pc.Cle.Replace("ProfilePriority_", ""));

                var timeoutConfig = configs.FirstOrDefault(c => c.Cle == $"ProfileTimeout_{obis}");
                var fallbackConfig = configs.FirstOrDefault(c => c.Cle == $"ProfileFallback_{obis}");

                result.Add(new ProfileReadingEntry
                {
                    Priority = priorityNum,
                    ProfileObis = obis,
                    TimeoutSeconds = timeoutConfig != null ? int.Parse(timeoutConfig.Valeur) : 30,
                    FallbackMaxHours = fallbackConfig != null ? int.Parse(fallbackConfig.Valeur) : 48,
                });
            }

            _cached = result.OrderBy(r => r.Priority).ToList();
            return _cached;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ProfileReading config, using defaults");
            _cached = Defaults;
            return _cached;
        }
    }
}
