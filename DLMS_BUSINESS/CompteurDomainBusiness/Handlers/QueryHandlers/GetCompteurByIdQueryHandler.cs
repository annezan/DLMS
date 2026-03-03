using DLMS_BUSINESS.CompteurDomainBusiness.Mappers;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Queries;
using DLMS_MODELS.CompteurDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetCompteurByTypeQueryHandler : IRequestHandler<GetCompteurByIdQuery, ResponseBase<CompteurResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ICompteurQueryRepository _CompteurQueryRepository;
        private readonly IAuthorizationService _authorizationService;

        public GetCompteurByTypeQueryHandler(
            IMediator mediator, 
            ICompteurQueryRepository CompteurQueryRepository,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _CompteurQueryRepository = CompteurQueryRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<CompteurResponse>> Handle(GetCompteurByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<CompteurResponse> responseBase = new ResponseBase<CompteurResponse>();

            try
            {
                // Vérifier l'accès si l'utilisateur a un poste assigné
                var hasPosteAssigne = await _authorizationService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var hasAccess = await _authorizationService.UserHasAccessToCompteurAsync(request.UserId, request.Id);
                    if (!hasAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Accès refusé : Ce compteur n'est pas installé dans un équipement de votre poste.";
                        return responseBase;
                    }
                }

                // Récupérer le compteur
                var compteur = await _CompteurQueryRepository.GetCompteurById(request.Id);
                
                if (compteur == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Compteur introuvable";
                    return responseBase;
                }

                responseBase.Data = CompteurMapper.Mapper.Map<CompteurResponse>(compteur);
                responseBase.IsSuccess = true;
                responseBase.Message = "Compteur récupéré avec succès";

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération du compteur : {ex.Message}";
                return responseBase;
            }
        }
    }
}
