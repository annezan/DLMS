using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries
{
    public class GxdlmsprofilgenericdetailQueryRepository : QueryBaseRepository<Gxdlmsprofilgenericdetail>, IGxdlmsprofilgenericdetailQueryRepository
    {

        public GxdlmsprofilgenericdetailQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<List<Gxdlmsprofilgenericdetail>> GetProfilgenericdetailByStatus(string numeroCompteur, int gxdlmsprofilgenericId, DateTime dateEnr)
        {
            try
            {
                var Gxdlmsprofilgenericdetails = await _context.Gxdlmsprofilgenericdetails
                    .Where(x => x.IsArchive == false
                        && x.NumeroCompteur == numeroCompteur
                        && x.GxdlmsprofilgenericId == gxdlmsprofilgenericId
                        && x.DateEnr.Value.Date == dateEnr.Date)
                    .Include(x => x.Codeobis)
                    .Include(x => x.Gxdlmsprofilgeneric)
                    .ThenInclude(x => x.Codeobis)
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                // La conversion ÷1000 a été supprimée : Value contient désormais
                // la valeur correctement scalée (raw × scaler Gurux) depuis le service.
                // L'ancienne division créait une double conversion.

                return Gxdlmsprofilgenericdetails;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<Gxdlmsprofilgenericdetail>> GetProfilgenericdetailByMultipleCriteria(List<string> numeroCompteurs, List<int> gxdlmsprofilgenericIds, List<int> codeobisIds, DateTime dateEnr)
        {
            try
            {
                var query = _context.Gxdlmsprofilgenericdetails
                    .Where(x => x.IsArchive == false && x.DateEnr.Value.Date == dateEnr.Date);

                if (numeroCompteurs != null && numeroCompteurs.Any())
                {
                    query = query.Where(x => numeroCompteurs.Contains(x.NumeroCompteur));
                }

                if (gxdlmsprofilgenericIds != null && gxdlmsprofilgenericIds.Any())
                {
                    query = query.Where(x => gxdlmsprofilgenericIds.Contains(x.GxdlmsprofilgenericId));
                }

                if (codeobisIds != null && codeobisIds.Any())
                {
                    query = query.Where(x => codeobisIds.Contains(x.CodeObisId));
                }

                var Gxdlmsprofilgenericdetails = await query
                    .Include(x => x.Codeobis)
                    .Include(x => x.Gxdlmsprofilgeneric)
                    .ThenInclude(x => x.Codeobis)
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                // La conversion ÷1000 a été supprimée : Value contient désormais
                // la valeur correctement scalée (raw × scaler Gurux) depuis le service.

                return Gxdlmsprofilgenericdetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
