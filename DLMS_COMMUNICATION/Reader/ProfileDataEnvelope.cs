using System.Collections.Generic;

namespace DLMS_COMMUNICATION.Reader
{
    public class ProfileDataEnvelope
    {
        public List<KeyValuePair<object[], object[]>> Entries { get; set; } = new();
        public Dictionary<string, ScalerMetadata> Scalers { get; set; } = new();
        public Dictionary<string, double?> TcTtValues { get; set; } = new();
    }

    public class ScalerMetadata
    {
        public double Scaler { get; set; }
        public int Exponent { get; set; }
        public string Unit { get; set; } = "";
        public int UnitCode { get; set; }
    }
}
