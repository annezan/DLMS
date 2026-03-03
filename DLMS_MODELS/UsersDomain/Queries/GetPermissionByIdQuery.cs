using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetPermissionByIdQuery : IRequest<ResponseBase<PermissionResponse>>
    {
        public Guid Id { get; private set; }

        public GetPermissionByIdQuery(Guid Id)
        {
            this.Id = Id;
        }
    }
}
