using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CommandeDomainDal.Repositories
{
    public class CommandeCommandRepository : CommandRepository<Commande>, ICommandeCommandRepository
    {
        public CommandeCommandRepository(DLMSDBContext context)
            : base(context)
        {
        }

        public async Task<Commande> AddCommande(Commande Commande)
        {
            try
            {
                var commande = _context.Commandes.AsNoTracking().FirstOrDefault(x => x.Id == Commande.Id && x.IsArchive == false);
                if (commande == null)
                {
                    _context.Commandes.Add(Commande);
                    await _context.SaveChangesAsync();
                    return Commande;
                }
                return null;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        public async Task<Commande> EditCommande(Commande Commande)
        {
            try
            {
                var Commande_update = _context.Commandes.AsNoTracking().FirstOrDefault(x => x.Id == Commande.Id && x.IsArchive == false);
                if (Commande_update != null)
                {
                    Commande_update.Libellecommande = Commande_update.Libellecommande == Commande.Libellecommande ? Commande_update.Libellecommande : Commande.Libellecommande;
                    Commande_update.Dateexec = Commande_update.Dateexec == Commande.Dateexec ? Commande_update.Dateexec : Commande.Dateexec;
                    Commande_update.Datefin = Commande_update.Datefin == Commande.Datefin ? Commande_update.Datefin : Commande.Datefin;
                    Commande_update.Numeroprofile = Commande_update.Numeroprofile == Commande.Numeroprofile ? Commande_update.Numeroprofile : Commande.Numeroprofile;
                    Commande_update.Nombreentree = Commande_update.Nombreentree == Commande.Nombreentree ? Commande_update.Nombreentree : Commande.Nombreentree;
                    Commande_update.Datedebut = Commande_update.Datedebut == Commande.Datedebut ? Commande_update.Datedebut : Commande.Datedebut;
                    Commande_update.Dateexp = Commande_update.Dateexp == Commande.Dateexp ? Commande_update.Dateexp : Commande.Dateexp;
                    Commande_update.TypecommandeId = Commande_update.TypecommandeId == Commande.TypecommandeId ? Commande_update.TypecommandeId : Commande.TypecommandeId;
                    Commande_update.Statut = Commande_update.Statut;
                    Commande_update.UpdatedBy = Commande_update.UpdatedBy == Commande.UpdatedBy ? Commande_update.UpdatedBy : Commande.UpdatedBy;
                    Commande_update.UpdatedAt = DateTime.Now;

                    _context.Commandes.Update(Commande_update);
                    await _context.SaveChangesAsync();
                    return Commande_update;
                }
                return null;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        public async Task<bool> DeleteCommande(Commande Commande)
        {
            try
            {
                var Commande_update = _context.Commandes.AsNoTracking().FirstOrDefault(x => x.Id == Commande.Id && x.IsArchive == false);
                if (Commande_update != null)
                {
                    Commande_update.Statut = "Terminé";
                    Commande_update.DeletedBy = Commande.DeletedBy;
                    Commande_update.DeletedAt = DateTime.Now;
                    Commande_update.IsArchive = true;

                    _context.Commandes.Update(Commande_update);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public async Task UpdateCommandStatusAsync(int commandId, string status, int retryCount)
        {
            try
            {
                var command = await _context.Commandes
                    .FirstOrDefaultAsync(c => c.Id == commandId && !c.IsArchive);
                
                if (command != null)
                {
                    command.Statut = status;
                    command.UpdatedAt = DateTime.Now;
                    _context.Commandes.Update(command);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du statut de la commande {commandId}: {ex.Message}", ex);
            }
        }

        public async Task SaveCommandResultAsync(int commandId, bool success, string result)
        {
            try
            {
                // Implémentation à adapter selon votre modèle de données
                // Par exemple, sauvegarder dans une table CommandResult
                await Task.Delay(5); // Placeholder
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la sauvegarde du résultat de la commande {commandId}: {ex.Message}", ex);
            }
        }

        public async Task UpdateCommandeCompteurStatusAsync(int commandeCompteurId, string status, int retryCount)
        {
            try
            {
                var commandeCompteur = await _context.CommandeCompteur
                    .FirstOrDefaultAsync(cc => cc.Id == commandeCompteurId && !cc.IsArchive);
                
                if (commandeCompteur != null)
                {
                    commandeCompteur.NumeroTentative = retryCount;
                    commandeCompteur.UpdatedAt = DateTime.Now;
                    _context.CommandeCompteur.Update(commandeCompteur);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du statut du CommandeCompteur {commandeCompteurId}: {ex.Message}", ex);
            }
        }
    }
}
