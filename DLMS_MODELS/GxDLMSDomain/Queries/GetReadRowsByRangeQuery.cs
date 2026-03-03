using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_MODELS.GxDLMSDomain.Queries
{
    public class GetReadRowsByRangeQuery : IRequest<ResponseBase<ReadRowsByRangeResponse>>
    {
        public string datestart { get; set; }
        public string dateend { get; set; }
        public string? port { get; set; } = null;
        public string? serialport { get; set; } = null;
        public string? AddressIp { get; set; } = null;
        public string? ClientAddress { get; set; } = null;
        public string? SerialNumber { get; set; } = null;
        public string? interfaceType { get; set; } = null;
        public string? Objects { get; set; } = null;
        public GetReadRowsByRangeQuery() { }
        public GetReadRowsByRangeQuery(string datestart, string dateend, string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            this.datestart=datestart;
            this.dateend=dateend;
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
