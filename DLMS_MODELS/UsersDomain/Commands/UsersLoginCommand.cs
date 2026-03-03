using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersLoginCommand : IRequest<ResponseBase<TokenResponse>>
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
    }
}
