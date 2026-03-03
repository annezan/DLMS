using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class RolesAddCommandHandler : IRequestHandler<RoleAddCommand, ResponseBase<RoleResponse>>
    {
        private readonly IRolesCommandRepository _commandRepository;
        private readonly IMediator _mediator;

        public RolesAddCommandHandler(IRolesCommandRepository commandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _commandRepository = commandRepository;
        }

        public async Task<ResponseBase<RoleResponse>> Handle(RoleAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<RoleResponse> responseBase = new ResponseBase<RoleResponse>();

            var roleExist = await _mediator.Send(new GetRoleByCodeQuery(request.Code));

            if (roleExist.Data != null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Ce code est déjà utilisé pour le rôle " + roleExist.Data.Libelle;
                return responseBase;
            }

            var roleEntity = RolesMapper.Mapper.Map<Role>(request);

            if (roleEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            roleEntity.CreatedAt = DateTime.Now;
            var newRole = await _commandRepository.AddAsync(roleEntity);

            responseBase.Data = RolesMapper.Mapper.Map<RoleResponse>(newRole);

            return responseBase;
        }

    }

}
