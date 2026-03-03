using DLMS_BUSINESS.CommandeDomainBusiness.Mappers;
using DLMS_DAL.Services;
using DLMS_MODELS.CommandeDomain.Commands;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_BUSINESS.CommandeDomainBusiness.Handlers.CommandHandlers
{
    public class CommandeDeleteCommandHandler : IRequestHandler<CommandeDeleteCommand, ResponseBase<string>>
    {
        private readonly ICommandeOrchestrationService _commandeOrchestrationService;
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public CommandeDeleteCommandHandler(
            ICommandeOrchestrationService commandeOrchestrationService, 
            IMediator mediator,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _commandeOrchestrationService = commandeOrchestrationService;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<string>> Handle(CommandeDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();

            try
            {
                // Vérifier l'accès à la commande si l'utilisateur a un poste assigné
                var hasPosteAssigne = await _authorizationService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var hasAccess = await _authorizationService.UserHasAccessToCommandeAsync(request.UserId, request.Id);
                    if (!hasAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Accès refusé : Vous ne pouvez pas supprimer cette commande car elle contient des compteurs qui n'appartiennent pas à votre poste.";
                        return responseBase;
                    }
                }

                var CommandeEntity = CommandeMapper.Mapper.Map<Commande>(request);
                var success = await _commandeOrchestrationService.DeleteCommandeWithCompteurs(CommandeEntity);

                if (!success)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu lors de la suppression";
                    return responseBase;
                }

                responseBase.Data = "Succès";
                responseBase.IsSuccess = true;
                responseBase.Message = "Commande supprimée avec succès";
                return responseBase;
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Un problème technique est survenu : {exp.Message}";
                return responseBase;
            }
        }
    }
}
