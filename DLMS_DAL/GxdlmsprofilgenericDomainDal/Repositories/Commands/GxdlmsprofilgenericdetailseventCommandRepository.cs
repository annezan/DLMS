using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_DAL.Helpers;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericdetailDomainDal.Repositories
{
    public class GxdlmsprofilgenericdetailseventCommandRepository : CommandRepository<Gxdlmsprofilgenericdetailsevent>, IGxdlmsprofilgenericdetailseventCommandRepository
    {
        public GxdlmsprofilgenericdetailseventCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }

        public async Task<bool> AddGxdlmsprofilgenericdetailsevent(Gxdlmsprofilgenericdetailsevent profilgenericdetail)
        {
            try
            {
                var detailold = await _context.Gxdlmsprofilgenericdetailsevents.AsNoTracking().FirstOrDefaultAsync(x => x.Value == profilgenericdetail.Value && x.DateEnr == profilgenericdetail.DateEnr && x.CodeObisId == profilgenericdetail.CodeObisId && x.GxdlmsprofilgenericId == profilgenericdetail.GxdlmsprofilgenericId && x.NumeroCompteur == profilgenericdetail.NumeroCompteur && x.EventId== profilgenericdetail.EventId);
                if (detailold == null)
                {
                    profilgenericdetail.Id = 0;
                    _context.Gxdlmsprofilgenericdetailsevents.Add(profilgenericdetail);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }


        }
        public async Task<Gxdlmsprofilgenericdetailsevent> DeleteGxdlmsprofilgenericdetailsevent(Gxdlmsprofilgenericdetailsevent profilgenericdetail)
        {
            try
            {
                var detailold = await _context.Gxdlmsprofilgenericdetailsevents.AsNoTracking().FirstOrDefaultAsync(x => x.Value == profilgenericdetail.Value && x.DateEnr == profilgenericdetail.DateEnr && x.CodeObisId == profilgenericdetail.CodeObisId && x.GxdlmsprofilgenericId == profilgenericdetail.GxdlmsprofilgenericId && x.NumeroCompteur == profilgenericdetail.NumeroCompteur);
                detailold.IsArchive = detailold.IsArchive == profilgenericdetail.IsArchive ? detailold.IsArchive : profilgenericdetail.IsArchive;

                _context.Gxdlmsprofilgenericdetailsevents.Update(detailold);
                await _context.SaveChangesAsync();

                return profilgenericdetail;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
