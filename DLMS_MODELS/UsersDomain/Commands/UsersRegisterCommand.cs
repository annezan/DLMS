using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersRegisterCommand : IRequest<ResponseBase<UsersResponse>>
    {
        public string Nom { get; set; }
        public string Prenoms { get; set; }
        public DateOnly DateNaissance { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string MotDePasse { get; set; }
        public string ConfirmationMotDePasse { get; set; }
        private DateTime CreatedAt { get; set; }

        public Guid RoleId { get; set; }

        public UsersRegisterCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
