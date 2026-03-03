using AutoMapper;
using DLMS_MODELS.TypecommandeDomain.Entities;
using DLMS_MODELS.TypecommandeDomain.Responses;
using DLMS_MODELS.TypecommandeDomain.Responses;

namespace DLMS_BUSINESS.TypecommandeDomainBusiness.Mappers
{
    public class TypecommandeMappingProfile : Profile
    {
        public TypecommandeMappingProfile()
        {
            CreateMap<Typecommande, TypecommandeResponse>().ReverseMap();
        }
    }
}
