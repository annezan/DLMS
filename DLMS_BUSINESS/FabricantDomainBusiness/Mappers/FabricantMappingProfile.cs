using AutoMapper;
using DLMS_MODELS.FabricantDomain.Commands;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_MODELS.FabricantDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;

namespace DLMS_BUSINESS.FabricantDomainBusiness.Mappers
{
    public class FabricantMappingProfile : Profile
    {
        public FabricantMappingProfile()
        {
            CreateMap<Fabricant, FabricantResponse>().ReverseMap();
            CreateMap<Fabricant, FabricantAddCommand>().ReverseMap();
            CreateMap<Fabricant, FabricantEditCommand>().ReverseMap();
            CreateMap<Fabricant, FabricantDeleteCommand>().ReverseMap();

        }
    }
}
