using AutoMapper;
using DLMS_MODELS.CompteurEquipementDomain.Commands;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;

namespace DLMS_BUSINESS.CompteurEquipementDomainBusiness.Mappers
{
    public class CompteurEquipementMappingProfile : Profile
    {
        public CompteurEquipementMappingProfile()
        {
            CreateMap<CompteurEquipement, CompteurEquipementResponse>().ReverseMap();
            CreateMap<CompteurEquipement, CompteurEquipementAddCommand>().ReverseMap();
            CreateMap<CompteurEquipement, CompteurEquipementDeleteCommand>().ReverseMap();

        }
    }
}
