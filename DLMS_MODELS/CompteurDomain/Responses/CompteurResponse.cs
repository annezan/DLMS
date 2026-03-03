using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.EquipementDomain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.CompteurDomain.Responses
{
    public class CompteurResponse : AuditableEntity
    {
        public int Id { get; set; }
        public string IdCompteur { get; set; }
        public string NumeroCompteur { get; set; } = null!;
        public string? MarqueCompteur { get; set; }

        public DateTime? DatePremierePose { get; set; }

        public DateTime? DatePoseActuelle { get; set; }

        public string? EnergyProfilePeriod { get; set; }

        public string? CrcFirmware { get; set; }

        public string? VersionFirmware { get; set; }

        public string? VersionFirmwareModem { get; set; }

        public string? AdresseIp { get; set; }

        public string? Phases { get; set; }

        public string? Tarif { get; set; }

        public string? TechnicalProfilePeriod { get; set; }

        public string? TimeDifference { get; set; }

        public string? DataConcentrator { get; set; }

        public string? TypeOfTransport { get; set; }

        public string? Typecompteur { get; set; }

        public int FabriquantId { get; set; }

        public string? Etatcontacteur { get; set; }
        public string? Port { get; set; }

        public List<CelluleResponse> Cellules { get; set; } = new List<CelluleResponse>();


    }
}
