namespace DLMS_SERVICE.Services.MultiPass;

public class MultiPassConfig
{
    public int GlobalCeilingSeconds { get; set; } = 3600;
    public int MaxConcurrentIps { get; set; } = 8;
    public double AdaptiveTimeoutMultiplier { get; set; } = 2.5;
    public int MinAdaptiveTimeoutSeconds { get; set; } = 90;
    public int PacingDelayMs { get; set; } = 100;
    public int TcpScanTimeoutSeconds { get; set; } = 8;
    public List<PassConfig> Passes { get; set; } = new();
}

public class PassConfig
{
    public int PassNumber { get; set; }
    public int BudgetSeconds { get; set; }
    public int CanaryTimeoutSeconds { get; set; }
    public int CachedTimeoutSeconds { get; set; }
    public int UncachedTimeoutSeconds { get; set; }
    public int MaxConsecutiveFailures { get; set; }
    public int CooldownCount { get; set; }
    public int CooldownSeconds { get; set; }
    public int PauseAfterSeconds { get; set; }
}
