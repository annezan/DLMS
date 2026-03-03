using DLMS_DAL.Bases;
using DLMS_MODELS.CompteurDomain.Entities;

namespace DLMS_DAL.CompteurDomainDal.Repositories.Commands
{
    public interface ICompteurCommandRepository : ICommandRepository<Compteur>
    {
         Task<Compteur> AddCompteur(Compteur compteur);
         Task<Compteur> EditCompteur(Compteur compteur);
         bool MAJCompteur(string result, int Id);
        Task<Compteur> DeleteCompteur(Compteur compteur);

    }
}
