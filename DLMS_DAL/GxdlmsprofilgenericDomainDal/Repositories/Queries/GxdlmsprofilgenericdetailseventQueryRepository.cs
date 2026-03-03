using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries
{
    public class GxdlmsprofilgenericdetailseventQueryRepository : QueryBaseRepository<Gxdlmsprofilgenericdetailsevent>, IGxdlmsprofilgenericdetailseventQueryRepository
    {
        public GxdlmsprofilgenericdetailseventQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<List<Gxdlmsprofilgenericdetailsevent>> GetProfilgenericdetailseventByStatus(string numeroCompteur)
        {
            try
            {

                var Gxdlmsprofilgenericdetailsevents= await _context.Gxdlmsprofilgenericdetailsevents.Where(x => x.IsArchive == false && x.NumeroCompteur == numeroCompteur).Include(x => x.Codeobis).Include(x => x.Gxdlmsprofilgeneric).ThenInclude(x => x.Codeobis).Include(x => x.Event).OrderBy(x=>x.Id).ToListAsync();
                return Gxdlmsprofilgenericdetailsevents;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


    }
}
