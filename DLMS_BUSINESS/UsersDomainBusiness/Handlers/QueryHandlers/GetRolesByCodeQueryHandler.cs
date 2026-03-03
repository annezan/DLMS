using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetRolesByCodeQueryHandler : IRequestHandler<GetRoleByCodeQuery, ResponseBase<RoleResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IRolesQueryRepository _rolesQueryRepository;

        public GetRolesByCodeQueryHandler(IMediator mediator, IRolesQueryRepository rolesQueryRepository)
        {
            _mediator = mediator;
            _rolesQueryRepository = rolesQueryRepository;
        }

        public async Task<ResponseBase<RoleResponse>> Handle(GetRoleByCodeQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<RoleResponse> responseBase = new ResponseBase<RoleResponse>();

            var role = await _rolesQueryRepository.GetRoleByCode(request.Code);

            if (role == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Role Introuvable ou désactivé";
                return responseBase;
            }

            responseBase.Data = RolesMapper.Mapper.Map<RoleResponse>(role); ;

            return responseBase;
        }
    }

}
