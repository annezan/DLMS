using DLMS_BUSINESS.AssociationKeyDomainBusiness.Mappers;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Commands;
using DLMS_DAL.Services; // Added for authorization
using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Commands;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.AssociationKeyDomainBusiness.Handlers.CommandHandlers
{
    public class AssociationKeyAddCommandHandler : IRequestHandler<AssociationKeyAddCommandList, ResponseBase<List<AssociationKeyResponse>>>
    {
        private readonly IAssociationKeyCommandRepository _AssociationKeyCommandRepository;
        private readonly IAuthorizationService _authService; // Added
        private readonly IMediator _mediator;

        public AssociationKeyAddCommandHandler(
            IAssociationKeyCommandRepository AssociationKeyCommandRepository,
            IAuthorizationService authService, // Added
            IMediator mediator)
        {
            _mediator = mediator;
            _AssociationKeyCommandRepository = AssociationKeyCommandRepository;
            _authService = authService; // Added
        }

        public async Task<ResponseBase<List<AssociationKeyResponse>>> Handle(AssociationKeyAddCommandList request, CancellationToken cancellationToken)
        {
            ResponseBase<List<AssociationKeyResponse>> responseBase = new ResponseBase<List<AssociationKeyResponse>>();

            try
            {
                // Vérifier l'accès à tous les compteurs de la liste
                foreach (var command in request.Commands)
                {
                    var hasAccess = await _authService.UserHasAccessToCompteurByCompteurIdAsync(command.UserId, command.CompteurId);
                    if (!hasAccess)
                    {
                        responseBase.IsSuccess = false;
                        responseBase.Message = $"Vous n'avez pas accès au compteur {command.CompteurId}.";
                        return responseBase;
                    }
                }

                var AssociationKeyEntity = AssociationKeyMapper.Mapper.Map<List<AssociationKey>>(request.Commands);

                var newAssociationKey = await _AssociationKeyCommandRepository.AddAssociationKey(AssociationKeyEntity);
                if (newAssociationKey == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }
                
                responseBase.Data = AssociationKeyMapper.Mapper.Map<List<AssociationKeyResponse>>(newAssociationKey);
                responseBase.IsSuccess = true;

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }
        }
    }
}
