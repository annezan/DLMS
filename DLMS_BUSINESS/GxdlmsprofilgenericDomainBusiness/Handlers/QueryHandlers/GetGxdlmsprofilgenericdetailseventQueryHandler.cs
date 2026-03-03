using DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Mappers;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_BUSINESS.GxdlmsprofilgenericdetailseventDomainBusiness.Mappers;

namespace DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Handlers.QueryHandlers
{
    public class GetGxdlmsprofilgenericdetailseventQueryHandler : IRequestHandler<GetGxdlmsprofilgenericdetailseventQuery, ResponseBase<List<GxdlmsprofilgenericdetailseventResponse>>>
    {
                                                                                                                                                        
        private readonly IMediator _mediator;
        private readonly IGxdlmsprofilgenericdetailseventQueryRepository _GxdlmsprofilgenericdetailseventQueryRepository;
        private readonly IAuthorizationService _authorizationService;

        public GetGxdlmsprofilgenericdetailseventQueryHandler(
            IMediator mediator, 
            IGxdlmsprofilgenericdetailseventQueryRepository GxdlmsprofilgenericdetailseventQueryRepository,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _GxdlmsprofilgenericdetailseventQueryRepository = GxdlmsprofilgenericdetailseventQueryRepository;
            _authorizationService = authorizationService;
        }

        public async Task<ResponseBase<List<GxdlmsprofilgenericdetailseventResponse>>> Handle(GetGxdlmsprofilgenericdetailseventQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<GxdlmsprofilgenericdetailseventResponse>> responseBase = new ResponseBase<List<GxdlmsprofilgenericdetailseventResponse>>();

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

                // Récupérer les événements du profil générique
                var Gxdlmsprofilgenericdetailsevent = await _GxdlmsprofilgenericdetailseventQueryRepository.GetProfilgenericdetailseventByStatus(request.NumeroCompteur);

                if (Gxdlmsprofilgenericdetailsevent == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Gxdlmsprofilgenericdetailsevent Introuvable ou désactivé";
                    return responseBase;
                }

                responseBase.Data = GxdlmsprofilgenericdetailseventMapper.Mapper.Map<List<GxdlmsprofilgenericdetailseventResponse>>(Gxdlmsprofilgenericdetailsevent);
                responseBase.IsSuccess = true;
                responseBase.Message = "Événements du profil générique récupérés avec succès";

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération des événements : {ex.Message}";
                return responseBase;
            }
            
        }
    }

}
