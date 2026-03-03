using DLMS_MODELS.Bases;
using DLMS_MODELS.EventsDomain.Responses;
using MediatR;

namespace DLMS_MODELS.EventsDomain.Queries
{
    public class GetEventsByValueQuery : IRequest<ResponseBase<EventsResponse>>
    {
        public int Value { get; set; }
        public GetEventsByValueQuery() { }
        public GetEventsByValueQuery(int Value)
        {
            this.Value = Value;
        }
    }
}
