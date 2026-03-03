using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.Helpers;
using DLMS_DAL.Services;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersForgotPasswordCommandHandler : IRequestHandler<UsersForgotPasswordCommand, ResponseBase<UsersResponse>>
    {
        private readonly IUsersCommandRepository _utilisateurCommandRepository;
        private readonly IUsersQueryRepository _utilisateurQueryRepository;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public UsersForgotPasswordCommandHandler(IUsersCommandRepository utilisateurCommandRepository,
           IUsersQueryRepository utilisateurQueryRepository, IMediator mediator, IEmailService emailService)
        {
            _utilisateurCommandRepository = utilisateurCommandRepository;
            _utilisateurQueryRepository = utilisateurQueryRepository;
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(UsersForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> responseBase = new ResponseBase<UsersResponse>();

            // Vérifier que l'utilisateur existe
            var userExist = await _mediator.Send(new GetUsersByEmailQuery(request.Email));

            if (userExist.Data == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Aucun compte trouvé avec l'adresse email {request.Email}";
                return responseBase;
            }

            try
            {
                var userResponse = userExist.Data;
                
                // Récupérer l'entité User pour la modifier
                var user = await _utilisateurQueryRepository.GetUsersById(userResponse.Id);
                if (user == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Utilisateur introuvable";
                    return responseBase;
                }
                
                // Générer un nouveau mot de passe temporaire
                string basePart = user.Nom.Substring(0, Math.Min(3, user.Nom.Length)).ToUpper();
                string datePart = DateTime.Now.ToString("yyyyMMdd");
                var motDePasseGenerer = basePart + "@" + datePart;

                // Mettre à jour le mot de passe de l'utilisateur
                user.MotDePasse = BCryptHelper.CryptPassword(motDePasseGenerer);
                user.MustChangePassword = true;
                user.FailedLoginAttempts = 0; // Réinitialiser les tentatives échouées
                user.IsLocked = false; // Déverrouiller le compte si nécessaire

                await _utilisateurCommandRepository.UpdateAsync(user);

                // Envoyer l'email avec le nouveau mot de passe
                try
                {
                    var emailSent = await _emailService.SendPasswordNotificationAsync(
                        user.Email,
                        user.Nom + " " + user.Prenoms,
                        motDePasseGenerer
                    );

                    if (!emailSent)
                    {
                        // L'email n'a pas pu être envoyé mais le mot de passe a été réinitialisé
                        responseBase.IsSuccess = false;
                        responseBase.Message = "Le mot de passe a été réinitialisé mais l'email n'a pas pu être envoyé. Contactez l'administrateur.";
                        return responseBase;
                    }
                }
                catch (Exception emailEx)
                {
                    // Logger l'erreur mais le mot de passe a été réinitialisé
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Le mot de passe a été réinitialisé mais une erreur est survenue lors de l'envoi de l'email. Contactez l'administrateur.";
                    return responseBase;
                }

                // Recharger l'utilisateur avec toutes ses relations (Role et Poste)
                var userWithRelations = await _utilisateurQueryRepository.GetUsersById(user.Id);
                responseBase.Data = UsersMapper.Mapper.Map<UsersResponse>(userWithRelations);
                responseBase.Message = "Un nouveau mot de passe temporaire a été envoyé à votre adresse email. Vous devez le changer à la prochaine connexion.";

                return responseBase;
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + exp.Message;
                return responseBase;
            }
        }
    }
}
