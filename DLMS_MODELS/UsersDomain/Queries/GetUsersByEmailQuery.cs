using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetUsersByEmailQuery : IRequest<ResponseBase<UsersResponse>>
    {
        public string Email { get; private set; }

        public GetUsersByEmailQuery(string email)
        {
            this.Email = email;
        }

    }
}
