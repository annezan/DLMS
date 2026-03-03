using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class CodeObi
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? TagPath { get; set; }

    public string? TagType { get; set; }

    public bool? Status { get; set; }

    public string AddedUser { get; set; } = null!;

    public string? UpdatedUser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Gxdlmsprofilgenericdetail> Gxdlmsprofilgenericdetails { get; set; } = new List<Gxdlmsprofilgenericdetail>();

    public virtual ICollection<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevents { get; set; } = new List<Gxdlmsprofilgenericdetailsevent>();

    public virtual ICollection<Gxdlmsprofilgeneric> Gxdlmsprofilgenerics { get; set; } = new List<Gxdlmsprofilgeneric>();
}
