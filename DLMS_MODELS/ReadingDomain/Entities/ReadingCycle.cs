using DLMS_MODELS.Bases;
using DLMS_MODELS.ReadingDomain.Enums;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class ReadingCycle : AuditableEntity
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public ReadingCycleStatus Statut { get; set; }
    public int TotalCompteurs { get; set; }
    public int CompteursLus { get; set; }
    public int MaxSessions { get; set; }
    public int SessionActuelle { get; set; }

    public ICollection<ReadingSession> Sessions { get; set; } = new List<ReadingSession>();
}
