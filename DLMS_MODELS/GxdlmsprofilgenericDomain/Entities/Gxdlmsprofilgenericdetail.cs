using System;
using System.Collections.Generic;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;

public partial class Gxdlmsprofilgenericdetail 
{
    public int Id { get; set; }
    public int CodeObisId { get; set; }

    public int GxdlmsprofilgenericId { get; set; }
    public string Value { get; set; } = null!;
    public string? RawValue { get; set; }
    public string? Unite { get; set; }
    public int? Exposant { get; set; }
    public double? ScalerGurux { get; set; }
    public DateTime? DateCreation { get; set; }

    public string NumeroCompteur { get; set; }
    public DateTime? DateEnr { get; set; }
    public bool? IsArchive { get; set; }

    //public virtual Compteur Compteur { get; set; } = null!;


    public virtual CodeObis Codeobis { get; set; } = null!;
    public virtual Gxdlmsprofilgeneric Gxdlmsprofilgeneric { get; set; } = null!;
}
