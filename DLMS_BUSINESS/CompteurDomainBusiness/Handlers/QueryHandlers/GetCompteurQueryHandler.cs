using DLMS_BUSINESS.CompteurDomainBusiness.Mappers;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Queries;
using DLMS_MODELS.CompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetCompteurQueryHandler : IRequestHandler<GetCompteurQuery, ResponseBase<List<CompteurResponse>>>
    {
        private readonly ICompteurQueryRepository _CompteurQueryRepository;
        private readonly IAuthorizationService _authorizationService;

        public GetCompteurQueryHandler(
            ICompteurQueryRepository CompteurQueryRepository,
            IAuthorizationService authorizationService)
        {
            _CompteurQueryRepository = CompteurQueryRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<List<CompteurResponse>>> Handle(GetCompteurQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CompteurResponse>> responseBase = new ResponseBase<List<CompteurResponse>>();
            
            try
            {
                var compteurs = await _CompteurQueryRepository.GetCompteur();
                if (compteurs == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Compteurs introuvables";
                    return responseBase;
                }

                var compteursResponse = CompteurMapper.Mapper.Map<List<CompteurResponse>>(compteurs);

                // Filtrer par poste si nécessaire
                var hasPosteAssigne = await _authorizationService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var compteursAccessibles = new List<CompteurResponse>();

                    foreach (var compteur in compteursResponse)
                    {
                        var hasAccess = await _authorizationService.UserHasAccessToCompteurAsync(request.UserId, compteur.Id);
                        if (hasAccess)
                        {
                            compteursAccessibles.Add(compteur);
                        }
                    }

                    responseBase.Data = compteursAccessibles;
                }
                else
                {
                    responseBase.Data = compteursResponse;
                }

                responseBase.IsSuccess = true;
                responseBase.Message = $"{responseBase.Data.Count} compteur(s) trouvé(s)";
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message =
                    $"Erreur lors de la récupération des compteurs : {ex.Message} " +
                    $"| Inner: {ex.InnerException?.Message}";
                return responseBase;
            }

        }
    }
}
