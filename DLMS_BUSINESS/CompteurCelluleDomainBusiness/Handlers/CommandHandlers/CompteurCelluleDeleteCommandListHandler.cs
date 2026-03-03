using DLMS_BUSINESS.CompteurCelluleDomainBusiness.Mappers;
using DLMS_DAL.CompteurCelluleDomainDal.Repositories.Commands;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Commands;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CompteurCelluleDomainBusiness.Handlers.CommandHandlers
{
    public class CompteurCelluleDeleteCommandListHandler : IRequestHandler<CompteurCelluleDeleteCommandList, ResponseBase<List<CompteurCelluleResponse>>>
    {
        private readonly ICompteurCelluleCommandRepository _repository;
        private readonly IAuthorizationService _authService;

        public CompteurCelluleDeleteCommandListHandler(
            ICompteurCelluleCommandRepository repository,
            IAuthorizationService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<ResponseBase<List<CompteurCelluleResponse>>> Handle(CompteurCelluleDeleteCommandList request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CompteurCelluleResponse>> responseBase = new ResponseBase<List<CompteurCelluleResponse>>();

            try
            {
                // Vérifier la permission de supprimer des associations Compteur-Cellule
                foreach (var command in request.Commands)
                {
                    var hasPermission = await _authService.UserHasPermissionAsync(command.UserId, "DELETE_COMPTEUR_CELLULE");
                    if (!hasPermission)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Vous n'avez pas la permission de supprimer des associations Compteur-Cellule.";
                        return responseBase;
                    }
                }

                // Vérifier l'accès à tous les compteurs de la liste
                foreach (var command in request.Commands)
                {
                    var hasAccess = await _authService.UserHasAccessToCompteurAsync(command.UserId, command.CompteurId);
                    if (!hasAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = $"Vous n'avez pas accès au compteur {command.CompteurId}.";
                        return responseBase;
                    }
                }

                // Vérifier l'accès à toutes les cellules de la liste
                foreach (var command in request.Commands)
                {
                    var hasCelluleAccess = await _authService.UserHasAccessToCelluleAsync(command.UserId, command.CelluleId);
                    if (!hasCelluleAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = $"Vous n'avez pas accès à la cellule {command.CelluleId}.";
                        return responseBase;
                    }
                }

                var entities = CompteurCelluleMapper.Mapper.Map<List<CompteurCellule>>(request.Commands);

                var results = await _repository.DeleteCompteurCelluleList(entities);
                if (results == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Erreur lors de la suppression des associations Compteur-Cellule";
                    return responseBase;
                }

                responseBase.Data = CompteurCelluleMapper.Mapper.Map<List<CompteurCelluleResponse>>(results);
                responseBase.IsSuccess = true;
                responseBase.Message = "Associations Compteur-Cellule supprimées avec succès";
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
