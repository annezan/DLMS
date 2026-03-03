using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Responses;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;

public partial class GxdlmsprofilgenericResponse 
{
    public int Id { get; set; }
    public int CodeObisId { get; set; }
    public bool? IsArchive { get; set; }
    public CodeObisResponse? Codeobis { get; set; }


}
