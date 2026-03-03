using AutoMapper;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_BUSINESS.GxDLMSDomainDomainBusiness.Mappers
{
    public class ReadMappingProfile : Profile
    {
        public ReadMappingProfile()
        {
            CreateMap<string, ReadResponse>()
                .ConvertUsing(src => new ReadResponse { Result = src });


        }
    }
}
