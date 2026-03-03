using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.EventsDomain.Entities;

public partial class Events 
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public string Code1 { get; set; } = null!;

    public int Value { get; set; } 
    public bool? BitMask { get; set; }
    public string? Code2 { get; set; }

    public virtual ICollection<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevents { get; set; } = new List<Gxdlmsprofilgenericdetailsevent>();

}
