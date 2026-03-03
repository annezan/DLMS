using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using DLMS_MODELS.CompteurDomain.Responses;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CommandeCompteurDomain.Responses
{
    public class CommandeCompteurResponse : AuditableEntity
    {
        public int Id { get; set; }

        public int CompteurId { get; set; }

        public int CommandeId { get; set; }

        public string? Libellegroupe { get; set; }

        public int? NumeroTentative { get; set; }

        public virtual CompteurResponse? Compteur { get; set; }

        //public virtual CommandeResponse? Commande { get; set; }

        public virtual ICollection<ResultatCommandeCompteurResponse>? ResultatCommandeCompteurs { get; set; }
    }
}
