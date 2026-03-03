using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersAddCommand : IRequest<ResponseBase<UsersResponse>>
    {
        public string Nom { get; set; }
        public string Prenoms { get; set; }
        public DateOnly DateNaissance { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        private DateTime CreatedAt { get; set; }
        public Guid RoleId { get; set; }
        
        /// <summary>
        /// ID du poste à assigner (optionnel - null = accès global)
        /// </summary>
        public int? PosteId { get; set; }

        public UsersAddCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
