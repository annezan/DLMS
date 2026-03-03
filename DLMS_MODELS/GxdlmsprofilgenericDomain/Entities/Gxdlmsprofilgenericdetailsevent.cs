using System;
using System.Collections.Generic;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.EventsDomain.Entities;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;

public partial class Gxdlmsprofilgenericdetailsevent
{
    public int Id { get; set; }
    public int CodeObisId { get; set; }

    public int GxdlmsprofilgenericId { get; set; }
    public int? EventId { get; set; }
    public string Value { get; set; } = null!;

  
    public string NumeroCompteur { get; set; }
    public DateTime? DateEnr { get; set; }
    public bool? IsArchive { get; set; }

    //public virtual Compteur Compteur { get; set; } = null!;

    public virtual CodeObis Codeobis { get; set; } = null!;
    public virtual Gxdlmsprofilgeneric Gxdlmsprofilgeneric { get; set; } = null!;
    public virtual Events Event { get; set; } = null!;
}
