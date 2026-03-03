using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetRolesByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ResponseBase<RoleResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IRolesQueryRepository _rolesQueryRepository;

        public GetRolesByIdQueryHandler(IMediator mediator, IRolesQueryRepository rolesQueryRepository)
        {
            _mediator = mediator;
            _rolesQueryRepository = rolesQueryRepository;
        }

        public async Task<ResponseBase<RoleResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<RoleResponse> responseBase = new ResponseBase<RoleResponse>();

            // Récupérer le rôle avec ses permissions
            var role = await _rolesQueryRepository.GetByIdWithPermissionsAsync(request.Id);

            if (role == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Role Introuvable ou désactivé";
                return responseBase;
            }

            responseBase.Data = RolesMapper.Mapper.Map<RoleResponse>(role);

            return responseBase;
        }
    }

}
