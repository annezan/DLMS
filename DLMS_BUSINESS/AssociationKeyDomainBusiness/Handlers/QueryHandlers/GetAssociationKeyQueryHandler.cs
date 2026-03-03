using DLMS_BUSINESS.AssociationKeyDomainBusiness.Mappers;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Queries;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.AssociationKeyDomainBusiness.Handlers.QueryHandlers
{
    public class GetAssociationKeyQueryHandler : IRequestHandler<GetAssociationKeyQuery, ResponseBase<List<AssociationKeyResponse>>>
    {
        private readonly IAssociationKeyQueryRepository _AssociationKeyQueryRepository;
        private readonly IAuthorizationService _authService; // Added

        public GetAssociationKeyQueryHandler(
            IAssociationKeyQueryRepository AssociationKeyQueryRepository,
            IAuthorizationService authService) // Added
        {
            _AssociationKeyQueryRepository = AssociationKeyQueryRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<AssociationKeyResponse>>> Handle(GetAssociationKeyQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<AssociationKeyResponse>> responseBase = new ResponseBase<List<AssociationKeyResponse>>();
            try
            {
                var associationKeys = await _AssociationKeyQueryRepository.GetAssociationKey();
                var associationKeysResponse = AssociationKeyMapper.Mapper.Map<List<AssociationKeyResponse>>(associationKeys);

                // Filtrer par poste si l'utilisateur a un poste assigné
                var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(request.UserId);

                if (hasPosteAssigne)
                {
                    var associationKeysAccessibles = new List<AssociationKeyResponse>();

                    foreach (var key in associationKeysResponse)
                    {
                        var hasAccess = await _authService.UserHasAccessToCompteurByCompteurIdAsync(request.UserId, key.CompteurId);
                        if (hasAccess)
                        {
                            associationKeysAccessibles.Add(key);
                        }
                    }

                    responseBase.Data = associationKeysAccessibles;
                }
                else
                {
                    responseBase.Data = associationKeysResponse;
                }

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
