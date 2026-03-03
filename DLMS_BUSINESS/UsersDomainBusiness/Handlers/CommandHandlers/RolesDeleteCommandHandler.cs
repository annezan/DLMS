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
    public class RolesDeleteCommandHandler : IRequestHandler<RoleDeleteCommand, ResponseBase<string>>
    {
        private readonly IRolesCommandRepository _commandRepository;
        private readonly IMediator _mediator;
        private readonly IRolesQueryRepository _rolesQueryRepository;

        public RolesDeleteCommandHandler(IRolesCommandRepository commandRepository, IMediator mediator, IRolesQueryRepository rolesQueryRepository)
        {
            _mediator = mediator;
            _commandRepository = commandRepository;
            _rolesQueryRepository = rolesQueryRepository;
        }

        public async Task<ResponseBase<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();

            var existingRole = await _rolesQueryRepository.GetByIdAsync(request.Id);
            if (existingRole == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Le rôle n'existe pas.";
                return responseBase;
            }


            try
            {
                existingRole.DeletedAt = DateTime.Now;
                await _commandRepository.DeleteAsync(existingRole);
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + exp.Message;
                return responseBase;
            }

            responseBase.Data = "Role suppimé avec succès";
            return responseBase;
        }

    }

}
