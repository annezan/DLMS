using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ResponseBase<List<UsersResponse>>>
    {
        private readonly IUsersQueryRepository _utilisateurQueryRepository;

        public GetUsersQueryHandler(IUsersQueryRepository utilisateurQueryRepository)
        {
            _utilisateurQueryRepository = utilisateurQueryRepository;
        }

        public async Task<ResponseBase<List<UsersResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<UsersResponse>> responseBase = new ResponseBase<List<UsersResponse>>();
            try
            {
                var users = await _utilisateurQueryRepository.GetAllUsersAsync();
                responseBase.Data = UsersMapper.Mapper.Map<List<UsersResponse>>(users);
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
