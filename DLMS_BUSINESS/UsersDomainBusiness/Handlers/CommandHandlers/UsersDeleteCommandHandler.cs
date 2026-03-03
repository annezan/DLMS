using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersDeleteCommandHandler : IRequestHandler<UsersDeleteCommand, ResponseBase<string>> 
    {

        private readonly IUsersCommandRepository _utilisateurCommandRepository;
        private readonly IUsersQueryRepository _utilisateurQueryRepository;


        public UsersDeleteCommandHandler(IUsersCommandRepository utilisateurCommandRepository,
            IUsersQueryRepository utilisateurQueryRepository)
        {
            _utilisateurCommandRepository = utilisateurCommandRepository;
            _utilisateurQueryRepository = utilisateurQueryRepository;
        }

        public async Task<ResponseBase<string>> Handle(UsersDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();
            try
            {
                var utilisateurEntity = await _utilisateurQueryRepository.GetUsersById(request.Id);
                if (utilisateurEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "L'utilisateur n'existe pas.";
                    return responseBase;
                }


                utilisateurEntity.DeletedAt = DateTime.Now;
                await _utilisateurCommandRepository.DeleteAsync(utilisateurEntity);
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + ex.Message;
                return responseBase;
            }

            responseBase.Data = "Utilisateur suppimé avec succès";
            return responseBase;
        }
    }
}
