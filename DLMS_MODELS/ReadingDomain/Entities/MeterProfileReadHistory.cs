using DLMS_MODELS.Bases;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class MeterProfileReadHistory : AuditableEntity
{
    public int Id { get; set; }
    public string CompteurSerial { get; set; } = string.Empty;
    public string ProfileObis { get; set; } = string.Empty;
    public DateTime LastReadUpTo { get; set; }
    public DateTime LastReadAt { get; set; }
    public int RowsRead { get; set; }
    public long ReadDurationMs { get; set; }
}
