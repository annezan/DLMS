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
    public class UsersAddCommandHandler : IRequestHandler<UsersAddCommand, ResponseBase<UsersResponse>>
    {

        private readonly IUsersCommandRepository _utilisateurCommandRepository;
        private readonly IUsersQueryRepository _utilisateurQueryRepository;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public UsersAddCommandHandler(IUsersCommandRepository utilisateurCommandRepository,
           IUsersQueryRepository utilisateurQueryRepository, IMediator mediator, IEmailService emailService)
        {
            _utilisateurCommandRepository = utilisateurCommandRepository;
            _utilisateurQueryRepository = utilisateurQueryRepository;
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(UsersAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> responseBase = new ResponseBase<UsersResponse>();

            var userExist = await _mediator.Send(new GetUsersByEmailQuery(request.Email));

            if (userExist.Data != null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Cet identifiant ({request.Email}) est déjà utilisé";
                return responseBase;
            }

            //2- Verifier que l'id Role existe
            var userRole = await _mediator.Send(new GetRoleByIdQuery(request.RoleId));
            if (userRole.Data == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Ce rôle n'existe pas dans le système";
                return responseBase;
            }


            var utilisateurEntity = UsersMapper.Mapper.Map<User>(request);
            if (utilisateurEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            try
            {
                string basePart = request.Nom.Substring(0, Math.Min(3, request.Nom.Length)).ToUpper();
                string datePart = request.DateNaissance.ToString("yyyyMMdd");
                var motDePasseGenerer = basePart + "@" + datePart;

                utilisateurEntity.MotDePasse = BCryptHelper.CryptPassword(motDePasseGenerer);
                utilisateurEntity.MustChangePassword = true;

                var userCreate = await _utilisateurCommandRepository.AddAsync(utilisateurEntity);

                // Recharger l'utilisateur avec toutes ses relations (Role et Poste)
                var userWithRelations = await _utilisateurQueryRepository.GetUsersById(userCreate.Id);

                // Envoyer l'email de bienvenue avec le mot de passe temporaire
                try
                {
                    var emailSent = await _emailService.SendPasswordNotificationAsync(
                        userWithRelations.Email,
                        userWithRelations.Nom + " " + userWithRelations.Prenoms,
                        motDePasseGenerer
                    );

                    if (!emailSent)
                    {
                        // L'email n'a pas pu être envoyé mais l'utilisateur est créé
                        Console.WriteLine($"Échec de l'envoi de l'email à {userWithRelations.Email}");
                    }
                }
                catch (Exception emailEx)
                {
                    // Logger l'erreur mais ne pas échouer la création de l'utilisateur
                    Console.WriteLine($"Erreur lors de l'envoi de l'email: {emailEx.Message}");
                }

                responseBase.Data = UsersMapper.Mapper.Map<UsersResponse>(userWithRelations);
                responseBase.Message = $"Votre mot de passe est {motDePasseGenerer}. Vous devez le changer à la première authentification.";

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
