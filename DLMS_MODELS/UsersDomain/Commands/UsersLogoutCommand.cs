using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersLogoutCommand : IRequest<ResponseBase<string>>
    {
        public Guid UserId { get; set; }
    }
}
