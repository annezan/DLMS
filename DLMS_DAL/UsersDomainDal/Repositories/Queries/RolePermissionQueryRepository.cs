using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public class RolePermissionQueryRepository : QueryBaseRepository<RolePermission>, IRolePermissionQueryRepository
    {
        private readonly DLMSDBContext _context;

        public RolePermissionQueryRepository(DLMSDBContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère toutes les permissions d'un rôle avec leurs détails
        /// </summary>
        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.DeletedAt == null)
                .Include(rp => rp.Permissions)
                .Select(rp => rp.Permissions)
                .ToListAsync();
        }

        /// <summary>
        /// Vérifie si une association rôle-permission existe déjà
        /// </summary>
        public async Task<bool> ExistsAsync(Guid roleId, Guid permissionId)
        {
            return await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && 
                               rp.PermissionId == permissionId && 
                               rp.DeletedAt == null);
        }
    }
}

