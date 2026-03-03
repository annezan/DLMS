using DLMS_MODELS.Bases;

namespace DLMS_MODELS.UsersDomain.Entities;

public class Role : ModelBase
{
    public string Libelle { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}
