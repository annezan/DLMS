using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetPermissionsQuery : IRequest<ResponseBase<List<PermissionResponse>>>
    {
        public GetPermissionsQuery()
        {
        }
    }
}
