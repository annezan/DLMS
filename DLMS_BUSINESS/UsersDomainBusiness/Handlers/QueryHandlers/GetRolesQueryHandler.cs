using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, ResponseBase<List<RoleResponse>>>
    {
        private readonly IRolesQueryRepository _queryRepository;

        public GetRolesQueryHandler(IRolesQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<ResponseBase<List<RoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<RoleResponse>> responseBase = new ResponseBase<List<RoleResponse>>();

            try
            {
                // Récupérer les rôles avec leurs permissions
                var roles = await _queryRepository.GetAllWithPermissionsAsync();
                responseBase.Data = RolesMapper.Mapper.Map<List<RoleResponse>>(roles);
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
