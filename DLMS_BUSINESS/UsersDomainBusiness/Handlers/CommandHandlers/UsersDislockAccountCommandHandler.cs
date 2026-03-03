using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
	public class UsersDislockAccountCommandHandler : IRequestHandler<UsersDislockAccountCommand, ResponseBase<string>>
    {
        private readonly IMediator _mediator;
        private readonly IUsersCommandRepository _commandRepository;
        private readonly IUsersQueryRepository _queryRepository;


        public UsersDislockAccountCommandHandler(IMediator mediator, IUsersCommandRepository commandRepository, IUsersQueryRepository queryRepository)
		{
            _mediator = mediator;
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
        }

        public async Task<ResponseBase<string>> Handle(UsersDislockAccountCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> response = new();

            // 1 - rechercher l'utilisateur
            var user = await _queryRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "L'utilisateur n'existe pas dans le système avec cet id : " + request.Id;
                return response;
            }

            // 2 - verifier que son compte est verrouillé
            if (!user.IsLocked)
            {
                response.IsSuccess = false;
                response.Message = "Le compte (" + user.Email + ") n'est pas vérrouillé.";
                return response;
            }

            // 3 - Debloquer le compte
            user.IsLocked = false;
            user.FailedLoginAttempts = 0;
            await _commandRepository.UpdateAsync(user);

            response.Message = "Le compte (" + user.Email + ") est devérrouillé.";
            return response;
        }
    }
}

