using DLMS_DAL.Bases;
using DLMS_MODELS.PosteDomain.Entities;

namespace DLMS_DAL.PosteDomainDal.Repositories.Queries
{
    public interface IPosteQueryRepository : IQueryBaseRepository<Poste>
    {
        Task<List<Poste>> GetPoste();
        Task<Poste> GetPosteById(int Id);
    }
}
