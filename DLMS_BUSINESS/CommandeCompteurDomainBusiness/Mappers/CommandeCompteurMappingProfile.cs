using AutoMapper;
using DLMS_MODELS.CommandeCompteurDomain.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Responses;
namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers
{
    public class CommandeCompteurMappingProfile : Profile
    {
        public CommandeCompteurMappingProfile()
        {
            CreateMap<Compteur, CompteurResponse>().ReverseMap();

            CreateMap<CommandeCompteur, CommandeCompteurResponse>()
                .ForMember(dest => dest.Compteur, opt => opt.MapFrom(src => src.Compteur))
                .ForMember(dest => dest.ResultatCommandeCompteurs, opt => opt.MapFrom(src => src.ResultatCommandeCompteurs))
                .ReverseMap();
            CreateMap<CommandeCompteur, CommandeCompteurAddCommand>().ReverseMap();
            CreateMap<CommandeCompteur, CommandeCompteurEditCommand>().ReverseMap();
            CreateMap<CommandeCompteur, CommandeCompteurDeleteCommand>().ReverseMap();

        }
    }
}
