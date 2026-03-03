using DLMS_BUSINESS.CommandeDomainBusiness.Mappers;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Commands;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeDomainBusiness.Handlers.CommandHandlers
{
    public class CommandeAddCommandHandler : IRequestHandler<CommandeAddCommand, ResponseBase<CommandeResponse>>
    {
        private readonly ICommandeOrchestrationService _commandeOrchestrationService;
        private readonly IMediator _mediator;

        public CommandeAddCommandHandler(ICommandeOrchestrationService commandeOrchestrationService, IMediator mediator)
        {
            _mediator = mediator;
            _commandeOrchestrationService = commandeOrchestrationService;
        }

        public async Task<ResponseBase<CommandeResponse>> Handle(CommandeAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CommandeResponse> responseBase = new ResponseBase<CommandeResponse>();
            try
            {
                var CommandeEntity = CommandeMapper.Mapper.Map<Commande>(request);
                CommandeEntity.Statut = "En cours";
                CommandeEntity.CreatedAt = DateTime.Now;

                var newCommande = await _commandeOrchestrationService.AddCommandeWithCompteurs(CommandeEntity, request.CompteurId);

                if (CommandeEntity is null || newCommande == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = CommandeMapper.Mapper.Map<CommandeResponse>(newCommande);

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu" + ex.Message;
                return responseBase;
            }
        }
    }
}
