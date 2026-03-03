using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_MODELS.GxDLMSDomain.Queries
{
    public class GetReadRowsByEntryQuery : IRequest<ResponseBase<ReadRowsByEntryResponse>>
    {
        public string? port { get; set; } = null;
        public string? serialport { get; set; } = null;
        public string? AddressIp { get; set; } = null;
        public string? ClientAddress { get; set; } = null;
        public string? SerialNumber { get; set; } = null;
        public string? interfaceType { get; set; } = null;
        public string? Objects { get; set; } = null;
        public GetReadRowsByEntryQuery() { }
        public GetReadRowsByEntryQuery(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            this.port = port;
            this.serialport = serialport;
            this.AddressIp = AddressIp;
            this.ClientAddress = ClientAddress;
            this.SerialNumber = SerialNumber;
            this.interfaceType = interfaceType;          
            this.Objects = Objects;

        }
    }
}
