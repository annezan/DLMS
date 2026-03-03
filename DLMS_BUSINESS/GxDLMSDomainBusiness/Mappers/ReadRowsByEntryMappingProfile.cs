using AutoMapper;
using DLMS_MODELS.GxDLMSDomain.Responses;

namespace DLMS_BUSINESS.GxDLMSDomainDomainBusiness.Mappers
{
    public class ReadRowsByEntryMappingProfile : Profile
    {
        public ReadRowsByEntryMappingProfile()
        {
            CreateMap<string, ReadRowsByEntryResponse>()
                .ConvertUsing(src => new ReadRowsByEntryResponse { Result = src });


        }
    }
}
