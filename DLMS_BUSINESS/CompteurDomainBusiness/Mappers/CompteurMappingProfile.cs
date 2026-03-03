using AutoMapper;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.CompteurDomain.Commands;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Responses;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.EquipementDomain.Entities;
using DLMS_MODELS.EquipementDomain.Responses;
using DLMS_MODELS.GxDLMSDomain.Responses;
using DLMS_MODELS.PosteDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;

namespace DLMS_BUSINESS.CompteurDomainBusiness.Mappers
{
    public class CompteurMappingProfile : Profile
    {
        public CompteurMappingProfile()
        {
            CreateMap<Cellule, CelluleResponse>().ReverseMap();

            CreateMap<Compteur, CompteurResponse>()
                .ForMember(dest => dest.Cellules, opt => opt.MapFrom(src => 
                    src.CompteurCellules
                        .Select(cc => cc.Cellule)
                        .Where(c => c != null)
                        .Select(c => new CelluleResponse
                        {
                            Id = c.Id,
                            Type = c.Type,
                            ValeurTension = c.ValeurTension,
                            Libelle = c.Libelle,
                            Adresse = c.Adresse,
                            PosteId = c.PosteId,
                            Poste = c.Poste != null ? new PosteResponse
                            {
                                Id = c.Poste.Id,
                                Libelle = c.Poste.Libelle,
                                Adresse = c.Poste.Adresse
                            } : null
                        })
                        .ToList()))
                .ReverseMap();
            CreateMap<Compteur, CompteurAddCommand>().ReverseMap();
            CreateMap<Compteur, CompteurEditCommand>().ReverseMap();
            CreateMap<Compteur, CompteurDeleteCommand>().ReverseMap();

        }
    }
}
