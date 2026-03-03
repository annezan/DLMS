using AutoMapper;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Responses;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers
{
    public class ResultatCommandeCompteurMappingProfile : Profile
    {
        public ResultatCommandeCompteurMappingProfile()
        {
            CreateMap<CodeObis, CodeObisResponse>().ReverseMap();
            CreateMap<Gxdlmsprofilgeneric, GxdlmsprofilgenericResponse>().ReverseMap();

            CreateMap<ResultatCommandeCompteur, ResultatCommandeCompteurResponse>()
                .ForMember(dest => dest.CodeObis, opt => opt.MapFrom(src => src.CodeObis))
                .ForMember(dest => dest.CommandeCompteur, opt => opt.MapFrom(src => src.CommandeCompteur))
                .ForMember(dest => dest.Gxdlmsprofilgeneric, opt => opt.MapFrom(src => src.Gxdlmsprofilgeneric))
                .ReverseMap();
        }
    }
}
