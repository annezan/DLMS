using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Queries;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetCommandeCompteursQueryHandler : IRequestHandler<GetCommandeCompteursQuery, ResponseBase<List<CommandeCompteurResponse>>>
    {
        private readonly ICommandeCompteurQueryRepository _commandeCompteurQueryRepository;
        private readonly IAuthorizationService _authService; // Added

        public GetCommandeCompteursQueryHandler(
            ICommandeCompteurQueryRepository commandeCompteurQueryRepository,
            IAuthorizationService authService) // Added
        {
            _commandeCompteurQueryRepository = commandeCompteurQueryRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<CommandeCompteurResponse>>> Handle(GetCommandeCompteursQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CommandeCompteurResponse>> responseBase = new ResponseBase<List<CommandeCompteurResponse>>();
            try
            {
                var commandeCompteurs = await _commandeCompteurQueryRepository.GetCommandeCompteurs();
                if (commandeCompteurs == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Commande Compteur Introuvable";
                    return responseBase;
                }

                var commandeCompteursResponse = CommandeCompteurMapper.Mapper.Map<List<CommandeCompteurResponse>>(commandeCompteurs);

                // Filtrer par poste si l'utilisateur a un poste assigné
                var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var commandeCompteursAccessibles = new List<CommandeCompteurResponse>();

                    // Vérification double : accès à la commande ET au compteur
                    foreach (var cc in commandeCompteursResponse)
                    {
                        var hasAccessToCommande = await _authService.UserHasAccessToCommandeAsync(request.UserId, cc.CommandeId);
                        var hasAccessToCompteur = await _authService.UserHasAccessToCompteurAsync(request.UserId, cc.CompteurId);

                        if (hasAccessToCommande && hasAccessToCompteur)
                        {
                            commandeCompteursAccessibles.Add(cc);
                        }
                    }

                    responseBase.Data = commandeCompteursAccessibles;
                }
                else
                {
                    responseBase.Data = commandeCompteursResponse;
                }

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
