using AutoMapper;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.EquipementDomain.Commands;
using DLMS_MODELS.EquipementDomain.Entities;
using DLMS_MODELS.EquipementDomain.Responses;
using DLMS_MODELS.TypecommandeDomain.Entities;
using DLMS_MODELS.TypecommandeDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.PosteDomain.Responses;

namespace DLMS_BUSINESS.EquipementDomainBusiness.Mappers
{
    public class EquipementMappingProfile : Profile
    {
        public EquipementMappingProfile()
        {
            CreateMap<Cellule, CelluleResponse>().ReverseMap();

            CreateMap<Equipement, EquipementResponse>()
                .ForMember(dest => dest.Cellules, opt => opt.MapFrom(src => 
                    src.EquipementCompteur
                        .SelectMany(ec => ec.Compteur.CompteurCellules)
                        .Select(cc => cc.Cellule)
                        .Where(c => c != null && !c.IsArchive)
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
                        .Distinct()
                        .ToList()))
                .ReverseMap();
            CreateMap<Equipement, EquipementAddCommand>().ReverseMap();
            CreateMap<Equipement, EquipementEditCommand>().ReverseMap();
            CreateMap<Equipement, EquipementDeleteCommand>().ReverseMap();
        }
    }
}
