using DLMS_DAL.Bases;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public interface ITestConnexionQueryRepository 
    {
        string GetTestConnexion(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects);
    }
}
