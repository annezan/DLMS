using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands
{
    public class ResultatCommandeCompteurCommandRepository : CommandRepository<ResultatCommandeCompteur>, IResultatCommandeCompteurCommandRepository
    {
        public ResultatCommandeCompteurCommandRepository(DLMSDBContext context)
            : base(context)
        {
        }

        public async Task<ResultatCommandeCompteur> AddResultatCommandeCompteur(ResultatCommandeCompteur resultatCommandeCompteur)
        {
            try
            {
                resultatCommandeCompteur.CreatedAt = DateTime.Now;
                _context.ResultatCommandeCompteurs.Add(resultatCommandeCompteur);
                await _context.SaveChangesAsync();
                return resultatCommandeCompteur;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResultatCommandeCompteur> EditResultatCommandeCompteur(ResultatCommandeCompteur resultatCommandeCompteur)
        {
            try
            {
                var resultat_update = await _context.ResultatCommandeCompteurs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == resultatCommandeCompteur.Id && x.IsArchive == false);
                
                if (resultat_update != null)
                {
                    resultat_update.Value = resultatCommandeCompteur.Value;
                    resultat_update.DateEnr = resultatCommandeCompteur.DateEnr;
                    resultat_update.IsArchive = resultatCommandeCompteur.IsArchive;
                    resultat_update.UpdatedBy = "Admin";
                    resultat_update.UpdatedAt = DateTime.Now;
                    
                    _context.ResultatCommandeCompteurs.Update(resultat_update);
                    await _context.SaveChangesAsync();
                }
                return resultat_update;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteResultatCommandeCompteur(ResultatCommandeCompteur resultatCommandeCompteur)
        {
            try
            {
                var resultat_update = await _context.ResultatCommandeCompteurs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == resultatCommandeCompteur.Id && x.IsArchive == false);
                
                if (resultat_update != null)
                {
                    resultat_update.DeletedBy = resultatCommandeCompteur.DeletedBy;
                    resultat_update.DeletedAt = DateTime.Now;
                    resultat_update.IsArchive = true;
                    _context.ResultatCommandeCompteurs.Update(resultat_update);
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
    }
}
