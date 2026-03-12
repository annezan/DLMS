using DLMS_MODELS.Bases;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class SessionPassResult : AuditableEntity
{
    public int Id { get; set; }
    public int ReadingSessionId { get; set; }
    public int NumeroPasse { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public int BudgetSeconds { get; set; }
    public int CompteursEnScope { get; set; }
    public int CompteursLus { get; set; }
    public int CompteursEchoues { get; set; }
    public int CompteursDifferes { get; set; }
    public int IpsDifferees { get; set; }
    public long DureeMs { get; set; }

    // Config snapshot
    public int CanaryTimeoutSeconds { get; set; }
    public int CachedTimeoutSeconds { get; set; }
    public int UncachedTimeoutSeconds { get; set; }
    public int MaxConsecutiveFailures { get; set; }
    public int CooldownCount { get; set; }
    public int CooldownSeconds { get; set; }

    public ReadingSession ReadingSession { get; set; } = null!;
}
