using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.CommandHandlers
{
    public class CommandeCompteurDeleteCommandHandler : IRequestHandler<CommandeCompteurDeleteCommand, ResponseBase<string>>
    {
        private readonly ICommandeOrchestrationService _commandeOrchestrationService;
        private readonly IAuthorizationService _authService; // Added
        private readonly ICommandeCompteurQueryRepository _commandeCompteurQueryRepository; // Added
        private readonly IMediator _mediator;

        public CommandeCompteurDeleteCommandHandler(
            ICommandeOrchestrationService commandeOrchestrationService,
            IAuthorizationService authService, // Added
            ICommandeCompteurQueryRepository commandeCompteurQueryRepository, // Added
            IMediator mediator)
        {
            _mediator = mediator;
            _commandeOrchestrationService = commandeOrchestrationService;
            _authService = authService; // Added
            _commandeCompteurQueryRepository = commandeCompteurQueryRepository; // Added
        }

        public async Task<ResponseBase<string>> Handle(CommandeCompteurDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();

            try
            {
                // Récupérer la relation pour vérifier l'accès
                var existingRelation = await _commandeCompteurQueryRepository.GetCommandeCompteurById(request.Id);
                
                if (existingRelation == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Relation Commande-Compteur introuvable.";
                    return responseBase;
                }

                // Vérifier l'accès (double check : commande ET compteur)
                var hasAccessToCommande = await _authService.UserHasAccessToCommandeAsync(request.UserId, existingRelation.CommandeId);
                var hasAccessToCompteur = await _authService.UserHasAccessToCompteurAsync(request.UserId, existingRelation.CompteurId);

                if (!hasAccessToCommande || !hasAccessToCompteur)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas accès pour supprimer cette relation Commande-Compteur.";
                    return responseBase;
                }

                // Appliquer les modifications
                var CommandeCompteurEntity = CommandeCompteurMapper.Mapper.Map<CommandeCompteur>(request);
                var deleteCommandeCompteur = await _commandeOrchestrationService.DeleteCommandeCompteurWithOrphanCheck(CommandeCompteurEntity);

                if (CommandeCompteurEntity is null || deleteCommandeCompteur == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = "Succès";
                responseBase.IsSuccess = true;

                return responseBase;
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + exp.Message;
                return responseBase;
            }
        }

    }

}
