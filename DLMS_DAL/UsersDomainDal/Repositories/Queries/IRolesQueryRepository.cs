using DLMS_DAL.Bases;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public interface IRolesQueryRepository : IQueryBaseRepository<Role>
    {
        Task<Role?> GetRoleByCode(string code);
        
        /// <summary>
        /// Récupère tous les rôles avec leurs permissions
        /// </summary>
        Task<List<Role>> GetAllWithPermissionsAsync();
        
        /// <summary>
        /// Récupère un rôle par ID avec ses permissions
        /// </summary>
        Task<Role?> GetByIdWithPermissionsAsync(Guid id);
    }
}
