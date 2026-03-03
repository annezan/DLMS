using DLMS_DAL.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands
{
    public interface IResultatCommandeCompteurCommandRepository : ICommandRepository<ResultatCommandeCompteur>
    {
        Task<ResultatCommandeCompteur> AddResultatCommandeCompteur(ResultatCommandeCompteur resultatCommandeCompteur);
        Task<ResultatCommandeCompteur> EditResultatCommandeCompteur(ResultatCommandeCompteur resultatCommandeCompteur);
        Task<bool> DeleteResultatCommandeCompteur(ResultatCommandeCompteur resultatCommandeCompteur);
    }
}
