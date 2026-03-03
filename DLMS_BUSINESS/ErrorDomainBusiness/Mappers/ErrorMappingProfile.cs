using AutoMapper;
using DLMS_MODELS.ErrorDomain.Entities;
using DLMS_MODELS.ErrorDomain.Responses;

namespace DLMS_BUSINESS.ErrorDomainBusiness.Mappers
{
    public class ErrorMappingProfile : Profile
    {
        public ErrorMappingProfile()
        {
            CreateMap<Error, ErrorResponse>().ReverseMap();
        }
    }
}
