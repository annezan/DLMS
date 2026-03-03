using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Xdlmscontextinfo
{
    public int Contextinfoid { get; set; }

    public int Associationid { get; set; }

    public int? Conformance { get; set; }

    public int? Maxreceivepdusize { get; set; }

    public int? Maxsendpdusize { get; set; }

    public int? Dlmsversionnumber { get; set; }

    public int? Qualityofservice { get; set; }

    public string? Cypheringinfo { get; set; }

    public virtual Gxdlmsassociationlogicalname Association { get; set; } = null!;
}
