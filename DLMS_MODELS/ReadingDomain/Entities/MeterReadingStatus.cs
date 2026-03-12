using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class MeterReadingStatus : AuditableEntity
{
    public int Id { get; set; }
    public int ReadingSessionId { get; set; }
    public int CompteurEquipementId { get; set; }
    public int NumeroPasse { get; set; }
    public MeterReadingResult Resultat { get; set; }
    public string? MessageErreur { get; set; }
    public string? AdresseIp { get; set; }
    public string? Port { get; set; }
    public string? NumeroCompteur { get; set; }
    public long? TempsHdlcMs { get; set; }
    public long? TempsLectureMs { get; set; }
    public long? TempsClesMs { get; set; }
    public long? TempsTotalMs { get; set; }
    public int? TimeoutApplique { get; set; }

    public ReadingSession ReadingSession { get; set; } = null!;
    public CompteurEquipement CompteurEquipement { get; set; } = null!;
}
