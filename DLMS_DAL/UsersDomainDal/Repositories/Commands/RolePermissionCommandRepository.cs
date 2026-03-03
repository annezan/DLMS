using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.UsersDomainDal.Repositories.Commands
{
    public class RolePermissionCommandRepository : CommandRepository<RolePermission>, IRolePermissionCommandRepository
    {
        private readonly DLMSDBContext _context;

        public RolePermissionCommandRepository(DLMSDBContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Ajoute plusieurs associations rôle-permission en une seule fois
        /// </summary>
        public async Task AddRangeAsync(List<RolePermission> rolePermissions)
        {
            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime toutes les associations entre un rôle et une liste de permissions
        /// </summary>
        public async Task DeleteRangeByRoleAndPermissionsAsync(Guid roleId, List<Guid> permissionIds)
        {
            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();

            if (rolePermissions.Any())
            {
                _context.RolePermissions.RemoveRange(rolePermissions);
                await _context.SaveChangesAsync();
            }
        }
    }
}

