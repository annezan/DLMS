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
    public class GetAssociationKeyExistingQueryHandler : IRequestHandler<GetAssociationKeyExistingQuery, ResponseBase<AssociationKeyResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IAssociationKeyQueryRepository _AssociationKeyQueryRepository;
        private readonly IAuthorizationService _authService; // Added

        public GetAssociationKeyExistingQueryHandler(
            IMediator mediator,
            IAssociationKeyQueryRepository AssociationKeyQueryRepository,
            IAuthorizationService authService) // Added
        {
            _mediator = mediator;
            _AssociationKeyQueryRepository = AssociationKeyQueryRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<AssociationKeyResponse>> Handle(GetAssociationKeyExistingQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<AssociationKeyResponse> responseBase = new ResponseBase<AssociationKeyResponse>();

            try
            {
                // Vérifier l'accès au compteur
                var hasAccess = await _authService.UserHasAccessToCompteurByCompteurIdAsync(request.UserId, request.Compteurid);
                if (!hasAccess)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas accès à ce compteur.";
                    return responseBase;
                }

                var AssociationKey = await _AssociationKeyQueryRepository.GetAssociationKeyExisting(request.Keyvalue, request.Compteurid);

                if (AssociationKey == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "AssociationKey Introuvable";
                    return responseBase;
                }

                responseBase.Data = AssociationKeyMapper.Mapper.Map<AssociationKeyResponse>(AssociationKey);
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
