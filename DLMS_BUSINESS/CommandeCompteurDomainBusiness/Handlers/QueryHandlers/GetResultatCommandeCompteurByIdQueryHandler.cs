using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Queries;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetResultatCommandeCompteurByIdQueryHandler : IRequestHandler<GetResultatCommandeCompteurByIdQuery, ResponseBase<ResultatCommandeCompteurResponse>>
    {
        private readonly IResultatCommandeCompteurQueryRepository _resultatCommandeCompteurQueryRepository;
        private readonly IAuthorizationService _authService;

        public GetResultatCommandeCompteurByIdQueryHandler(
            IResultatCommandeCompteurQueryRepository resultatCommandeCompteurQueryRepository,
            IAuthorizationService authService)
        {
            _resultatCommandeCompteurQueryRepository = resultatCommandeCompteurQueryRepository;
            _authService = authService;
        }

        public async Task<ResponseBase<ResultatCommandeCompteurResponse>> Handle(GetResultatCommandeCompteurByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<ResultatCommandeCompteurResponse> responseBase = new ResponseBase<ResultatCommandeCompteurResponse>();
            try
            {
                var resultat = await _resultatCommandeCompteurQueryRepository.GetResultatCommandeCompteurById(request.Id);
                if (resultat == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Résultat Introuvable";
                    return responseBase;
                }

                var resultatResponse = ResultatCommandeCompteurMapper.Mapper.Map<ResultatCommandeCompteurResponse>(resultat);

                responseBase.Data = resultatResponse;
                responseBase.IsSuccess = true;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Résultat Introuvable : " + ex.Message;
                return responseBase;
            }
        }
    }
}
