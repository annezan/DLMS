using DLMS_BUSINESS.AssociationKeyDomainBusiness.Mappers;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Queries;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;

namespace DLMS_BUSINESS.AssociationKeyDomainBusiness.Handlers.QueryHandlers
{
    public class GetAssociationKeyByTypeQueryHandler : IRequestHandler<GetAssociationKeyByTypeQuery, ResponseBase<List<AssociationKeyResponse>>>
    {
        private readonly IMediator _mediator;
        private readonly IAssociationKeyQueryRepository _AssociationKeyQueryRepository;
        private readonly IAuthorizationService _authService; // Added

        public GetAssociationKeyByTypeQueryHandler(
            IMediator mediator,
            IAssociationKeyQueryRepository AssociationKeyQueryRepository,
            IAuthorizationService authService) // Added
        {
            _mediator = mediator;
            _AssociationKeyQueryRepository = AssociationKeyQueryRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<AssociationKeyResponse>>> Handle(GetAssociationKeyByTypeQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<AssociationKeyResponse>> responseBase = new ResponseBase<List<AssociationKeyResponse>>();

            try
            {
                // Vérifier l'accès au compteur
                var hasAccess = await _authService.UserHasAccessToCompteurByCompteurIdAsync(request.UserId, request.CompteurId);
                if (!hasAccess)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas accès à ce compteur.";
                    return responseBase;
                }

                var AssociationKey = await _AssociationKeyQueryRepository.GetAssociationKeyByType(request.Type, request.CompteurId);

                if (AssociationKey == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "AssociationKey Introuvable";
                    return responseBase;
                }
                
                responseBase.Data = AssociationKeyMapper.Mapper.Map<List<AssociationKeyResponse>>(AssociationKey);
                responseBase.IsSuccess = true;

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "AssociationKey Introuvable";
                return responseBase;
            }
        }
    }
}
