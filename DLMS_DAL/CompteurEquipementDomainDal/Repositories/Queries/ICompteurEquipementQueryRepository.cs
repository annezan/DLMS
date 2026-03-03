using DLMS_DAL.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;

namespace DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries
{
    public interface ICompteurEquipementQueryRepository : IQueryBaseRepository<CompteurEquipement>
    {
        Task<List<CompteurEquipement>> GetCompteurEquipement();
        Task<List<CompteurEquipement>> GetCompteurEquipementByIdEquipement(int Id);
        Task<List<CompteurEquipement>> GetCompteurEquipementByIdCompteur(int Id);
        Task<int> GetUniqueIpCountAsync();
    }
}
