using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using System;
using System.Collections.Generic;


namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;

public partial class Gxdlmsprofilgeneric
{
    public int Id { get; set; }
    public int CodeObisId { get; set; }
    public bool? IsArchive { get; set; }

    public virtual CodeObis Codeobis { get; set; } = null!;

}
