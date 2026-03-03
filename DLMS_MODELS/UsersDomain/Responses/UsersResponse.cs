using DLMS_MODELS.PosteDomain.Responses;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_MODELS.UsersDomain.Responses
{
    public class UsersResponse
    {
        public Guid Id { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public string Nom { get; set; }
        public string Prenoms { get; set; }
        public DateOnly? DateNaissance { get; set; }
        public string NomPrenoms
        {
            get
            {
                return string.Join(" ", this.Nom, this.Prenoms);
            }
        }

        public Guid? RoleId { get; set; }
        public RoleResponse? Role { get; set; }
        public int? PosteId { get; set; }
        public PosteResponse? Poste { get; set; }
        public bool IsLocked { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
