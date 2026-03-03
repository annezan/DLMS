using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmssecuritysetup
{
    public int Gxdlmssecuritysetupid { get; set; }

    public int Objectid { get; set; }

    public int? Securitypolicy { get; set; }

    public int? Securitysuite { get; set; }

    public string? Clientsystemtitle { get; set; }

    public string? Serversystemtitle { get; set; }

    public string? Certificatedata { get; set; }

    public string? Guek { get; set; }

    public string? Gbek { get; set; }

    public string? Gak { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
