using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    public class GetRoleByIdQuery : IRequest<ResponseBase<RoleResponse>>
    {
        public Guid Id { get; set; }

        public GetRoleByIdQuery() { }

        public GetRoleByIdQuery(Guid Id)
        {
            this.Id = Id;
        }
    }
}
