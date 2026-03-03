using DLMS_MODELS.Bases;

namespace DLMS_MODELS.ReadObjectCommandeDomain.Responses
{
    public class ReadObjectCommandeResponse
    {
        public int CommandeId { get; set; }
        public string Result { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
