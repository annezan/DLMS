
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_MODELS.GxDLMSDomain.Queries;

namespace DLMS_UTILITIES
{
    public class ReadUtilities
    {

        private readonly IReadQueryRepository _ReadQueryRepository;
        private readonly IReadObjectProfileQueryRepository _ReadObjectProfileQueryRepository;



        public ReadUtilities(IReadQueryRepository ReadQueryRepository, IReadObjectCommandeQueryRepository ReadObjectCommandeQueryRepository, IReadObjectProfileQueryRepository ReadObjectProfileQueryRepository)
        {
            _ReadQueryRepository = ReadQueryRepository;
            _ReadObjectProfileQueryRepository = ReadObjectProfileQueryRepository;
        }
        public async Task<string> GetRead(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            try
            {
                return await _ReadQueryRepository.GetRead(port,serialport,AddressIp,ClientAddress,SerialNumber,interfaceType,Objects);

            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }

        public async Task<string> GetReadObjectProfile(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            try
            {
                return await _ReadObjectProfileQueryRepository.GetReadObjectProfile(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, Objects);

            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }



    }
}
