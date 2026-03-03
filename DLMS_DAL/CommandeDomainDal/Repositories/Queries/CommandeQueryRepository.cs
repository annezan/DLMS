using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CommandeDomainDal.Repositories.Queries
{
    public class CommandeQueryRepository : QueryBaseRepository<Commande>, ICommandeQueryRepository
    {

        public CommandeQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<Commande> GetCommandeById(int id)
        {
            try
            {
                var commande = await _context.Commandes.Where(x => x.Id == id && x.IsArchive== false).Include(x => x.Typecommande).Include(x => x.CommandeCompteur).ThenInclude(x => x.Compteur).FirstOrDefaultAsync();
                return commande;

            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        
        public async Task<List<Commande>> GetCommandes()
        {
            try
            {
                var Commandes= await _context.Commandes.Where(x=>x.IsArchive==false).Include(x => x.Typecommande).ToListAsync();
                return Commandes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Commande>> GetCommandesByCompteur(string numeroCompteur)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numeroCompteur))
                    return new List<Commande>();

                numeroCompteur = numeroCompteur.Trim();
                var commandes = await _context.Commandes
                    .Where(c => c.IsArchive == false && 
                               c.CommandeCompteur.Any(cc => cc.IsArchive == false && 
                                                             cc.Compteur.NumeroCompteur == numeroCompteur))
                    .Include(c => c.CommandeCompteur)
                        .ThenInclude(cc => cc.Compteur)
                    .ToListAsync();
                return commandes;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<Commande>> GetActiveCommandsAsync()
        {
            try
            {
                var now = DateTime.Now;
                var commandes= await _context.Commandes
                    .Where(c => c.Statut == "En cours" && 
                               c.IsArchive==false && 
                               (c.Dateexec == null || c.Dateexec <= now) &&
                               (c.Dateexp == null || c.Dateexp >= now))
                    .Include(c => c.CommandeCompteur)
                        .ThenInclude(cc => cc.Compteur)
                            .ThenInclude(c => c.CompteurEquipement)
                                .ThenInclude(ce => ce.Equipement)
                    .Include(c => c.Typecommande)
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();
                return commandes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des commandes actives: {ex.Message}", ex);
            }
        }

        public async Task<List<Commande>> GetFailedCommandsForRetryAsync(int maxRetryCount)
        {
            try
            {
                return await _context.Commandes
                    .Where(c => c.Statut == "Failed" && !c.IsArchive && 
                        c.CommandeCompteur.Any(cc => !cc.IsArchive && (cc.NumeroTentative) < maxRetryCount))
                    .Include(c => c.CommandeCompteur)
                        .ThenInclude(cc => cc.Compteur)
                            .ThenInclude(c => c.CompteurEquipement)
                                .ThenInclude(ce => ce.Equipement)
                    .Include(c => c.Typecommande)
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des commandes failed pour retry: {ex.Message}", ex);
            }
        }

        public async Task<List<CommandeCompteur>> GetCommandeCompteursByCommandIdAsync(int commandId)
        {
            try
            {
                return await _context.CommandeCompteur
                    .Where(cc => cc.CommandeId == commandId && !cc.IsArchive)
                    .Include(cc => cc.Compteur)
                        .ThenInclude(c => c.CompteurEquipement)
                            .ThenInclude(ce => ce.Equipement)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des CommandeCompteurs pour la commande {commandId}: {ex.Message}", ex);
            }
        }

    }
}
