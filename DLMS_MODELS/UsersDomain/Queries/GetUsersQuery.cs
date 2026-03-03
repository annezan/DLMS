using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetUsersQuery : IRequest<ResponseBase<List<UsersResponse>>>
    {
        public GetUsersQuery()
        {
        }
    }
}
