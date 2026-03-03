using DLMS_DAL.Bases;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public interface IReadObjectProfileQueryRepository 
    {
        Task<string> GetReadObjectProfile(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects);
    }
}
