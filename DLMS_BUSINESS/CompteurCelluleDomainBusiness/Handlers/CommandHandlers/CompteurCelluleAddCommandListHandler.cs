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
    public class CompteurCelluleAddCommandListHandler : IRequestHandler<CompteurCelluleAddCommandList, ResponseBase<List<CompteurCelluleResponse>>>
    {
        private readonly ICompteurCelluleCommandRepository _repository;
        private readonly IAuthorizationService _authService;
        private readonly IMediator _mediator;

        public CompteurCelluleAddCommandListHandler(
            ICompteurCelluleCommandRepository repository,
            IAuthorizationService authService,
            IMediator mediator)
        {
            _repository = repository;
            _authService = authService;
            _mediator = mediator;
        }

        public async Task<ResponseBase<List<CompteurCelluleResponse>>> Handle(CompteurCelluleAddCommandList request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CompteurCelluleResponse>> responseBase = new ResponseBase<List<CompteurCelluleResponse>>();

            try
            {
                // Vérifier la permission de créer des associations Compteur-Cellule
                foreach (var command in request.Commands)
                {
                    var hasPermission = await _authService.UserHasPermissionAsync(command.UserId, "CREATE_COMPTEUR_CELLULE");
                    if (!hasPermission)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Vous n'avez pas la permission de créer des associations Compteur-Cellule.";
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

                var entities = CompteurCelluleMapper.Mapper.Map<List<CompteurCellule>>(request.Commands);

                var newCompteurCellule = await _repository.AddCompteurCelluleList(entities);
                if (newCompteurCellule == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Erreur lors de la création des associations Compteur-Cellule";
                    return responseBase;
                }

                responseBase.Data = CompteurCelluleMapper.Mapper.Map<List<CompteurCelluleResponse>>(newCompteurCellule);
                responseBase.IsSuccess = true;
                responseBase.Message = "Associations Compteur-Cellule créées avec succès";
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
