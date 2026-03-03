using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsgsmdiagnosticcellinfo
{
    public int Cellinfoid { get; set; }

    public int? Gxdlmsgsmdiagnosticid { get; set; }

    public int? Cellid { get; set; }

    public int? Locationid { get; set; }

    public int? Signalquality { get; set; }

    public int? Ber { get; set; }

    public virtual Gxdlmsgsmdiagnostic? Gxdlmsgsmdiagnostic { get; set; }
}
