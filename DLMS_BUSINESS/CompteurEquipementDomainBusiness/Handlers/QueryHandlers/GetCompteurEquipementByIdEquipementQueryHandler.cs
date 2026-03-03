using DLMS_BUSINESS.CompteurDomainBusiness.Mappers;
using DLMS_BUSINESS.CompteurEquipementDomainBusiness.Handlers.CommandHandlers;
using DLMS_BUSINESS.CompteurEquipementDomainBusiness.Mappers;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Queries;
using DLMS_MODELS.CompteurDomain.Responses;
using DLMS_MODELS.CompteurEquipementDomain.Queries;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CompteurDomainBusiness.Handlers.QueryHandlers
{
    public class GetCompteurEquipementByIdEquipementHandler : IRequestHandler<GetCompteurEquipementByIdEquipementQuery, ResponseBase<List<CompteurEquipementResponse>>>
    {
        private readonly ICompteurEquipementQueryRepository _CompteurQueryRepository;
        private readonly IAuthorizationService _authService; // Added

        public GetCompteurEquipementByIdEquipementHandler(
            ICompteurEquipementQueryRepository CompteurQueryRepository,
            IAuthorizationService authService) // Added
        {
            _CompteurQueryRepository = CompteurQueryRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<CompteurEquipementResponse>>> Handle(GetCompteurEquipementByIdEquipementQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CompteurEquipementResponse>> responseBase = new ResponseBase<List<CompteurEquipementResponse>>();
            try
            {
                // Vérifier la permission de consulter les associations Compteur-Equipement
                var hasPermission = await _authService.UserHasPermissionAsync(request.UserId, "VIEW_COMPTEUR_EQUIPEMENT");
                if (!hasPermission)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas la permission de consulter les associations Compteur-Equipement.";
                    return responseBase;
                }

                // Vérifier l'accès à l'équipement
                var hasAccess = await _authService.UserHasAccessToEquipementAsync(request.UserId, request.Id);
                if (!hasAccess)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas accès à cet équipement.";
                    return responseBase;
                }

                var Compteur = await _CompteurQueryRepository.GetCompteurEquipementByIdEquipement(request.Id);
                if (Compteur == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Equipement Introuvable";
                    return responseBase;
                }
                responseBase.Data = CompteurEquipementMapper.Mapper.Map<List<CompteurEquipementResponse>>(Compteur);
                responseBase.IsSuccess = true;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Equipement Introuvable";
                return responseBase;
            }
        }
    }
}
