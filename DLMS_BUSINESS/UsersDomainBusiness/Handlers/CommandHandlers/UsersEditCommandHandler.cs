using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersEditCommandHandler : IRequestHandler<UsersEditCommand, ResponseBase<UsersResponse>>
    {

        private readonly IUsersCommandRepository _utilisateurCommandRepository;
        private readonly IUsersQueryRepository _utilisateurQueryRepository;
        private readonly IMediator _mediator;

        public UsersEditCommandHandler(IUsersCommandRepository utilisateurCommandRepository,
           IUsersQueryRepository utilisateurQueryRepository, IMediator mediator)
        {
            _utilisateurCommandRepository = utilisateurCommandRepository;
            _utilisateurQueryRepository = utilisateurQueryRepository;
            _mediator = mediator;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(UsersEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> responseBase = new ResponseBase<UsersResponse>();

            var existingUser = await _utilisateurQueryRepository.GetByIdAsync(request.Id);
            if (existingUser == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "L'utilisateur ne correspond pas à l'ID fournit.";
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

            // Appliquer les modifications
            var utilisateurEntity = UsersMapper.Mapper.Map(request, existingUser);

            if (utilisateurEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            try
            {
                utilisateurEntity.UpdatedAt = DateTime.Now;
                await _utilisateurCommandRepository.UpdateAsync(utilisateurEntity);
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + exp.Message;
                return responseBase;
            }

            var modifiedUtilisateur = await _utilisateurQueryRepository.GetUsersById(request.Id);
            responseBase.Data = UsersMapper.Mapper.Map<UsersResponse>(modifiedUtilisateur);

            return responseBase;
        }
    }
}
