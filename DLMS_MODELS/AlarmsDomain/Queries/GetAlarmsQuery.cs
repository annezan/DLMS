using DLMS_MODELS.Bases;
using DLMS_MODELS.AlarmsDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AlarmsDomain.Queries
{
    public class GetAlarmsQuery : IRequest<ResponseBase<List<AlarmsResponse>>>
    {
        public GetAlarmsQuery()
        {
        }
    }
}
