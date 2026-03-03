using DLMS_DAL.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands
{
    public interface IGxdlmsprofilgenericdetailseventCommandRepository : ICommandRepository<Gxdlmsprofilgenericdetailsevent>
    {
        Task<bool> AddGxdlmsprofilgenericdetailsevent(Gxdlmsprofilgenericdetailsevent profilgenericdetail);
        Task<Gxdlmsprofilgenericdetailsevent> DeleteGxdlmsprofilgenericdetailsevent(Gxdlmsprofilgenericdetailsevent profilgenericdetail);
    }
}
