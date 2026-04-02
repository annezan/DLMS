using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using System;

namespace DLMS_MODELS.CommandeCompteurDomain.Entities;

public partial class ResultatCommandeCompteur : AuditableEntity
{
    public int Id { get; set; }

    public int CommandeCompteurId { get; set; }

    public int CodeObisId { get; set; }

    public int GxdlmsprofilgenericId { get; set; }

    public string Value { get; set; } = null!;
    public string? RawValue { get; set; }
    public string? Unite { get; set; }
    public int? Exposant { get; set; }
    public double? ScalerGurux { get; set; }
    public DateTime? DateCreation { get; set; }

    public string NumeroCompteur { get; set; } = null!;

    public DateTime? DateEnr { get; set; }

    public bool? IsArchive { get; set; }

    // Relations
    public virtual CommandeCompteur CommandeCompteur { get; set; } = null!;

    public virtual CodeObis CodeObis { get; set; } = null!;

    public virtual Gxdlmsprofilgeneric Gxdlmsprofilgeneric { get; set; } = null!;
}
