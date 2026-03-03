using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsautoconnect
{
    public int Gxdlmsautoconnectid { get; set; }

    public int Objectid { get; set; }

    public int? Version { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Mode { get; set; }

    public int? Repetitions { get; set; }

    public int? Repetitiondelay { get; set; }

    public string? Callingwindow { get; set; }

    public string? Destinations { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
