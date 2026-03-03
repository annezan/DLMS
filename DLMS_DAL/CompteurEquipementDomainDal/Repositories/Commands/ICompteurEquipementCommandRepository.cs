using DLMS_DAL.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;

namespace DLMS_DAL.CompteurEquipementDomainDal.Repositories.Commands
{
    public interface ICompteurEquipementCommandRepository : ICommandRepository<CompteurEquipement>
    {
         Task<List<CompteurEquipement>> AddCompteurEquipement(List<CompteurEquipement> compteurequipement);
         Task<List<CompteurEquipement>> DeleteCompteurEquipement(List<CompteurEquipement> compteurequipement);

    }
}
