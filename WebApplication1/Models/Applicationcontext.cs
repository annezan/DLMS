using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Applicationcontext
{
    public int Contextid { get; set; }

    public int Associationid { get; set; }

    public int? Jointisoctt { get; set; }

    public int? Country { get; set; }

    public int? Countryname { get; set; }

    public int? Identifiedorganization { get; set; }

    public int? Dlmsua { get; set; }

    public int? Applicationcontext1 { get; set; }

    public virtual Gxdlmsassociationlogicalname Association { get; set; } = null!;
}
