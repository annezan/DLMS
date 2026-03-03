using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetRoleByCodeQuery : IRequest<ResponseBase<RoleResponse>>
    {
        public string Code { get; private set; }

        public GetRoleByCodeQuery(string code)
        {
            this.Code = code;
        }
    }
}
