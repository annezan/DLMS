using AutoMapper;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Responses;
using DLMS_MODELS.EventsDomain.Entities;
using DLMS_MODELS.EventsDomain.Responses;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Commands;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;


namespace DLMS_BUSINESS.GxdlmsprofilgenericdetailseventDomainBusiness.Mappers
{
    public class GxdlmsprofilgenericdetailseventMappingProfile : Profile
    {
        public GxdlmsprofilgenericdetailseventMappingProfile()
        {
            CreateMap<CodeObis, CodeObisResponse>().ReverseMap();
            CreateMap<Gxdlmsprofilgeneric, GxdlmsprofilgenericResponse>().ReverseMap();
            CreateMap<Events, EventsResponse>().ReverseMap();

            CreateMap<Gxdlmsprofilgenericdetailsevent, GxdlmsprofilgenericdetailseventResponse>()
                .ForMember(dest => dest.Codeobis, opt => opt.MapFrom(src => src.Codeobis))
                .ForMember(dest => dest.Gxdlmsprofilgeneric, opt => opt.MapFrom(src => src.Gxdlmsprofilgeneric))
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Event)).ReverseMap();


            CreateMap<Gxdlmsprofilgenericdetailsevent, GxdlmsprofilgenericdetailseventAddCommand>().ReverseMap();
            CreateMap<Gxdlmsprofilgenericdetailsevent, GxdlmsprofilgenericdetailseventDeleteCommand>().ReverseMap();
        }
    }
}
