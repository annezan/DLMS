using DLMS_BUSINESS.EventsDomainBusiness.Mappers;
using DLMS_DAL.EventsDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EventsDomain.Queries;
using DLMS_MODELS.EventsDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.EventsDomainBusiness.Handlers.QueryHandlers
{
    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, ResponseBase<List<EventsResponse>>>
    {
        private readonly IEventsQueryRepository _EventsQueryRepository;

        public GetEventsQueryHandler(IEventsQueryRepository EventsQueryRepository)
        {
            _EventsQueryRepository = EventsQueryRepository;
        }

        public async Task<ResponseBase<List<EventsResponse>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<EventsResponse>> responseBase = new ResponseBase<List<EventsResponse>>();
            try
            {
                var Events = await _EventsQueryRepository.GetEvents();
                responseBase.Data = EventsMapper.Mapper.Map<List<EventsResponse>>(Events);
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
