using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class RolesEditCommandHandler : IRequestHandler<RoleEditCommand, ResponseBase<RoleResponse>>
    {
        private readonly IRolesCommandRepository _commandRepository;
        private readonly IMediator _mediator;
        private readonly IRolesQueryRepository _rolesQueryRepository;

        public RolesEditCommandHandler(IRolesCommandRepository commandRepository, IMediator mediator, IRolesQueryRepository rolesQueryRepository)
        {
            _mediator = mediator;
            _commandRepository = commandRepository;
            _rolesQueryRepository = rolesQueryRepository;
        }

        public async Task<ResponseBase<RoleResponse>> Handle(RoleEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<RoleResponse> responseBase = new ResponseBase<RoleResponse>();

            var existingRole = await _rolesQueryRepository.GetByIdAsync(request.Id);
            if (existingRole == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Le rôle n'existe pas.";
                return responseBase;
            }

            // Appliquer les modifications
            var roleEntity = RolesMapper.Mapper.Map(request, existingRole);

            if (roleEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            try
            {
                roleEntity.UpdatedAt = DateTime.Now;
                await _commandRepository.UpdateAsync(roleEntity);
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + exp.Message;
                return responseBase;
            }

            var modifiedRole = await _mediator.Send(new GetRoleByIdQuery(request.Id));

            responseBase.Data = RolesMapper.Mapper.Map<RoleResponse>(modifiedRole.Data);

            return responseBase;
        }

    }

}
