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

                // Divide values by 1000 for specific OBIS units
                var unitsToDivide = new[] { "Kvarh", "Kvar", "Kwh", "KVA", "KW", "KVAh" };
                
                var result = new List<Gxdlmsprofilgenericdetail>();
                
                foreach (var detail in Gxdlmsprofilgenericdetails)
                {
                    var newDetail = new Gxdlmsprofilgenericdetail
                    {
                        Id = detail.Id,
                        CodeObisId = detail.CodeObisId,
                        GxdlmsprofilgenericId = detail.GxdlmsprofilgenericId,
                        NumeroCompteur = detail.NumeroCompteur,
                        DateEnr = detail.DateEnr,
                        IsArchive = detail.IsArchive,
                        Codeobis = detail.Codeobis,
                        Gxdlmsprofilgeneric = detail.Gxdlmsprofilgeneric,
                        Value = detail.Value
                    };
                    
                    if (detail.Codeobis?.Unit != null && unitsToDivide.Contains(detail.Codeobis.Unit))
                    {
                        if (decimal.TryParse(detail.Value, out decimal value))
                        {
                            newDetail.Value = (value / 1000).ToString();
                        }
                    }
                    
                    result.Add(newDetail);
                }

                return result;
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

                // Divide values by 1000 for specific OBIS units
                var unitsToDivide = new[] { "Kvarh", "Kvar", "Kwh", "KVA", "KW", "KVAh" };
                
                var result = new List<Gxdlmsprofilgenericdetail>();
                
                foreach (var detail in Gxdlmsprofilgenericdetails)
                {
                    var newDetail = new Gxdlmsprofilgenericdetail
                    {
                        Id = detail.Id,
                        CodeObisId = detail.CodeObisId,
                        GxdlmsprofilgenericId = detail.GxdlmsprofilgenericId,
                        NumeroCompteur = detail.NumeroCompteur,
                        DateEnr = detail.DateEnr,
                        IsArchive = detail.IsArchive,
                        Codeobis = detail.Codeobis,
                        Gxdlmsprofilgeneric = detail.Gxdlmsprofilgeneric,
                        Value = detail.Value
                    };
                    
                    if (detail.Codeobis?.Unit != null && unitsToDivide.Contains(detail.Codeobis.Unit))
                    {
                        if (decimal.TryParse(detail.Value, out decimal value))
                        {
                            newDetail.Value = (value / 1000).ToString();
                        }
                    }
                    
                    result.Add(newDetail);
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
