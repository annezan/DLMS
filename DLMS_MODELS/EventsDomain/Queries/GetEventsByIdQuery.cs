using DLMS_MODELS.Bases;
using DLMS_MODELS.EventsDomain.Responses;
using MediatR;

namespace DLMS_MODELS.EventsDomain.Queries
{
    public class GetEventsByIdQuery : IRequest<ResponseBase<EventsResponse>>
    {
        public int Id { get; set; }

        public GetEventsByIdQuery() { }
        public GetEventsByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
