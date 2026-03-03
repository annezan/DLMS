using DLMS_BUSINESS.AlarmsDomainBusiness.Mappers;
using DLMS_DAL.AlarmsDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.AlarmsDomain.Queries;
using DLMS_MODELS.AlarmsDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;

namespace DLMS_BUSINESS.AlarmsDomainBusiness.Handlers.QueryHandlers
{
    public class GetAlarmsByValueQueryHandler : IRequestHandler<GetAlarmsByValueQuery, ResponseBase<AlarmsResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IAlarmsQueryRepository _AlarmsQueryRepository;

        public GetAlarmsByValueQueryHandler(IMediator mediator, IAlarmsQueryRepository AlarmsQueryRepository)
        {
            _mediator = mediator;
            _AlarmsQueryRepository = AlarmsQueryRepository;
        }

        public async Task<ResponseBase<AlarmsResponse>> Handle(GetAlarmsByValueQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<AlarmsResponse> responseBase = new ResponseBase<AlarmsResponse>();

            try
            {
                var Alarms = await _AlarmsQueryRepository.GetAlarmsByValue(request.Value);
                responseBase.Data = AlarmsMapper.Mapper.Map<AlarmsResponse>(Alarms);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Alarms Introuvable ou désactivé";
                return responseBase;
            }
        }
    }

}
