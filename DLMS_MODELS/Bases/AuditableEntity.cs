namespace DLMS_MODELS.Bases
{
    public class AuditableEntity
    {
        public DateTime? CreatedAt { get; set; } = null;

        public DateTime? UpdatedAt { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;

        public string CreatedBy { get; set; } = String.Empty;

        public string UpdatedBy { get; set; } = String.Empty;

        public string DeletedBy { get; set; } = String.Empty;
        public bool IsArchive { get; set; }=false;

    }
}
