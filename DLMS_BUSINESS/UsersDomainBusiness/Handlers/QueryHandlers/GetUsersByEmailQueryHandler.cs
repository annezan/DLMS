using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetUsersByEmailQueryHandler : IRequestHandler<GetUsersByEmailQuery, ResponseBase<UsersResponse>>
    {
        private readonly IUsersQueryRepository _utilisateurQueryRepository;


        public GetUsersByEmailQueryHandler(IUsersQueryRepository utilisateurQueryRepository)
        {
            _utilisateurQueryRepository = utilisateurQueryRepository;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(GetUsersByEmailQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> responseBase = new ResponseBase<UsersResponse>();

            var user = await _utilisateurQueryRepository.GetUsersByEmail(request.Email);

            if (user == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Utilisateur Introuvable ou désactivé";
                return responseBase;
            }
            responseBase.Data = UsersMapper.Mapper.Map<UsersResponse>(user);

            return responseBase;
        }
    }
}
