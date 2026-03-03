using AutoMapper;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Commands;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;

namespace DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Mappers
{
    public class GxdlmsprofilgenericMappingProfile : Profile
    {
        public GxdlmsprofilgenericMappingProfile()
        {
            CreateMap<Gxdlmsprofilgeneric, GxdlmsprofilgenericResponse>().ReverseMap();
            
        }
    }
}
