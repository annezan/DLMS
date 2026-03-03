using DLMS_BUSINESS.CommandeDomainBusiness.Mappers;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Queries;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeDomainBusiness.Handlers.QueryHandlers
{
    public class GetCommandesQueryHandler : IRequestHandler<GetCommandesQuery, ResponseBase<List<CommandeResponse>>>
    {
        private readonly ICommandeQueryRepository _CommandeQueryRepository;
        private readonly IAuthorizationService _authorizationService;

        public GetCommandesQueryHandler(
            ICommandeQueryRepository CommandeQueryRepository,
            IAuthorizationService authorizationService)
        {
            _CommandeQueryRepository = CommandeQueryRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<List<CommandeResponse>>> Handle(GetCommandesQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CommandeResponse>> responseBase = new ResponseBase<List<CommandeResponse>>();
            try
            {
                var commandes = await _CommandeQueryRepository.GetCommandes();
                if (commandes == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Commandes introuvables";
                    return responseBase;
                }

                // Mapper les commandes
                var commandesResponse = CommandeMapper.Mapper.Map<List<CommandeResponse>>(commandes);

                // Filtrer par poste si l'utilisateur a un poste assigné
                var hasPosteAssigne = await _authorizationService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    // Filtrer les commandes accessibles à l'utilisateur
                    var commandesAccessibles = new List<CommandeResponse>();

                    foreach (var commande in commandesResponse)
                    {
                        var hasAccess = await _authorizationService.UserHasAccessToCommandeAsync(request.UserId, commande.Id);
                        if (hasAccess)
                        {
                            commandesAccessibles.Add(commande);
                        }
                    }

                    responseBase.Data = commandesAccessibles;
                }
                else
                {
                    // Pas de filtrage si l'utilisateur n'a pas de poste assigné
                    responseBase.Data = commandesResponse;
                }

                responseBase.IsSuccess = true;
                responseBase.Message = $"{responseBase.Data.Count} commande(s) trouvée(s)";
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération des commandes : {ex.Message}";
                return responseBase;
            }
        }
    }
}
