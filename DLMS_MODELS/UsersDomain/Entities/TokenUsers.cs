namespace DLMS_MODELS.UsersDomain.Entities
{
    public class TokenUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Token { get; set; }

        public string JwtId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiryAt { get; set; }

        public bool IsUsed { get; set; }

        public Guid UsersId { get; set; }

        public User Users { get; set; }
    }
}
