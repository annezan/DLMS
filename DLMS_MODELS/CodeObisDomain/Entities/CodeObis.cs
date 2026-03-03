using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CodeObisDomain.Entities;

public partial class CodeObis 
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public string Code1 { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Code2 { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<Gxdlmsprofilgenericdetail> Gxdlmsprofilgenericdetails { get; set; } = new List<Gxdlmsprofilgenericdetail>();

    public virtual ICollection<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevents { get; set; } = new List<Gxdlmsprofilgenericdetailsevent>();

    public virtual Gxdlmsprofilgeneric Gxdlmsprofilgenerics { get; set; }
}
