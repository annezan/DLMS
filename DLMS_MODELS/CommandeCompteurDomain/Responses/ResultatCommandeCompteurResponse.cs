using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Responses;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using System;

namespace DLMS_MODELS.CommandeCompteurDomain.Responses
{
    public class ResultatCommandeCompteurResponse : AuditableEntity
    {
        public int Id { get; set; }

        public int CommandeCompteurId { get; set; }

        public int CodeObisId { get; set; }

        public int GxdlmsprofilgenericId { get; set; }

        public string Value { get; set; } = null!;

        public string NumeroCompteur { get; set; } = null!;

        public DateTime? DateEnr { get; set; }

        public bool? IsArchive { get; set; }

        public virtual CodeObisResponse? CodeObis { get; set; }

        public virtual CommandeCompteurResponse? CommandeCompteur { get; set; }

        public virtual GxdlmsprofilgenericResponse? Gxdlmsprofilgeneric { get; set; }
    }
}
