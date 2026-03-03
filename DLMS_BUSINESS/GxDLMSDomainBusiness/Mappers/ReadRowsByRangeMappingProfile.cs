using AutoMapper;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_BUSINESS.GxDLMSDomainDomainBusiness.Mappers
{
    public class ReadRowsByRangeMappingProfile : Profile
    {
        public ReadRowsByRangeMappingProfile()
        {
            CreateMap<string, ReadRowsByRangeResponse>()
                .ConvertUsing(src => new ReadRowsByRangeResponse { Result = src });


        }
    }
}
