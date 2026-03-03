using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersRefreshTokenCommand : IRequest<ResponseBase<TokenResponse>>
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
