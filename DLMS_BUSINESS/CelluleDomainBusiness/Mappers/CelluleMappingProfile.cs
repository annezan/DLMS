using AutoMapper;

using DLMS_MODELS.CelluleDomain.Commands;

using DLMS_MODELS.CelluleDomain.Entities;

using DLMS_MODELS.CelluleDomain.Responses;

using DLMS_MODELS.PosteDomain.Entities;

using DLMS_MODELS.PosteDomain.Responses;

using DLMS_MODELS.CompteurDomain.Responses;

using DLMS_MODELS.CompteurCelluleDomain.Entities;



namespace DLMS_BUSINESS.CelluleDomainBusiness.Mappers

{

    public class CelluleMappingProfile : Profile

    {

        public CelluleMappingProfile()

        {

            CreateMap<Poste, PosteResponse>()

                .ForMember(dest => dest.Cellules, opt => opt.Ignore())

                .ReverseMap();

            CreateMap<Cellule, CelluleResponse>()

                .ForMember(dest => dest.Poste, opt => opt.MapFrom(src => src.Poste))

                .ForMember(dest => dest.Compteurs, opt => opt.MapFrom(src => 

                    src.CelluleCompteurs

                        .Select(cc => cc.Compteur)

                        .Where(c => c != null)

                        .Select(c => new CompteurResponse

                        {

                            Id = c.Id,

                            IdCompteur = c.IdCompteur,

                            NumeroCompteur = c.NumeroCompteur,

                            MarqueCompteur = c.MarqueCompteur,

                            DatePremierePose = c.DatePremierePose,

                            DatePoseActuelle = c.DatePoseActuelle,

                            EnergyProfilePeriod = c.EnergyProfilePeriod,

                            CrcFirmware = c.CrcFirmware,

                            VersionFirmware = c.VersionFirmware,

                            VersionFirmwareModem = c.VersionFirmwareModem,

                            Phases = c.Phases,

                            Tarif = c.Tarif,

                            TechnicalProfilePeriod = c.TechnicalProfilePeriod,

                            TimeDifference = c.TimeDifference,

                            TypeOfTransport = c.TypeOfTransport,

                            Typecompteur = c.Typecompteur,

                            FabriquantId = c.FabriquantId,

                            Etatcontacteur = c.Etatcontacteur

                        })

                        .ToList()))

                .ReverseMap();

            CreateMap<Cellule, CelluleAddCommand>().ReverseMap();

            CreateMap<Cellule, CelluleEditCommand>().ReverseMap();

            CreateMap<Cellule, CelluleDeleteCommand>().ReverseMap();

        }

    }

}



