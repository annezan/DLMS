using DLMS_BUSINESS.EventsDomainBusiness.Mappers;
using DLMS_DAL.EventsDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EventsDomain.Queries;
using DLMS_MODELS.EventsDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_BUSINESS.EventsDomainBusiness.Mappers;
using DLMS_MODELS.EventsDomain.Responses;

namespace DLMS_BUSINESS.EventsDomainBusiness.Handlers.QueryHandlers
{
    public class GetEventsByIdQueryHandler : IRequestHandler<GetEventsByIdQuery, ResponseBase<EventsResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IEventsQueryRepository _EventsQueryRepository;

        public GetEventsByIdQueryHandler(IMediator mediator, IEventsQueryRepository EventsQueryRepository)
        {
            _mediator = mediator;
            _EventsQueryRepository = EventsQueryRepository;
        }

        public async Task<ResponseBase<EventsResponse>> Handle(GetEventsByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<EventsResponse> responseBase = new ResponseBase<EventsResponse>();

            try
            {
                var Events = await _EventsQueryRepository.GetEventsById(request.Id);
                responseBase.Data = EventsMapper.Mapper.Map<EventsResponse>(Events);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Evénement Introuvable ou désactivé";
                return responseBase;
            }

        }
    }

}
