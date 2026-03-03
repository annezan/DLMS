using DLMS_DAL.Bases;
using DLMS_MODELS.CelluleDomain.Entities;

namespace DLMS_DAL.CelluleDomainDal.Repositories.Commands
{
    public interface ICelluleCommandRepository : ICommandRepository<Cellule>
    {
         Task<Cellule> AddCellule(Cellule cellule);
         Task<Cellule> EditCellule(Cellule cellule);
         Task<Cellule> DeleteCellule(Cellule cellule);

    }
}

