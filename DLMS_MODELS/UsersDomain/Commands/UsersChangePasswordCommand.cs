using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersChangePasswordCommand : IRequest<ResponseBase<UsersResponse>>
    {
        public string Email { get; set; }
        public string AncienMotDePasse { get; set; }
        public string NouveauMotDePasse { get; set; }
        public string ConfirmationNouveauMotDePasse { get; set; }
    }
}

