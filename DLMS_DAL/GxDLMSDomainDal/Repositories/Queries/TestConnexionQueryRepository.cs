using DLMS_DAL.Bases;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using DLMS_COMMUNICATION;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class TestConnexionQueryRepository : ITestConnexionQueryRepository
    {

        public TestConnexionQueryRepository()
        { 
        }
        

        public string GetTestConnexion(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            try
            {
                var resultConnexion=ConnexionCommunication.Connexion(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
                
                return resultConnexion.ToString();
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

    }
}
