using AutoMapper;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Responses;

namespace DLMS_BUSINESS.CodeObisDomainBusiness.Mappers
{
    public class CodeObisMappingProfile : Profile
    {
        public CodeObisMappingProfile()
        {
            CreateMap<CodeObis, CodeObisResponse>().ReverseMap();
        }
    }
}
