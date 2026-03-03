using DLMS_DAL.Bases;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries
{
    public interface IGxdlmsprofilgenericdetailseventQueryRepository : IQueryBaseRepository<Gxdlmsprofilgenericdetailsevent>
    {
        Task<List<Gxdlmsprofilgenericdetailsevent>> GetProfilgenericdetailseventByStatus(string numeroCompteur);
        
    }
}
