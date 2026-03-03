using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsieclocalportsetup
{
    public int Gxdlmsieclocalportsetupid { get; set; }

    public int Objectid { get; set; }

    public int? Version { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Defaultmode { get; set; }

    public int? Defaultbaudrate { get; set; }

    public int? Proposedbaudrate { get; set; }

    public int? Responsetime { get; set; }

    public string? Deviceaddress { get; set; }

    public string? Password1 { get; set; }

    public string? Password2 { get; set; }

    public string? Password5 { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
