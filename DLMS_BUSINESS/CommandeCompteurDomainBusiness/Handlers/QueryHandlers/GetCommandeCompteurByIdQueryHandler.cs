using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Queries;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetCommandeCompteurByIdQueryHandler : IRequestHandler<GetCommandeCompteurByIdQuery, ResponseBase<CommandeCompteurResponse>>
    {
        private readonly ICommandeCompteurQueryRepository _commandeCompteurQueryRepository;
        private readonly IAuthorizationService _authService; // Added

        public GetCommandeCompteurByIdQueryHandler(
            ICommandeCompteurQueryRepository commandeCompteurQueryRepository,
            IAuthorizationService authService) // Added
        {
            _commandeCompteurQueryRepository = commandeCompteurQueryRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<CommandeCompteurResponse>> Handle(GetCommandeCompteurByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<CommandeCompteurResponse> responseBase = new ResponseBase<CommandeCompteurResponse>();
            try
            {
                var commandeCompteur = await _commandeCompteurQueryRepository.GetCommandeCompteurById(request.Id);
                if (commandeCompteur == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Commande Compteur Introuvable";
                    return responseBase;
                }

                var commandeCompteurResponse = CommandeCompteurMapper.Mapper.Map<CommandeCompteurResponse>(commandeCompteur);

                // Vérifier l'accès (double check : commande ET compteur)
                var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var hasAccessToCommande = await _authService.UserHasAccessToCommandeAsync(request.UserId, commandeCompteurResponse.CommandeId);
                    var hasAccessToCompteur = await _authService.UserHasAccessToCompteurAsync(request.UserId, commandeCompteurResponse.CompteurId);

                    if (!hasAccessToCommande || !hasAccessToCompteur)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Vous n'avez pas accès à cette relation Commande-Compteur.";
                        return responseBase;
                    }
                }

                responseBase.Data = commandeCompteurResponse;
                responseBase.IsSuccess = true;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Commande Compteur Introuvable " + ex.Message;
                return responseBase;
            }
        }
    }
}
