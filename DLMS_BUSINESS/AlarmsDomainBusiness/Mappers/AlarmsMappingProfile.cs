using AutoMapper;
using DLMS_MODELS.AlarmsDomain.Entities;
using DLMS_MODELS.AlarmsDomain.Responses;

namespace DLMS_BUSINESS.AlarmsDomainBusiness.Mappers
{
    public class AlarmsMappingProfile : Profile
    {
        public AlarmsMappingProfile()
        {
            CreateMap<Alarms, AlarmsResponse>().ReverseMap();
        }
    }
}
