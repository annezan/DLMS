using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_DAL.Bases;

namespace DLMS_DAL.CompteurCelluleDomainDal.Repositories.Commands
{
    public interface ICompteurCelluleCommandRepository : ICommandRepository<CompteurCellule>
    {
        Task<CompteurCellule> GetByIdsAsync(int compteurId, int celluleId);
        Task<List<CompteurCellule>> AddCompteurCelluleList(List<CompteurCellule> compteurCellules);
        Task<List<CompteurCellule>> DeleteCompteurCelluleList(List<CompteurCellule> compteurCellules);
    }
}
