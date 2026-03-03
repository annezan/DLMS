using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Queries;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetResultatCommandeCompteursByCommandeCompteurIdQueryHandler : IRequestHandler<GetResultatCommandeCompteursByCommandeCompteurIdQuery, ResponseBase<List<ResultatCommandeCompteurResponse>>>
    {
        private readonly IResultatCommandeCompteurQueryRepository _resultatCommandeCompteurQueryRepository;
        private readonly IAuthorizationService _authService;

        public GetResultatCommandeCompteursByCommandeCompteurIdQueryHandler(
            IResultatCommandeCompteurQueryRepository resultatCommandeCompteurQueryRepository,
            IAuthorizationService authService)
        {
            _resultatCommandeCompteurQueryRepository = resultatCommandeCompteurQueryRepository;
            _authService = authService;
        }

        public async Task<ResponseBase<List<ResultatCommandeCompteurResponse>>> Handle(GetResultatCommandeCompteursByCommandeCompteurIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<ResultatCommandeCompteurResponse>> responseBase = new ResponseBase<List<ResultatCommandeCompteurResponse>>();
            try
            {
                var resultats = await _resultatCommandeCompteurQueryRepository.GetResultatsByCommandeCompteurId(request.CommandeCompteurId);
                
                if (resultats == null || !resultats.Any())
                {
                    responseBase.IsSuccess = true;
                    responseBase.Message = "Aucun résultat trouvé pour cette commande compteur";
                    responseBase.Data = new List<ResultatCommandeCompteurResponse>();
                    return responseBase;
                }

                var resultatsResponse = resultats
                    .Select(r => ResultatCommandeCompteurMapper.Mapper.Map<ResultatCommandeCompteurResponse>(r))
                    .ToList();

                responseBase.Data = resultatsResponse;
                responseBase.IsSuccess = true;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Erreur lors de la récupération des résultats : " + ex.Message;
                return responseBase;
            }
        }
    }
}
