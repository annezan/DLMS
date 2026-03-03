using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersRegisterCommandHandler : IRequestHandler<UsersRegisterCommand, ResponseBase<UsersResponse>>
    {
        private readonly IUsersCommandRepository _utilisateurCommandRepository;
        private readonly IMediator _mediator;

        public UsersRegisterCommandHandler(IUsersCommandRepository utilisateurCommandRepository,
            IMediator mediator)
        {
            _mediator = mediator;
            _utilisateurCommandRepository = utilisateurCommandRepository;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(UsersRegisterCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> responseBase = new ResponseBase<UsersResponse>();

            try
            {
                // 1- Vérifier que les mots de passe correspondent
                if (request.MotDePasse != request.ConfirmationMotDePasse)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Le mot de passe et sa confirmation ne correspondent pas.";
                    return responseBase;
                }

                // 2- Vérifier que l'email n'est pas déjà utilisé
                var userExist = await _mediator.Send(new GetUsersByEmailQuery(request.Email));

                if (userExist.Data != null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Cet identifiant est déjà utilisé";
                    return responseBase;
                }

                // 3- Vérifier que l'id Role existe
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

                var newUtilisateur = await _utilisateurCommandRepository.Register(utilisateurEntity);
                var customerResponse = UsersMapper.Mapper.Map<UsersResponse>(newUtilisateur);

                responseBase.Data = customerResponse;

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Something went wrong! " + ex.Message;
                return responseBase;
            }
}
    }

}
