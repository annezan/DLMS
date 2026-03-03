using DLMS_DAL.Bases;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_MODELS.EquipementDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.EquipementDomainDal.Repositories.Queries
{
    public class EquipementQueryRepository : QueryBaseRepository<Equipement>, IEquipementQueryRepository
    {
        public EquipementQueryRepository(DLMSDBContext context) :
            base(context)
        {
        }

        public async Task<List<Equipement>> GetEquipement()
        {
            try
            {
                if (_context.Equipement == null)
                {
                    throw new Exception("Equipement est null");
                }
                var Equipement = await _context.Equipement
                    .Where(x => x.IsArchive == false)
                    .Include(e => e.EquipementCompteur)
                        .ThenInclude(ec => ec.Compteur)
                        .ThenInclude(c => c.CompteurCellules)
                        .ThenInclude(cc => cc.Cellule)
                    .ToListAsync();
                return Equipement;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<Equipement> GetEquipementById(int Id)
        {
            try
            {
                if (_context.Equipement == null)
                {
                    throw new Exception("Equipement est null");
                }
                var Equipement = await _context.Equipement
                    .Where(x => x.Id == Id)
                    .Include(e => e.EquipementCompteur)
                        .ThenInclude(ec => ec.Compteur)
                        .ThenInclude(c => c.CompteurCellules)
                        .ThenInclude(cc => cc.Cellule)
                    .FirstOrDefaultAsync();
                return Equipement;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<List<Equipement>> GetEquipementByCelluleId(int celluleId)
        {
            try
            {
                if (_context.Equipement == null)
                {
                    throw new Exception("Equipement est null");
                }
                var equipements = await _context.Equipement
                    .Where(x => x.IsArchive == false)
                    .Include(e => e.EquipementCompteur)
                        .ThenInclude(ec => ec.Compteur)
                        .ThenInclude(c => c.CompteurCellules)
                        .ThenInclude(cc => cc.Cellule)
                    .Where(e => e.EquipementCompteur
                        .Any(ec => ec.Compteur.CompteurCellules
                            .Any(cc => cc.CelluleId == celluleId && !cc.IsArchive)))
                    .ToListAsync();
                return equipements;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
