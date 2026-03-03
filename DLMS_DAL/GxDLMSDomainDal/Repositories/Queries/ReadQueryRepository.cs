using DLMS_DAL.Bases;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using DLMS_COMMUNICATION;
using DLMS_COMMUNICATION.Reader;
using DLMS_MODELS.AssociationKeyDomain.Entities;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class ReadQueryRepository : QueryBaseRepository<AssociationKey>, IReadQueryRepository
    {
        public ReadQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }


        public async Task<string> GetRead(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            try
            {
                var authentication = await _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname== "authentication" &&  x.CompteurId.Contains(SerialNumber)).FirstOrDefaultAsync();
                var unicast = await _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "unicast" && x.CompteurId.Contains(SerialNumber)).FirstOrDefaultAsync();

                if (authentication == null || unicast == null)
                {
                    return "Lecture impossible";
                }

                var AuthenticationKey= Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS");
                var UnicastKey= Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS");

                var result =ReaderCommunication.Read(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, authentication.Pwd, AuthenticationKey, UnicastKey, Objects);
                if (result== "Lecture impossible")
                {
                    return "Lecture impossible";
                }
                return result.ToString();
                
            }
            catch (Exception ex)
            {
                return "Lecture impossible"; 
            }

        }

    }
}
