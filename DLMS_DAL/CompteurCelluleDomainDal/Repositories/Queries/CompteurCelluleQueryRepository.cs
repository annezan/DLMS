using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CompteurCelluleDomainDal.Repositories.Queries
{
    public class CompteurCelluleQueryRepository : QueryBaseRepository<CompteurCellule>, ICompteurCelluleQueryRepository
    {
        public CompteurCelluleQueryRepository(DLMSDBContext context) :
            base(context)
        {
        }

        public async Task<List<CompteurCellule>> GetByIdCompteurAsync(int compteurId)
        {
            return await _context.CompteurCellule
                .Where(cc => cc.CompteurId == compteurId && cc.IsArchive==false)
                .Include(x=>x.Compteur)
                .Include(c=>c.Cellule)
                .ToListAsync();
        }

        public async Task<List<CompteurCellule>> GetByIdCelluleAsync(int celluleId)
        {
            return await _context.CompteurCellule
                .Where(cc => cc.CelluleId == celluleId && cc.IsArchive==false)
                .Include(x => x.Compteur)
                .Include(c => c.Cellule)
                .ToListAsync();
        }
    }
}
