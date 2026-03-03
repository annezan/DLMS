using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_MODELS.GxDLMSDomain.Queries
{
    public class GetTestConnexionQuery : IRequest<ResponseBase<TestConnexionResponse>>
    {
        public string? port { get; set; } = null;
        public string? serialport { get; set; } = null;
        public string? AddressIp { get; set; } = null;
        public string? ClientAddress { get; set; } = null;
        public string? SerialNumber { get; set; } = null;
        public string? interfaceType { get; set; } = null;
        public string? password { get; set; } = null;
        public string? AuthenticationKey { get; set; } = null;
        public string? UnicastKey { get; set; } = null;
        public string? Objects { get; set; } = null;
        public GetTestConnexionQuery() { }
        public GetTestConnexionQuery(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            this.port = port;
            this.serialport = serialport;
            this.AddressIp = AddressIp;
            this.ClientAddress = ClientAddress;
            this.SerialNumber = SerialNumber;
            this.interfaceType = interfaceType;
            this.password = password;
            this.AuthenticationKey = AuthenticationKey;
            this.UnicastKey = UnicastKey;
            this.Objects = Objects;

        }
    }
}
