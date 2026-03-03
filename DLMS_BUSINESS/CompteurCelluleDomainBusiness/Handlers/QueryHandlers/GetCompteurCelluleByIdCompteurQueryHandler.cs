using DLMS_BUSINESS.CompteurCelluleDomainBusiness.Mappers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Queries;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using DLMS_DAL.CompteurCelluleDomainDal.Repositories.Queries;
using DLMS_DAL.Services; // Added for authorization
using MediatR;

namespace DLMS_BUSINESS.CompteurCelluleDomainBusiness.Handlers.QueryHandlers
{
    public class GetCompteurCelluleByIdCompteurQueryHandler : IRequestHandler<GetCompteurCelluleByIdCompteurQuery, ResponseBase<List<CompteurCelluleResponse>>>
    {
        private readonly ICompteurCelluleQueryRepository _repository;
        private readonly IAuthorizationService _authService; // Added

        public GetCompteurCelluleByIdCompteurQueryHandler(ICompteurCelluleQueryRepository repository, IAuthorizationService authService) // Added
        {
            _repository = repository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<CompteurCelluleResponse>>> Handle(GetCompteurCelluleByIdCompteurQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CompteurCelluleResponse>> responseBase = new ResponseBase<List<CompteurCelluleResponse>>();
            try
            {
                // Vérifier la permission de consulter les associations Compteur-Cellule
                var hasPermission = await _authService.UserHasPermissionAsync(request.UserId, "VIEW_COMPTEUR_CELLULE");
                if (!hasPermission)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas la permission de consulter les associations Compteur-Cellule.";
                    return responseBase;
                }

                // Vérifier l'accès au compteur
                var hasAccess = await _authService.UserHasAccessToCompteurAsync(request.UserId, request.Id);
                if (!hasAccess)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Vous n'avez pas accès à ce compteur.";
                    return responseBase;
                }

                var entities = await _repository.GetByIdCompteurAsync(request.Id);
                if (entities == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "CompteurCellule records not found";
                    return responseBase;
                }
                
                responseBase.Data = CompteurCelluleMapper.Mapper.Map<List<CompteurCelluleResponse>>(entities);
                responseBase.IsSuccess = true;
                responseBase.Message = "CompteurCellule records retrieved successfully";
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = ex.Message;
                return responseBase;
            }
        }
    }
}
