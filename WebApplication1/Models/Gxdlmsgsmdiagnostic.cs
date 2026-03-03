using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsgsmdiagnostic
{
    public int Gxdlmsgsmdiagnosticid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public string? Operator { get; set; }

    public int? Status { get; set; }

    public int? Circuitswitchstatus { get; set; }

    public int? Packetswitchstatus { get; set; }

    public DateTime? Capturetime { get; set; }

    public virtual ICollection<Gxdlmsgsmdiagnosticadjacentcell> Gxdlmsgsmdiagnosticadjacentcells { get; set; } = new List<Gxdlmsgsmdiagnosticadjacentcell>();

    public virtual ICollection<Gxdlmsgsmdiagnosticcellinfo> Gxdlmsgsmdiagnosticcellinfos { get; set; } = new List<Gxdlmsgsmdiagnosticcellinfo>();

    public virtual Objects Object { get; set; } = null!;
}
