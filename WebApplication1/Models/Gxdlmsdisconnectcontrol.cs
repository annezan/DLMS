using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsdisconnectcontrol
{
    public int Gxdlmsdisconnectcontrolid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Outputstate { get; set; }

    public int? Controlstate { get; set; }

    public int? Controlmode { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
