using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersAssignPosteCommandHandler : IRequestHandler<UsersAssignPosteCommand, ResponseBase<UsersResponse>>
    {
        private readonly IUsersCommandRepository _userCommandRepository;
        private readonly IUsersQueryRepository _userQueryRepository;
        private readonly IPosteQueryRepository _posteQueryRepository;
        private readonly IMediator _mediator;

        public UsersAssignPosteCommandHandler(
            IUsersCommandRepository userCommandRepository,
            IUsersQueryRepository userQueryRepository,
            IPosteQueryRepository posteQueryRepository,
            IMediator mediator)
        {
            _userCommandRepository = userCommandRepository;
            _userQueryRepository = userQueryRepository;
            _posteQueryRepository = posteQueryRepository;
            _mediator = mediator;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(UsersAssignPosteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> response = new ResponseBase<UsersResponse>();

            try
            {
                // Vérifier que l'utilisateur existe
                var user = await _userQueryRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Utilisateur introuvable";
                    return response;
                }

                // Si un PosteId est fourni, vérifier qu'il existe
                if (request.PosteId.HasValue)
                {
                    var poste = await _posteQueryRepository.GetByIdAsync(request.PosteId.Value);
                    
                    if (poste == null)
                    {
                        response.IsSuccess = false;
                        response.Message = $"Le poste {request.PosteId.Value} n'existe pas";
                        return response;
                    }
                }

                // Assigner ou retirer le poste
                user.PosteId = request.PosteId;
                await _userCommandRepository.UpdateAsync(user);

                response.Data = UsersMapper.Mapper.Map<UsersResponse>(user);
                response.Message = request.PosteId.HasValue
                    ? $"Poste {request.PosteId.Value} assigné avec succès à l'utilisateur"
                    : "Poste retiré avec succès de l'utilisateur";
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Erreur lors de l'assignation du poste : {ex.Message}";
                return response;
            }
        }
    }
}

