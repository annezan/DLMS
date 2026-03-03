using DLMS_DAL.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands
{
    public interface ICommandeCompteurCommandRepository : ICommandRepository<CommandeCompteur>
    {
        Task<CommandeCompteur> AddCommandeCompteur(CommandeCompteur CommandeCompteur);
        Task<CommandeCompteur> EditCommandeCompteur(CommandeCompteur CommandeCompteur);
        Task<bool> DeleteCommandeCompteur(CommandeCompteur CommandeCompteur);
    }
}
