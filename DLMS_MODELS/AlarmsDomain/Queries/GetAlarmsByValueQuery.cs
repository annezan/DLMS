using DLMS_MODELS.Bases;
using DLMS_MODELS.AlarmsDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AlarmsDomain.Queries
{
    public class GetAlarmsByValueQuery : IRequest<ResponseBase<AlarmsResponse>>
    {
        public int Value { get; set; }
        public GetAlarmsByValueQuery() {}
        public GetAlarmsByValueQuery(int Value)
        {
            this.Value = Value;
        }
    }
}
