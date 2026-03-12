using DLMS_MODELS.Bases;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class ReadingConfiguration : AuditableEntity
{
    public int Id { get; set; }
    public string Cle { get; set; } = string.Empty;
    public string Valeur { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? GroupeConfig { get; set; }
}
