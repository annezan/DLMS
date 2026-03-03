using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersForgotPasswordCommand : IRequest<ResponseBase<UsersResponse>>
    {
        public string Email { get; set; }

        public UsersForgotPasswordCommand()
        {
        }
    }
}
