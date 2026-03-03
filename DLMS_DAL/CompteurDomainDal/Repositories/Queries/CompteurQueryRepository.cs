using DLMS_DAL.Bases;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CompteurDomainDal.Repositories.Queries
{
    public class CompteurQueryRepository : QueryBaseRepository<Compteur>, ICompteurQueryRepository
    {

        public CompteurQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<List<Compteur>> GetCompteur()
        {
            try
            {

                var Compteur = await _context.Compteur
                    .Where(x => x.IsArchive == false)
                    .Include(c => c.CompteurEquipement)
                        .ThenInclude(ce => ce.Equipement)
                    .Include(c => c.CompteurCellules)
                        .ThenInclude(cc => cc.Cellule)
                    .ToListAsync();
                return Compteur;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<Compteur> GetCompteurById(int Id)
        {
            try
            {
                if (_context.Compteur == null)
                {
                    throw new Exception("Compteur est null"); // Ou gérez cette situation différemment.
                }
                var compteur = await _context.Compteur
                    .Where(x => x.IsArchive == false &&  x.Id == Id)
                    .Include(c => c.CompteurEquipement)
                        .ThenInclude(ce => ce.Equipement)
                    .Include(c => c.CompteurCellules)
                        .ThenInclude(cc => cc.Cellule)
                    .FirstOrDefaultAsync();
                return compteur;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<List<Compteur>> GetActiveCompteursAsync()
        {
            try
            {
                var compteurs = await _context.Compteur
                    .Where(x => x.IsArchive == false)
                    .Include(c => c.CompteurEquipement.Where(ce => ce.IsArchive == false))
                        .ThenInclude(ce => ce.Equipement)
                    .ToListAsync();
                
                // Retourner uniquement les compteurs qui ont au moins un équipement non archivé
                return compteurs.Where(c => c.CompteurEquipement.Any()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des compteurs actifs: {ex.Message}", ex);
            }
        }

        public async Task<List<DateTime>> GetReadHoursForCompteurAsync(string numeroCompteur, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Récupération des heures depuis Gxdlmsprofilgenericdetail uniquement
                var readHours = await _context.Gxdlmsprofilgenericdetails
                    .Where(d => d.NumeroCompteur == numeroCompteur && 
                               d.DateEnr >= startDate && 
                               d.DateEnr <= endDate &&
                               (d.IsArchive == false || d.IsArchive == null))
                    .Select(d => d.DateEnr!.Value)
                    .ToListAsync();

                return readHours.OrderBy(h => h).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des heures de lecture pour le compteur {numeroCompteur}: {ex.Message}", ex);
            }
        }

    }
}
