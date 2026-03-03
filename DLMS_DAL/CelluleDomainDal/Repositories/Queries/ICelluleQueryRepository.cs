using DLMS_DAL.Bases;
using DLMS_MODELS.CelluleDomain.Entities;

namespace DLMS_DAL.CelluleDomainDal.Repositories.Queries
{
    public interface ICelluleQueryRepository : IQueryBaseRepository<Cellule>
    {
        Task<List<Cellule>> GetCellule();
        Task<Cellule> GetCelluleById(int id);
        Task<List<Cellule>> GetCelluleByPosteId(int posteId);
    }
}

