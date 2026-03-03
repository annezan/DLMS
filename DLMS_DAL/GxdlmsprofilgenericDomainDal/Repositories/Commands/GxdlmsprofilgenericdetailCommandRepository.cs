using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_DAL.Helpers;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericdetailDomainDal.Repositories
{
    public class GxdlmsprofilgenericdetailCommandRepository : CommandRepository<Gxdlmsprofilgenericdetail>, IGxdlmsprofilgenericdetailCommandRepository
    {
        public GxdlmsprofilgenericdetailCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }

        public async Task<bool> AddGxdlmsprofilgenericdetail(Gxdlmsprofilgenericdetail profilgenericdetail)
        {
            try
            {
                var detailold = await _context.Gxdlmsprofilgenericdetails.AsNoTracking().FirstOrDefaultAsync(x => x.Value == profilgenericdetail.Value && x.DateEnr == profilgenericdetail.DateEnr && x.GxdlmsprofilgenericId == profilgenericdetail.GxdlmsprofilgenericId && x.NumeroCompteur == profilgenericdetail.NumeroCompteur && x.CodeObisId==profilgenericdetail.CodeObisId) ;
                if (detailold == null)
                {
                    profilgenericdetail.Id = 0;
                    _context.Gxdlmsprofilgenericdetails.Add(profilgenericdetail);
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
        public async Task<Gxdlmsprofilgenericdetail> DeleteGxdlmsprofilgenericdetail(Gxdlmsprofilgenericdetail profilgenericdetail)
        {
            try
            {
                var detailold = await _context.Gxdlmsprofilgenericdetails.AsNoTracking().FirstOrDefaultAsync(x => x.Value == profilgenericdetail.Value && x.DateEnr == profilgenericdetail.DateEnr && x.GxdlmsprofilgenericId == profilgenericdetail.GxdlmsprofilgenericId && x.NumeroCompteur == profilgenericdetail.NumeroCompteur);
                detailold.IsArchive = detailold.IsArchive == profilgenericdetail.IsArchive ? detailold.IsArchive : profilgenericdetail.IsArchive;

                _context.Gxdlmsprofilgenericdetails.Update(detailold);
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
