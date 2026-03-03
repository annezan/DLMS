using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_DAL.Bases;

namespace DLMS_DAL.CompteurCelluleDomainDal.Repositories.Queries
{
    public interface ICompteurCelluleQueryRepository : IQueryBaseRepository<CompteurCellule>
    {
        Task<List<CompteurCellule>> GetByIdCompteurAsync(int compteurId);
        Task<List<CompteurCellule>> GetByIdCelluleAsync(int celluleId);
    }
}
