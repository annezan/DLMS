using DLMS_DAL.Bases;
using DLMS_DAL.CelluleDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_MODELS.CelluleDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CelluleDomainDal.Repositories.Queries
{
    public class CelluleQueryRepository : QueryBaseRepository<Cellule>, ICelluleQueryRepository
    {
        public CelluleQueryRepository(DLMSDBContext context) :
            base(context)
        {
        }

        public async Task<List<Cellule>> GetCellule()
        {
            try
            {
                if (_context.Cellule == null)
                {
                    throw new Exception("Cellule est null");
                }
                var cellules = await _context.Cellule
                    .Include(c => c.Poste)
                    .Include(c => c.CelluleCompteurs)
                        .ThenInclude(cc => cc.Compteur)
                    .Where(x => x.IsArchive == false)
                    .ToListAsync();
                return cellules;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<Cellule> GetCelluleById(int id)
        {
            try
            {
                var cellule = await _context.Cellule
                    .Include(c => c.Poste)
                    .Include(c => c.CelluleCompteurs)
                        .ThenInclude(cc => cc.Compteur)
                    .Where(x => x.Id == id && x.IsArchive == false)
                    .FirstOrDefaultAsync();
                return cellule;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<List<Cellule>> GetCelluleByPosteId(int posteId)
        {
            try
            {
                var cellules = await _context.Cellule
                    .Include(c => c.Poste)
                    .Include(c => c.CelluleCompteurs)
                        .ThenInclude(cc => cc.Compteur)
                    .Where(x => x.PosteId == posteId && x.IsArchive == false)
                    .ToListAsync();
                return cellules;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}

