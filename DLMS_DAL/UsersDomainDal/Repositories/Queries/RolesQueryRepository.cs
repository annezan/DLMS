using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public class RolesQueryRepository : QueryBaseRepository<Role>, IRolesQueryRepository
    {

        public RolesQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<Role?> GetRoleByCode(string code)
        {
            return await _context.Roles
                .Where(x => x.DeletedAt == null && x.Code == code)
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permissions)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Récupère tous les rôles avec leurs permissions
        /// </summary>
        public async Task<List<Role>> GetAllWithPermissionsAsync()
        {
            return await _context.Roles
                .Where(r => r.DeletedAt == null)
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permissions)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un rôle par ID avec ses permissions
        /// </summary>
        public async Task<Role?> GetByIdWithPermissionsAsync(Guid id)
        {
            return await _context.Roles
                .Where(r => r.Id == id && r.DeletedAt == null)
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permissions)
                .FirstOrDefaultAsync();
        }
    }
}
