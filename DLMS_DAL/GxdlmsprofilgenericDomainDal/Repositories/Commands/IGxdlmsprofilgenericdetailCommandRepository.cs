using DLMS_DAL.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;

namespace DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands
{
    public interface IGxdlmsprofilgenericdetailCommandRepository : ICommandRepository<Gxdlmsprofilgenericdetail>
    {
        Task<bool> AddGxdlmsprofilgenericdetail(Gxdlmsprofilgenericdetail profilgenericdetail);
        Task<Gxdlmsprofilgenericdetail> DeleteGxdlmsprofilgenericdetail(Gxdlmsprofilgenericdetail profilgenericdetail);

    }
}
