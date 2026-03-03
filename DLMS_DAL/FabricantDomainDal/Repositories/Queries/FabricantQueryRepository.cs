using DLMS_DAL.Bases;
using DLMS_DAL.FabricantsDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_MODELS.FabricantDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.FabricantDomainDal.Repositories.Queries
{
    public class FabricantQueryRepository : QueryBaseRepository<Fabricant>, IFabricantQueryRepository
    {

        public FabricantQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<List<Fabricant>> GetFabricant()
        {
            try
            {
                if (_context.Fabricants == null)
                {
                    throw new Exception("Fabricants est null"); // Ou gérez cette situation différemment.
                }
                var Fabricants = await _context.Fabricants.Where(x => x.IsArchive == false).ToListAsync();
                return Fabricants;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<Fabricant> GetFabricantById(int Id)
        {
            try
            {
                if (_context.Fabricants == null)
                {
                    throw new Exception("Fabricants est null"); // Ou gérez cette situation différemment.
                }
                var Fabricant = await _context.Fabricants.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return Fabricant;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

    }
}
