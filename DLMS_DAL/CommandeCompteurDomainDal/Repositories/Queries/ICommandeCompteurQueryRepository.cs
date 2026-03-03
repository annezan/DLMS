using DLMS_DAL.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries
{
    public interface ICommandeCompteurQueryRepository : IQueryBaseRepository<CommandeCompteur>
    {
        Task<List<CommandeCompteur>> GetCommandeCompteurs();
        Task<CommandeCompteur> GetCommandeCompteurById(int id);
        Task<IEnumerable<CommandeCompteur>> GetByCommandeId(int commandeId);
        Task<CommandeCompteur> GetByCommandeCompteurId(int commandeCompteurId);

    }
} 