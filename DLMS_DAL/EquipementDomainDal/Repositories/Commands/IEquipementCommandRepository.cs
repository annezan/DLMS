using DLMS_DAL.Bases;
using DLMS_MODELS.EquipementDomain.Entities;

namespace DLMS_DAL.EquipementDomainDal.Repositories.Commands
{
    public interface IEquipementCommandRepository : ICommandRepository<Equipement>
    {
         Task<Equipement> AddEquipement(Equipement Equipement);
         Task<Equipement> EditEquipement(Equipement Equipement);
         Task<Equipement> DeleteEquipement(Equipement Equipement);

    }
}
