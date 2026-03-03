using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersLoginCommandHandler : IRequestHandler<UsersLoginCommand, ResponseBase<TokenResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IUsersCommandRepository _utilisateurCommandRepository;
        private readonly IUsersQueryRepository _queryRepository;


        public UsersLoginCommandHandler(IMediator mediator, IUsersCommandRepository utilisateurCommandRepository, IUsersQueryRepository queryRepository)
        {
            _mediator = mediator;
            _utilisateurCommandRepository = utilisateurCommandRepository;
            _queryRepository = queryRepository;

        }

        public async Task<ResponseBase<TokenResponse>> Handle(UsersLoginCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<TokenResponse> response = new();

            var user = await _queryRepository.GetUsersByEmail(request.LoginId);

            if(user == null)
            {
                response.IsSuccess = false;
                response.Message = "L'utilisateur n'existe pas dans le système avec cet email : " + request.LoginId;
                return response;
            }

            // Vérifier si le compte est verrouillé
            if (user.IsLocked)
            {
                response.IsSuccess = false;
                response.Message = "Compte ("+ request.LoginId + ") verrouillé . Contactez un administrateur.";
                return response;
            }

            // Vérifier si le compte est à sa première connexion
            if (user.MustChangePassword)
            {
                response.IsSuccess = false;
                response.Message = "Vous êtes à votre première connexion. Veillez changer votre mot de passe";
                return response;
            }

            // Rechercher l'utilisateur par son email
            var userLogOn = await _utilisateurCommandRepository.Login(request.LoginId, request.Password);

            if (userLogOn == null)
            {
                user.FailedLoginAttempts++;

                var message = "Votre mot de passe est incorrect. ";

                if (user.FailedLoginAttempts >= 3) // Seuil de verrouillage
                {
                    user.IsLocked = true; // Verrouillage du compte
                    message += "Compte (" + request.LoginId + ") verrouillé. Contactez un administrateur.";
                }
                else
                {
                    message += " Nombre de tentative restant : " + (3-user.FailedLoginAttempts);
                }

                await _utilisateurCommandRepository.UpdateAsync(user);

                response.IsSuccess = false;
                response.Message = message;
                return response;
            }

            var authenticationResult = await _mediator.Send(new UsersAuthenticateCommand(user));
            if (authenticationResult != null && authenticationResult.Success)
            {
                response.Data = new TokenResponse()
                {
                    Token = authenticationResult.Token,
                    RefreshToken = authenticationResult.RefreshToken
                };


                if (user.FailedLoginAttempts > 0)
                {
                    // Réinitialiser les tentatives en cas de succès
                    user.FailedLoginAttempts = 0;
                    await _utilisateurCommandRepository.UpdateAsync(user);
                }
            }
            else
            {
                response.Message = "Something went wrong!";
                response.IsSuccess = false;
            }

            return response;

        }

    }

}
