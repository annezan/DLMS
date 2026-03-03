using DLMS_MODELS.Bases;
using DLMS_MODELS.EventsDomain.Responses;
using MediatR;

namespace DLMS_MODELS.EventsDomain.Queries
{
    public class GetEventsQuery : IRequest<ResponseBase<List<EventsResponse>>>
    {
        public GetEventsQuery()
        {
        }
    }
}
