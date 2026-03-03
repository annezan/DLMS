using System;
using System.Collections.Generic;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Responses;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;

public partial class GxdlmsprofilgenericdetailResponse 
{
    public int Id { get; set; }
    public int CodeObisId { get; set; }

    public int GxdlmsprofilgenericId { get; set; }
    public string Value { get; set; } = null!;

    public string NumeroCompteur { get; set; }
    public DateTime? DateEnr { get; set; }
    public bool? IsArchive { get; set; }
    public CodeObisResponse? Codeobis { get; set; }
    public GxdlmsprofilgenericResponse? Gxdlmsprofilgeneric { get; set; }


}
