using DLMS_MODELS.Bases;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class IpSessionStats : AuditableEntity
{
    public int Id { get; set; }
    public int ReadingSessionId { get; set; }
    public string AdresseIp { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public int TotalTentatives { get; set; }
    public int Reussites { get; set; }
    public int Echecs { get; set; }
    public double TauxReussite { get; set; }
    public double LatenceMoyenneMs { get; set; }
    public bool TcpAccessible { get; set; }
    public long TcpLatenceMs { get; set; }
    public bool CanaryReussi { get; set; }
    public int EchecsConsecutifsMax { get; set; }

    public ReadingSession ReadingSession { get; set; } = null!;
}
