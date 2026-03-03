using DLMS_DAL.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeDomain.Entities;

namespace DLMS_DAL.CommandeDomainDal.Repositories.Queries
{
    public interface ICommandeQueryRepository : IQueryBaseRepository<Commande>
    {
        Task<Commande> GetCommandeById(int id);
        Task<List<Commande>> GetCommandes();
        Task<List<Commande>> GetCommandesByCompteur(string numeroCompteur);
        Task<List<Commande>> GetActiveCommandsAsync();
        Task<List<Commande>> GetFailedCommandsForRetryAsync(int maxRetryCount);
        Task<List<CommandeCompteur>> GetCommandeCompteursByCommandIdAsync(int commandId);
    }
}
