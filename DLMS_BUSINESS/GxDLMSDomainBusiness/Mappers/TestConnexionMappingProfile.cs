using AutoMapper;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_BUSINESS.GxDLMSDomainDomainBusiness.Mappers
{
    public class TestConnexionMappingProfile : Profile
    {
        public TestConnexionMappingProfile()
        {
            CreateMap<string, TestConnexionResponse>()
                .ConvertUsing(src => new TestConnexionResponse { Result = src });


        }
    }
}
