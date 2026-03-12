using DLMS_MODELS.Bases;
using DLMS_MODELS.ReadingDomain.Enums;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class ReadingSession : AuditableEntity
{
    public int Id { get; set; }
    public int ReadingCycleId { get; set; }
    public int NumeroSession { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public ReadingSessionStatus Statut { get; set; }
    public int CompteursEnScope { get; set; }
    public int CompteursLus { get; set; }
    public int CompteursEchoues { get; set; }
    public long DureeMs { get; set; }
    public long DureeLectureMs { get; set; }
    public long DureePauseMs { get; set; }
    public int NombreIps { get; set; }
    public int IpsAccessibles { get; set; }
    public double TauxReussite { get; set; }
    public double DebitCompteursParMinute { get; set; }

    public ReadingCycle ReadingCycle { get; set; } = null!;
    public ICollection<SessionPassResult> Passes { get; set; } = new List<SessionPassResult>();
    public ICollection<MeterReadingStatus> MeterReadings { get; set; } = new List<MeterReadingStatus>();
    public ICollection<IpSessionStats> IpStats { get; set; } = new List<IpSessionStats>();
}
