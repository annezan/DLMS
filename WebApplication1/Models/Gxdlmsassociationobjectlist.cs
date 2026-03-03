using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsassociationobjectlist
{
    public int Objectid { get; set; }

    public int Associationid { get; set; }

    public int? Gxdlmsdataid { get; set; }

    public virtual Gxdlmsassociationlogicalname Association { get; set; } = null!;

    public virtual Gxdlmsdatum? Gxdlmsdata { get; set; }
}
