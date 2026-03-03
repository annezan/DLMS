using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.Helpers;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersChangePasswordCommandHandler : IRequestHandler<UsersChangePasswordCommand, ResponseBase<UsersResponse>>
    {
        private readonly IUsersCommandRepository _usersCommandRepository;
        private readonly IMediator _mediator;

        public UsersChangePasswordCommandHandler(IUsersCommandRepository usersCommandRepository, IMediator mediator)
        {
            _usersCommandRepository = usersCommandRepository;
            _mediator = mediator;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(UsersChangePasswordCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> response = new ResponseBase<UsersResponse>();

            // 1- Vérifier que les nouveaux mots de passe correspondent
            if (request.NouveauMotDePasse != request.ConfirmationNouveauMotDePasse)
            {
                response.IsSuccess = false;
                response.Message = "Le nouveau mot de passe et sa confirmation ne correspondent pas.";
                return response;
            }

            // 2- Vérifier que le nouveau mot de passe est différent de l'ancien
            if (request.AncienMotDePasse == request.NouveauMotDePasse)
            {
                response.IsSuccess = false;
                response.Message = "Le nouveau mot de passe doit être différent de l'ancien.";
                return response;
            }

            // 3- Récupérer l'utilisateur par email
            var userResponse = await _mediator.Send(new GetUsersByEmailQuery(request.Email));
            if (userResponse.Data == null)
            {
                response.IsSuccess = false;
                response.Message = "Aucun utilisateur trouvé avec cet email.";
                return response;
            }

            var user = await _mediator.Send(new GetUsersByIdQuery(userResponse.Data.Id));
            if (user.Data == null)
            {
                response.IsSuccess = false;
                response.Message = "Utilisateur introuvable.";
                return response;
            }

            // 4- Vérifier l'ancien mot de passe
            var userEntity = await _usersCommandRepository.Login(request.Email, request.AncienMotDePasse);
            if (userEntity == null)
            {
                response.IsSuccess = false;
                response.Message = "L'ancien mot de passe est incorrect.";
                return response;
            }

            try
            {
                // 5- Mettre à jour le mot de passe
                userEntity.MotDePasse = BCryptHelper.CryptPassword(request.NouveauMotDePasse);
                userEntity.MustChangePassword = false;

                await _usersCommandRepository.UpdateAsync(userEntity);

                response.IsSuccess = true;
                response.Message = "Mot de passe modifié avec succès.";
                response.Data = UsersMapper.Mapper.Map<UsersResponse>(userEntity);

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Une erreur est survenue lors du changement de mot de passe : {ex.Message}";
                return response;
            }
        }
    }
}

