using AutoMapper;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_BUSINESS.GxDLMSDomainDomainBusiness.Mappers
{
    public class ReadObjectProfileMappingProfile : Profile
    {
        public ReadObjectProfileMappingProfile()
        {
            CreateMap<string, ReadObjectProfileResponse>()
                .ConvertUsing(src => new ReadObjectProfileResponse { Result = src });


        }
    }
}
