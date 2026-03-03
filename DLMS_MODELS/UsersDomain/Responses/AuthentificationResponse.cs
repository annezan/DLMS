namespace DLMS_MODELS.UsersDomain.Responses
{
    public class AuthentificationResponse : TokenResponse
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
