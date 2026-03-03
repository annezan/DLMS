using DLMS_DAL.Bases;
using DLMS_MODELS.CommandeDomain.Entities;

namespace DLMS_DAL.CommandeDomainDal.Repositories.Commands
{
    public interface ICommandeCommandRepository : ICommandRepository<Commande>
    {
        Task<Commande> AddCommande(Commande Commande);
        Task<Commande> EditCommande(Commande Commande);
        Task<bool> DeleteCommande(Commande Commande);
        Task UpdateCommandStatusAsync(int commandId, string status, int retryCount);
        Task SaveCommandResultAsync(int commandId, bool success, string result);
        Task UpdateCommandeCompteurStatusAsync(int commandeCompteurId, string status, int retryCount);
    }
}
