using DLMS_DAL.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries
{
    public interface IResultatCommandeCompteurQueryRepository : IQueryBaseRepository<ResultatCommandeCompteur>
    {
        Task<List<ResultatCommandeCompteur>> GetResultatCommandeCompteurs();
        Task<ResultatCommandeCompteur> GetResultatCommandeCompteurById(int id);
        Task<List<ResultatCommandeCompteur>> GetResultatsByCommandeCompteurId(int commandeCompteurId);
    }
}
