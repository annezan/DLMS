using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries
{
    public class ResultatCommandeCompteurQueryRepository : QueryBaseRepository<ResultatCommandeCompteur>, IResultatCommandeCompteurQueryRepository
    {
        public ResultatCommandeCompteurQueryRepository(DLMSDBContext context) : base(context)
        {
        }

        public async Task<List<ResultatCommandeCompteur>> GetResultatCommandeCompteurs()
        {
            try
            {
                return await _context.ResultatCommandeCompteurs
                    .Include(r => r.CommandeCompteur)
                    .Include(r => r.CodeObis)
                    .Include(r => r.Gxdlmsprofilgeneric)
                    .Where(x => x.IsArchive == false)
                    .OrderByDescending(x => x.DateEnr)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResultatCommandeCompteur> GetResultatCommandeCompteurById(int id)
        {
            try
            {
                return await _context.ResultatCommandeCompteurs
                    .Include(r => r.CommandeCompteur)
                    .Include(r => r.CodeObis)
                    .Include(r => r.Gxdlmsprofilgeneric)
                    .FirstOrDefaultAsync(r => r.Id == id && r.IsArchive == false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ResultatCommandeCompteur>> GetResultatsByCommandeCompteurId(int commandeCompteurId)
        {
            try
            {
                return await _context.ResultatCommandeCompteurs
                    .Include(r => r.CodeObis)
                    .Include(r => r.Gxdlmsprofilgeneric)
                    .Where(x => x.CommandeCompteurId == commandeCompteurId && x.IsArchive == false)
                    .OrderByDescending(x => x.DateEnr)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
