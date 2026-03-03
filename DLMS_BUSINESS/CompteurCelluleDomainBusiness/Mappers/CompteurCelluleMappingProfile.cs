using AutoMapper;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.CompteurCelluleDomain.Commands;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Responses;

namespace DLMS_BUSINESS.CompteurCelluleDomainBusiness.Mappers
{
    public class CompteurCelluleMappingProfile : Profile
    {
        public CompteurCelluleMappingProfile()
        {
            CreateMap<Compteur, CompteurResponse>().ReverseMap();
            CreateMap<Cellule, CelluleResponse>().ReverseMap();

            CreateMap<CompteurCellule, CompteurCelluleResponse>()
                .ForMember(dest => dest.Compteur, opt => opt.MapFrom(src => src.Compteur))
                .ForMember(dest => dest.Cellule, opt => opt.MapFrom(src => src.Cellule))
                .ReverseMap();

            CreateMap<CompteurCellule, CompteurCelluleAddCommand>().ReverseMap();
            CreateMap<CompteurCellule, CompteurCelluleDeleteCommand>().ReverseMap();
        }
    }
}
