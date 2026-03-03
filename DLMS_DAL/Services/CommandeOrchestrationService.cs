using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;

namespace DLMS_DAL.Services
{
    public interface ICommandeOrchestrationService
    {
        Task<Commande> AddCommandeWithCompteurs(Commande commande, List<int> compteurIds);
        Task<Commande> EditCommandeWithCompteurs(Commande commande, List<int> compteurIds);
        Task<bool> DeleteCommandeWithCompteurs(Commande commande);
        Task<bool> DeleteCommandeCompteurWithOrphanCheck(CommandeCompteur commandeCompteur);
    }

    public class CommandeOrchestrationService : ICommandeOrchestrationService
    {
        private readonly ICommandeCommandRepository _commandeRepo;
        private readonly ICommandeCompteurCommandRepository _commandeCompteurRepo;
        private readonly ICommandeCompteurQueryRepository _commandeCompteurQueryRepo;


        public CommandeOrchestrationService(
            ICommandeCommandRepository commandeRepo,
            ICommandeCompteurCommandRepository commandeCompteurRepo,
            ICommandeCompteurQueryRepository commandeCompteurQueryRepo)
        {
            _commandeRepo = commandeRepo;
            _commandeCompteurRepo = commandeCompteurRepo;
            _commandeCompteurQueryRepo = commandeCompteurQueryRepo;
        }

        public async Task<Commande> AddCommandeWithCompteurs(Commande commande, List<int> compteurIds)
        {
            // Ajouter d'abord la commande
            var addedCommande = await _commandeRepo.AddCommande(commande);
            
            if (addedCommande != null)
            {
                // Puis ajouter les associations CommandeCompteur
                foreach (var compteurId in compteurIds)
                {
                    var commandeCompteur = new CommandeCompteur
                    {
                        CommandeId = addedCommande.Id,
                        CompteurId = compteurId,
                        Libellegroupe = addedCommande.Libellecommande,
                        CreatedBy = addedCommande.CreatedBy
                    };
                    await _commandeCompteurRepo.AddCommandeCompteur(commandeCompteur);
                }
            }

            return addedCommande;
        }

        public async Task<Commande> EditCommandeWithCompteurs(Commande commande, List<int> compteurIds)
        {
            // Mettre à jour d'abord la commande
            var updatedCommande = await _commandeRepo.EditCommande(commande);
            
            if (updatedCommande != null)
            {
                // Supprimer les anciennes associations
                var deleteCommandeCompteur = new CommandeCompteur
                {
                    CommandeId = commande.Id,
                    Libellegroupe = commande.Libellecommande
                };
                await _commandeCompteurRepo.DeleteCommandeCompteur(deleteCommandeCompteur);

                // Ajouter les nouvelles associations
                foreach (var compteurId in compteurIds)
                {
                    var commandeCompteur = new CommandeCompteur
                    {
                        CommandeId = commande.Id,
                        CompteurId = compteurId,
                        Libellegroupe = commande.Libellecommande,
                        UpdatedBy = commande.UpdatedBy
                    };
                    await _commandeCompteurRepo.AddCommandeCompteur(commandeCompteur);
                }
            }

            return updatedCommande;
        }

        public async Task<bool> DeleteCommandeWithCompteurs(Commande commande)
        {
            // Supprimer d'abord la commande
            var success = await _commandeRepo.DeleteCommande(commande);
            
            if (success)
            {
                // Puis supprimer les associations CommandeCompteur
                var commandeCompteur = new CommandeCompteur
                {
                    CommandeId = commande.Id,
                    DeletedBy = commande.DeletedBy
                };
                await _commandeCompteurRepo.DeleteCommandeCompteur(commandeCompteur);
            }

            return success;
        }

        public async Task<bool> DeleteCommandeCompteurWithOrphanCheck(CommandeCompteur commandeCompteur)
        {
            var success = await _commandeCompteurRepo.DeleteCommandeCompteur(commandeCompteur);
            
            if (success)
            {
                var commandeCompteurAll = await _commandeCompteurQueryRepo.GetByCommandeCompteurId(commandeCompteur.Id);

                // Vérifier s'il reste des CommandeCompteur pour cette Commande en utilisant le QueryRepository
                var remainingCommandeCompteurs = await _commandeCompteurQueryRepo.GetByCommandeId(commandeCompteurAll.CommandeId);
                if (remainingCommandeCompteurs == null || !remainingCommandeCompteurs.Any())
                {

                    // Si plus aucun CommandeCompteur, supprimer la Commande
                    var commande = new Commande
                    {
                        Id = commandeCompteurAll.CommandeId,
                        DeletedBy = commandeCompteur.DeletedBy
                    };
                    await _commandeRepo.DeleteCommande(commande);
                }
            }

            return success;
        }
    }
} 