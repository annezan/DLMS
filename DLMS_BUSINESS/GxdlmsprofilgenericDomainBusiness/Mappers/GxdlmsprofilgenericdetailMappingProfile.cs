using AutoMapper;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Responses;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Commands;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;

namespace DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Mappers
{
    public class GxdlmsprofilgenericdetailMappingProfile : Profile
    {
        public GxdlmsprofilgenericdetailMappingProfile()
        {
            CreateMap<CodeObis, CodeObisResponse>().ReverseMap();
            CreateMap<Gxdlmsprofilgeneric, GxdlmsprofilgenericResponse>().ReverseMap();

            CreateMap<Gxdlmsprofilgenericdetail, GxdlmsprofilgenericdetailResponse>()
                .ForMember(dest => dest.Codeobis, opt => opt.MapFrom(src => src.Codeobis))
                .ForMember(dest => dest.Gxdlmsprofilgeneric, opt => opt.MapFrom(src => src.Gxdlmsprofilgeneric)).ReverseMap();
            CreateMap<Gxdlmsprofilgenericdetail, GxdlmsprofilgenericdetailAddCommand>().ReverseMap();
            CreateMap<Gxdlmsprofilgenericdetail, GxdlmsprofilgenericdetailDeleteCommand>().ReverseMap();
        }
    }
}
