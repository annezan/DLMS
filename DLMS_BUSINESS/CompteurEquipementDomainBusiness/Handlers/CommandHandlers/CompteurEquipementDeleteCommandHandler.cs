using DLMS_BUSINESS.CompteurEquipementDomainBusiness.Mappers;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Commands;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Commands;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CompteurEquipementDomainBusiness.Handlers.CommandHandlers
{
    public class CompteurEquipementDeleteCommandHandler : IRequestHandler<CompteurEquipementDeleteCommandList, ResponseBase<List<CompteurEquipementResponse>>>
    {
        private readonly ICompteurEquipementCommandRepository _CompteurEquipementCommandRepository;
        private readonly IAuthorizationService _authService; // Added
        private readonly IMediator _mediator;

        public CompteurEquipementDeleteCommandHandler(
            ICompteurEquipementCommandRepository CompteurEquipementCommandRepository,
            IAuthorizationService authService, // Added
            IMediator mediator)
        {
            _mediator = mediator;
            _CompteurEquipementCommandRepository = CompteurEquipementCommandRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<CompteurEquipementResponse>>> Handle(CompteurEquipementDeleteCommandList request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CompteurEquipementResponse>> responseBase = new ResponseBase<List<CompteurEquipementResponse>>();

            try
            {
                // Vérifier la permission de supprimer des associations Compteur-Equipement
                foreach (var command in request.Commands)
                {
                    var hasPermission = await _authService.UserHasPermissionAsync(command.UserId, "DELETE_COMPTEUR_EQUIPEMENT");
                    if (!hasPermission)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Vous n'avez pas la permission de supprimer des associations Compteur-Equipement.";
                        return responseBase;
                    }
                }

                // Vérifier l'accès à tous les équipements de la liste
                foreach (var command in request.Commands)
                {
                    var hasAccess = await _authService.UserHasAccessToEquipementAsync(command.UserId, command.EquipementId);
                    if (!hasAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = $"Vous n'avez pas accès à l'équipement {command.EquipementId}.";
                        return responseBase;
                    }
                }

                var CompteurEquipementEntity = CompteurEquipementMapper.Mapper.Map<List<CompteurEquipement>>(request.Commands);

                var newCompteurEquipement = await _CompteurEquipementCommandRepository.DeleteCompteurEquipement(CompteurEquipementEntity);
                if (newCompteurEquipement == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Erreur lors de la suppression des associations Compteur-Equipement";
                    return responseBase;
                }

                responseBase.Data = CompteurEquipementMapper.Mapper.Map<List<CompteurEquipementResponse>>(newCompteurEquipement);
                responseBase.IsSuccess = true;
                responseBase.Message = "Associations Compteur-Equipement supprimées avec succès";
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
