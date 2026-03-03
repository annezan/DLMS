using DLMS_DAL.Bases;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public interface IReadQueryRepository 
    {
        Task<string> GetRead(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects);
    }
}
