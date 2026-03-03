using DLMS_MODELS.Bases;

namespace DLMS_MODELS.UsersDomain.Entities;

public class Permission : ModelBase
{
    public string Libelle { get; set; }
    public string Description { get; set; }

    // Exemple : "GET:/api/users", "POST:/api/orders"
    public string Action { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; }
}
