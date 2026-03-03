using DLMS_BUSINESS.UsersDomainBusiness.Mappers;
using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.QueryHandlers
{
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, ResponseBase<PermissionResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IPermissionsQueryRepository _queryRepository;

        public GetPermissionByIdQueryHandler(IMediator mediator, IPermissionsQueryRepository queryRepository)
        {
            _mediator = mediator;
            _queryRepository = queryRepository;
        }

        public async Task<ResponseBase<PermissionResponse>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<PermissionResponse> responseBase = new ResponseBase<PermissionResponse>();

            var role = await _queryRepository.GetByIdAsync(request.Id);

            if (role == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Permission Introuvable ou désactivé";
                return responseBase;
            }

            responseBase.Data = PermissionMapper.Mapper.Map<PermissionResponse>(role);

            return responseBase;
        }
    }

}
