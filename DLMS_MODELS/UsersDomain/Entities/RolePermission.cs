using DLMS_MODELS.Bases;

namespace DLMS_MODELS.UsersDomain.Entities;

public class RolePermission : ModelBase
{
    public Guid RoleId { get; set; }
    public Role Roles { get; set; }

    public Guid PermissionId { get; set; }
    public Permission Permissions { get; set; }
}
