using DLMS_MODELS.Bases;
using DLMS_MODELS.AlarmsDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AlarmsDomain.Queries
{
    public class GetAlarmsByIdQuery : IRequest<ResponseBase<AlarmsResponse>>
    {
        public int Id { get; set; }
        public GetAlarmsByIdQuery() { }
        public GetAlarmsByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
