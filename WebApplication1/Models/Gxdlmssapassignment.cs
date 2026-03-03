using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmssapassignment
{
    public int Gxdlmssapassignmentid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public string? Sapassignmentlist { get; set; }

    public virtual Objects Object { get; set; } = null!;

    public virtual ICollection<Sapassignment> Sapassignments { get; set; } = new List<Sapassignment>();
}
