using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersLogoutTokenCommandHandler : IRequestHandler<UsersLogoutCommand, ResponseBase<string>>
    {
        private readonly IUsersTokenQueryRepository _queryRepository;
        private readonly IUsersTokenCommandRepository _commandRepository;
        private readonly IMediator _mediator;

        public UsersLogoutTokenCommandHandler(
            IUsersTokenCommandRepository commandRepository,
            IUsersTokenQueryRepository tokenUtilisateurCommandRepository,
            IMediator mediator
            )
        {
            _commandRepository = commandRepository;
            _queryRepository = tokenUtilisateurCommandRepository;
            _mediator = mediator;

        }

        public async Task<ResponseBase<string>> Handle(UsersLogoutCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> response = new ResponseBase<string>();

            try
            {
                var tokenEntity = await _queryRepository.GetTokensByUserIdAsync(request.UserId);

                if (tokenEntity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid Token";
                    return response;
                }

                await _commandRepository.DeleteAsync(tokenEntity);

                response.Data = "Utilisateur déconnecté avec succès";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong!";
                return response;
            }
        }
    }
}
