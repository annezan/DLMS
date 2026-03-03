using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries
{
    public class GxdlmsprofilgenericQueryRepository : QueryBaseRepository<Gxdlmsprofilgeneric>, IGxdlmsprofilgenericQueryRepository
    {

        public GxdlmsprofilgenericQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<List<Gxdlmsprofilgeneric>> GetProfilgenericByStatus(bool IsArchive)
        {
            try
            {

                var Gxdlmsprofilgenerics=  await _context.Gxdlmsprofilgenerics.Where(x => x.IsArchive == IsArchive).ToListAsync();
                return Gxdlmsprofilgenerics;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<Gxdlmsprofilgeneric> GetProfilgenericByLN(string LN)
        {
            try
            {
                var codeobis= await _context.CodeObis.Where(x => x.Value == LN).FirstOrDefaultAsync();
                var profilgeneric = await _context.Gxdlmsprofilgenerics.Where(x => x.CodeObisId == codeobis.Id).FirstOrDefaultAsync();

                return profilgeneric;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Gxdlmsprofilgeneric?>> GetGxdlmsprofilgeneric()
        {
            return await _context.Gxdlmsprofilgenerics.ToListAsync();
        }

    }
}
