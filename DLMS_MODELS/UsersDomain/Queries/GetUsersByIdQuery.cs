using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetUsersByIdQuery : IRequest<ResponseBase<UsersResponse>>
    {
        public Guid Id { get; private set; }

        public GetUsersByIdQuery(Guid Id)
        {
            this.Id = Id;
        }
    }
}
