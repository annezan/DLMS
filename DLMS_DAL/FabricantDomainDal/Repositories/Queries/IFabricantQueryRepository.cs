using DLMS_DAL.Bases;
using DLMS_MODELS.FabricantDomain.Entities;

namespace DLMS_DAL.FabricantsDomainDal.Repositories.Queries
{
    public interface IFabricantQueryRepository : IQueryBaseRepository<Fabricant>
    {
        Task<List<Fabricant>> GetFabricant();
        Task<Fabricant> GetFabricantById(int Id);
    }
}
