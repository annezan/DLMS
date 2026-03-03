using DLMS_DAL.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries
{
    public interface IGxdlmsprofilgenericQueryRepository : IQueryBaseRepository<Gxdlmsprofilgeneric>
    {

        Task<List<Gxdlmsprofilgeneric>> GetProfilgenericByStatus(bool IsArchive);
        Task<Gxdlmsprofilgeneric> GetProfilgenericByLN(string LN);
        Task<List<Gxdlmsprofilgeneric?>> GetGxdlmsprofilgeneric();
    }
}
