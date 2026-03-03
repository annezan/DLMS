using DLMS_BUSINESS.AlarmsDomainBusiness.Mappers;
using DLMS_DAL.AlarmsDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.AlarmsDomain.Queries;
using DLMS_MODELS.AlarmsDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.AlarmsDomainBusiness.Handlers.QueryHandlers
{
    public class GetAlarmsQueryHandler : IRequestHandler<GetAlarmsQuery, ResponseBase<List<AlarmsResponse>>>
    {
        private readonly IAlarmsQueryRepository _AlarmsQueryRepository;

        public GetAlarmsQueryHandler(IAlarmsQueryRepository AlarmsQueryRepository)
        {
            _AlarmsQueryRepository = AlarmsQueryRepository;
        }

        public async Task<ResponseBase<List<AlarmsResponse>>> Handle(GetAlarmsQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<AlarmsResponse>> responseBase = new ResponseBase<List<AlarmsResponse>>();
            try
            {
                var Alarms = await _AlarmsQueryRepository.GetAlarms();
                responseBase.Data = AlarmsMapper.Mapper.Map<List<AlarmsResponse>>(Alarms);
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
