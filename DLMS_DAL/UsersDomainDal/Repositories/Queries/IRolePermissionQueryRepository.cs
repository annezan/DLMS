using DLMS_DAL.Bases;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public interface IRolePermissionQueryRepository : IQueryBaseRepository<RolePermission>
    {
        /// <summary>
        /// Récupère toutes les permissions d'un rôle avec leurs détails
        /// </summary>
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Vérifie si une association rôle-permission existe déjà
        /// </summary>
        Task<bool> ExistsAsync(Guid roleId, Guid permissionId);
    }
}

