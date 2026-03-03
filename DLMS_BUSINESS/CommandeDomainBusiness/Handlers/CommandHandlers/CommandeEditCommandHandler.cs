using DLMS_BUSINESS.CommandeDomainBusiness.Mappers;
using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.CommandeDomain.Commands;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CommandeDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_BUSINESS.CommandeDomainBusiness.Handlers.CommandHandlers
{
    public class CommandeEditCommandHandler : IRequestHandler<CommandeEditCommand, ResponseBase<CommandeResponse>>
    {
        private readonly ICommandeCommandRepository _CommandeCommandRepository;
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public CommandeEditCommandHandler(
            ICommandeCommandRepository CommandeCommandRepository, 
            IMediator mediator, 
            ICommandeQueryRepository CommandeQueryRepository,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _CommandeCommandRepository = CommandeCommandRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<CommandeResponse>> Handle(CommandeEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CommandeResponse> responseBase = new ResponseBase<CommandeResponse>();

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
                        responseBase.Message = "Accès refusé : Vous ne pouvez pas modifier cette commande car elle contient des compteurs qui n'appartiennent pas à votre poste.";
                        return responseBase;
                    }
                }

                // Appliquer les modifications
                var CommandeEntity = CommandeMapper.Mapper.Map<Commande>(request);
                var newCommande = await _CommandeCommandRepository.EditCommande(CommandeEntity);

                if (CommandeEntity is null || newCommande == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = CommandeMapper.Mapper.Map<CommandeResponse>(newCommande);
                responseBase.IsSuccess = true;
                responseBase.Message = "Commande modifiée avec succès";

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
