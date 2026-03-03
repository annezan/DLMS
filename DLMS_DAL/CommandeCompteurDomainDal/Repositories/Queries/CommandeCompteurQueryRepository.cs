using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries
{
    public class CommandeCompteurQueryRepository : QueryBaseRepository<CommandeCompteur>, ICommandeCompteurQueryRepository
    {
        public CommandeCompteurQueryRepository(DLMSDBContext context) : base(context)
        {
        }

        public async Task<List<CommandeCompteur>> GetCommandeCompteurs()
        {
            try
            {
                return await _context.CommandeCompteur
                    .Include(cc => cc.Compteur)
                    .Include(cc => cc.Commande)
                    .Where(x => x.IsArchive == false)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CommandeCompteur> GetCommandeCompteurById(int id)
        {
            try
            {
                return await _context.CommandeCompteur
                    .Include(cc => cc.Compteur)
                    .Include(cc => cc.Commande)
                    .FirstOrDefaultAsync(cc => cc.Id == id && cc.IsArchive == false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CommandeCompteur>> GetByCommandeId(int commandeId)
        {
            try
            {
                return await _context.CommandeCompteur
                    .Include(cc => cc.Compteur)
                    .Include(cc => cc.Commande)
                    .Where(x => x.CommandeId == commandeId && x.IsArchive == false)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CommandeCompteur> GetByCommandeCompteurId(int commandeCompteurId)
        {
            try
            {
                return await _context.CommandeCompteur
                    .Where(x => x.Id == commandeCompteurId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}