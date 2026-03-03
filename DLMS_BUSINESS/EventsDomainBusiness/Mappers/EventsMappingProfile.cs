using AutoMapper;
using DLMS_MODELS.EventsDomain.Entities;
using DLMS_MODELS.EventsDomain.Responses;

namespace DLMS_BUSINESS.EventsDomainBusiness.Mappers
{
    public class EventsMappingProfile : Profile
    {
        public EventsMappingProfile()
        {
            CreateMap<Events, EventsResponse>().ReverseMap();
        }
    }
}
