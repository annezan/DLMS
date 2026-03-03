using DLMS_BUSINESS.CommandeDomainBusiness.Mappers;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Queries;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeDomainBusiness.Handlers.QueryHandlers
{
    public class GetCommandeByIdQueryHandler : IRequestHandler<GetCommandeByIdQuery, ResponseBase<CommandeByIdResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ICommandeQueryRepository _CommandeQueryRepository;
        private readonly IAuthorizationService _authorizationService;

        public GetCommandeByIdQueryHandler(
            IMediator mediator, 
            ICommandeQueryRepository CommandeQueryRepository,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _CommandeQueryRepository = CommandeQueryRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<CommandeByIdResponse>> Handle(GetCommandeByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<CommandeByIdResponse> responseBase = new ResponseBase<CommandeByIdResponse>();

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
                        responseBase.Message = "Accès refusé : Cette commande contient des compteurs qui n'appartiennent pas à votre poste.";
                        return responseBase;
                    }
                }

                // Récupérer la commande
                var commande = await _CommandeQueryRepository.GetCommandeById(request.Id);

                if (commande == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Commande introuvable";
                    return responseBase;
                }

                responseBase.Data = CommandeByIdMapper.Mapper.Map<CommandeByIdResponse>(commande);
                responseBase.IsSuccess = true;
                responseBase.Message = "Commande récupérée avec succès";

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération de la commande : {ex.Message}";
                return responseBase;
            }
        }
    }
}
