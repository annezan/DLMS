using DLMS_DAL.Bases;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries
{
    public interface IGxdlmsprofilgenericdetailQueryRepository : IQueryBaseRepository<Gxdlmsprofilgenericdetail>
    {
        Task<List<Gxdlmsprofilgenericdetail>> GetProfilgenericdetailByStatus(string numeroCompteur, int gxdlmsprofilgenericId, DateTime dateEnr);
        Task<List<Gxdlmsprofilgenericdetail>> GetProfilgenericdetailByMultipleCriteria(List<string> numeroCompteurs, List<int> gxdlmsprofilgenericIds, List<int> codeobisIds, DateTime dateEnr);

        
    }
}
