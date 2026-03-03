using DLMS_MODELS.Bases;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.ErrorDomain.Entities;

public partial class Error 
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public string Code1 { get; set; } = null!;

    public int Value { get; set; }

    public string? Code2 { get; set; }



    //public virtual ICollection<Gxdlmsprofilgenericdetail> Gxdlmsprofilgenericdetails { get; set; } = new List<Gxdlmsprofilgenericdetail>();

    //public virtual ICollection<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevents { get; set; } = new List<Gxdlmsprofilgenericdetailsevent>();

    //public virtual ICollection<Gxdlmsprofilgeneric> Gxdlmsprofilgenerics { get; set; } = new List<Gxdlmsprofilgeneric>();
}
