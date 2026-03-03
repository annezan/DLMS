using DLMS_DAL.Bases;
using DLMS_MODELS.TypecommandeDomain.Entities;

namespace DLMS_DAL.TypecommandeDomainDal.Repositories.Queries
{
    public interface ITypecommandeQueryRepository : IQueryBaseRepository<Typecommande>
    {
        Task<List<Typecommande?>> GetTypecommande();

    }
}
