using System.Diagnostics;

namespace DLMS_MODELS
{
    public class DLMSConnectionParameters
    {
        public string? Port { get; set; }
        public string? SerialPort { get; set; }
        public string? AddressIp { get; set; }
        public string? ClientAddress { get; set; }
        public string? SerialNumber { get; set; }
        public string? InterfaceType { get; set; }
        public string? Password { get; set; }
        public string? AuthenticationKey { get; set; }
        public string? UnicastKey { get; set; }
        public string? Objects { get; set; }
        public TraceLevel Trace { get; set; } = TraceLevel.Verbose;
        public string? OutputFile { get; set; }
        public string? InvocationCounter { get; set; }
        public bool UseGbt { get; set; } = false;
    }
}
