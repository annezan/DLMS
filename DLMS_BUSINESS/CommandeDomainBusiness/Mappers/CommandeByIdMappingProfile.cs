using AutoMapper;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using DLMS_MODELS.CommandeDomain.Commands;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CommandeDomain.Responses;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Responses;
using DLMS_MODELS.TypecommandeDomain.Entities;
using DLMS_MODELS.TypecommandeDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;

namespace DLMS_BUSINESS.CommandeDomainBusiness.Mappers
{
    public class CommandeByIdMappingProfile : Profile
    {
        public CommandeByIdMappingProfile()
        {
            CreateMap<Typecommande, TypecommandeResponse>().ReverseMap();

            CreateMap<CommandeCompteur, CommandeCompteurResponse>().ReverseMap();
            CreateMap<Compteur, CompteurResponse>().ReverseMap();
            CreateMap<Commande, CommandeByIdResponse>()
                  .ForMember(dest => dest.CommandeCompteur, opt => opt.MapFrom(src => src.CommandeCompteur))
                  .ForMember(dest => dest.Typecommande, opt => opt.MapFrom(src => src.Typecommande)).ReverseMap();

            CreateMap<Commande, CommandeAddCommand>().ReverseMap();
            CreateMap<Commande, CommandeEditCommand>().ReverseMap();
            CreateMap<Commande, CommandeDeleteCommand>().ReverseMap();
        }
    }
}
