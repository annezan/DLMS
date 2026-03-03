using DLMS_DAL.Bases;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Commands
{
    public interface IRolePermissionCommandRepository : ICommandRepository<RolePermission>
    {
        /// <summary>
        /// Ajoute plusieurs associations rôle-permission en une seule fois
        /// </summary>
        Task AddRangeAsync(List<RolePermission> rolePermissions);

        /// <summary>
        /// Supprime toutes les associations entre un rôle et une liste de permissions
        /// </summary>
        Task DeleteRangeByRoleAndPermissionsAsync(Guid roleId, List<Guid> permissionIds);
    }
}

