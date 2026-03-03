using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetUsersByIdQueryHandler : IRequestHandler<GetUsersByIdQuery, ResponseBase<UsersResponse>>
    {
        private readonly IUsersQueryRepository _utilisateurQueryRepository;

        public GetUsersByIdQueryHandler(IUsersQueryRepository utilisateurQueryRepository)
        {
            _utilisateurQueryRepository = utilisateurQueryRepository;
        }

        public async Task<ResponseBase<UsersResponse>> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<UsersResponse> responseBase = new ResponseBase<UsersResponse>();

            var user = await _utilisateurQueryRepository.GetByIdAsync(request.Id);

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
