using DLMS_DAL.Bases;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries
{
    public class CompteurEquipementQueryRepository : QueryBaseRepository<CompteurEquipement>, ICompteurEquipementQueryRepository
    {

        public CompteurEquipementQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<List<CompteurEquipement>> GetCompteurEquipement()
        {
            try
            {

                var CompteurEquipement = await _context.CompteurEquipement.Where(x => x.IsArchive == false).Include(x => x.Equipement).Include(x => x.Compteur).ToListAsync();
                return CompteurEquipement;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<List<CompteurEquipement>> GetCompteurEquipementByIdEquipement(int Id)
        {
            try
            {
                if (_context.CompteurEquipement == null)
                {
                    throw new Exception("CompteurEquipement est null"); // Ou gérez cette situation différemment.
                }
                var CompteurEquipement = await _context.CompteurEquipement.Where(x => x.EquipementId == Id).Include(x=>x.Compteur).ToListAsync();
                return CompteurEquipement;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
        public async Task<List<CompteurEquipement>> GetCompteurEquipementByIdCompteur(int Id)
        {
            try
            {
                if (_context.CompteurEquipement == null)
                {
                    throw new Exception("CompteurEquipement est null"); // Ou gérez cette situation différemment.
                }
                var CompteurEquipement = await _context.CompteurEquipement.Where(x => x.CompteurId == Id).Include(x => x.Equipement).ToListAsync();
                return CompteurEquipement;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<int> GetUniqueIpCountAsync()
        {
            try
            {
                if (_context.CompteurEquipement == null)
                {
                    throw new Exception("CompteurEquipement est null");
                }
                
                var uniqueIpCount = await _context.CompteurEquipement
                    .Where(x => x.IsArchive == false && x.Equipement != null && !string.IsNullOrEmpty(x.Equipement.AdresseIp))
                    .Select(x => x.Equipement.AdresseIp)
                    .Distinct()
                    .CountAsync();
                
                return uniqueIpCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du comptage des IP uniques: {ex.Message}", ex);
            }
        }

    }
}
