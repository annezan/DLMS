using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, ResponseBase<List<PermissionResponse>>>
    {
        private readonly IPermissionsQueryRepository _queryRepository;

        public GetPermissionsQueryHandler(IPermissionsQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<ResponseBase<List<PermissionResponse>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<PermissionResponse>> responseBase = new ResponseBase<List<PermissionResponse>>();

            try
            {
                var permissions = await _queryRepository.GetAllAsync(x => x.DeletedAt == null);
                responseBase.Data = PermissionMapper.Mapper.Map<List<PermissionResponse>>(permissions);
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
