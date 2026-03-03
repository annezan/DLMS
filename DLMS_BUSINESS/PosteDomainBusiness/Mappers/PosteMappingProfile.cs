using AutoMapper;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.PosteDomain.Commands;
using DLMS_MODELS.PosteDomain.Entities;
using DLMS_MODELS.PosteDomain.Responses;
using DLMS_MODELS.TypecommandeDomain.Entities;
using DLMS_MODELS.TypecommandeDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;

namespace DLMS_BUSINESS.PosteDomainBusiness.Mappers
{
    public class PosteMappingProfile : Profile
    {
        public PosteMappingProfile()
        {
            CreateMap<Cellule, CelluleResponse>().ReverseMap();
            CreateMap<Poste, PosteResponse>()
                .ForMember(dest => dest.Cellules, opt => opt.MapFrom(src => src.Cellules))
                .ReverseMap();
            CreateMap<Poste, PosteAddCommand>().ReverseMap();
            CreateMap<Poste, PosteEditCommand>().ReverseMap();
            CreateMap<Poste, PosteDeleteCommand>().ReverseMap();
        }
    }
}
