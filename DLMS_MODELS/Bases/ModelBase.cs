namespace DLMS_MODELS.Bases
{
    public abstract class ModelBase : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
