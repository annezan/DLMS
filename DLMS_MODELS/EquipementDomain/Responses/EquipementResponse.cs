using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.EquipementDomain.Responses
{
    public class EquipementResponse : AuditableEntity
    {
        public int Id { get; set; }

        public string? NumeroSerie { get; set; }
        public string? Libelle { get; set; }
        public string? AdresseIp { get; set; }
        public string Type { get; set; }
        public string? Marque { get; set; }

        public string? Port { get; set; }
        public string? SerialPort { get; set; }
        public DateTime? DatePremierePose { get; set; }

        public DateTime? DatePoseActuelle { get; set; }
        
        // Optionnel: Liste des cellules associées via les compteurs
        public List<CelluleResponse> Cellules { get; set; } = new List<CelluleResponse>();
    }
}

