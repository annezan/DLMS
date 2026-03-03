using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using DLMS_MODELS.TypecommandeDomain.Entities;
using DLMS_MODELS.TypecommandeDomain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.CommandeDomain.Responses
{
    public class CommandeResponse : AuditableEntity
    {
        public int Id { get; set; }

        public string Libellecommande { get; set; } = null!;

        public DateTime? Dateexec { get; set; }

        public DateTime? Datefin { get; set; }

        public string Statut { get; set; }

        public int Idtype { get; set; }
        public int? Numeroprofile { get; set; }
        public int? Nombreentree { get; set; }
        public DateTime? Datedebut { get; set; }
        public DateTime? Dateexp { get; set; }
        public TypecommandeResponse? Typecommande { get; set; } = null!;

    }
}
