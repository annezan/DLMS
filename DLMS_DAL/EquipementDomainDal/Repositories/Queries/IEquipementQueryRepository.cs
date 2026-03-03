using DLMS_DAL.Bases;
using DLMS_MODELS.EquipementDomain.Entities;

namespace DLMS_DAL.EquipementDomainDal.Repositories.Queries
{
    public interface IEquipementQueryRepository : IQueryBaseRepository<Equipement>
    {
        Task<List<Equipement>> GetEquipement();
        Task<Equipement> GetEquipementById(int Id);
        Task<List<Equipement>> GetEquipementByCelluleId(int celluleId);
    }
}
