using DLMS_DAL.Bases;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_MODELS.PosteDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.PosteDomainDal.Repositories.Queries
{
    public class PosteQueryRepository : QueryBaseRepository<Poste>, IPosteQueryRepository
    {
        public PosteQueryRepository(DLMSDBContext context) :
            base(context)
        {
        }

        public async Task<List<Poste>> GetPoste()
        {
            try
            {
                if (_context.Poste == null)
                {
                    throw new Exception("Poste DbSet is null - database context not properly initialized");
                }
                var Poste = await _context.Poste
                    .Include(p => p.Cellules)
                    .Where(x => x.IsArchive == false)
                    .ToListAsync();
                return Poste;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving postes: {ex.Message}", ex);
            }
        }

        public async Task<Poste> GetPosteById(int Id)
        {
            try
            {
                if (_context.Poste == null)
                {
                    throw new Exception("Poste DbSet is null - database context not properly initialized");
                }
                var Poste = await _context.Poste
                    .Include(p => p.Cellules)
                    .Where(x => x.Id == Id)
                    .FirstOrDefaultAsync();
                return Poste;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving poste with ID {Id}: {ex.Message}", ex);
            }
        }
    }
}
