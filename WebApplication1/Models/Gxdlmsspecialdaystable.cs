using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsspecialdaystable
{
    public int Gxdlmsspecialdaystableid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public virtual Objects Object { get; set; } = null!;

    public virtual ICollection<Specialdayentry> Specialdayentries { get; set; } = new List<Specialdayentry>();
}
