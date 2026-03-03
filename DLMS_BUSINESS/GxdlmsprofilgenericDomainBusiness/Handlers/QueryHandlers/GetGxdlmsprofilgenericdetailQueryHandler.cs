using DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Mappers;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Responses;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Handlers.QueryHandlers
{
    public class GetGxdlmsprofilgenericdetailQueryHandler : IRequestHandler<GetGxdlmsprofilgenericdetailQuery, ResponseBase<List<GxdlmsprofilgenericdetailResponse>>>
    {
        private readonly IMediator _mediator;
        private readonly IGxdlmsprofilgenericdetailQueryRepository _GxdlmsprofilgenericdetailQueryRepository;
        private readonly IAuthorizationService _authorizationService;

        public GetGxdlmsprofilgenericdetailQueryHandler(
            IMediator mediator, 
            IGxdlmsprofilgenericdetailQueryRepository GxdlmsprofilgenericdetailQueryRepository,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _GxdlmsprofilgenericdetailQueryRepository = GxdlmsprofilgenericdetailQueryRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<List<GxdlmsprofilgenericdetailResponse>>> Handle(GetGxdlmsprofilgenericdetailQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<GxdlmsprofilgenericdetailResponse>> responseBase = new ResponseBase<List<GxdlmsprofilgenericdetailResponse>>();
            try
            {
                // Vérifier l'accès au compteur si l'utilisateur a un poste assigné
                var hasPosteAssigne = await _authorizationService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var hasAccess = await _authorizationService.UserHasAccessToCompteurByCompteurIdAsync(request.UserId, request.NumeroCompteur);
                    if (!hasAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Accès refusé : Ce compteur n'appartient pas à votre poste.";
                        return responseBase;
                    }
                }

                // Récupérer les détails du profil générique
                var Gxdlmsprofilgenericdetail = await _GxdlmsprofilgenericdetailQueryRepository.GetProfilgenericdetailByStatus(request.NumeroCompteur, request.GxdlmsprofilgenericId, request.DateEnr);

                if (Gxdlmsprofilgenericdetail == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Gxdlmsprofilgenericdetail Introuvable ou désactivé";
                    return responseBase;
                }

                responseBase.Data = GxdlmsprofilgenericdetailMapper.Mapper.Map<List<GxdlmsprofilgenericdetailResponse>>(Gxdlmsprofilgenericdetail);
                responseBase.IsSuccess = true;
                responseBase.Message = "Détails du profil générique récupérés avec succès";

                return responseBase;
            }
            catch (Exception ex) 
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération des détails : {ex.Message}";
                return responseBase;
            }
            
        }
    }

}
