using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmslimiteremergencyprofile
{
    public int Emergencyprofileid { get; set; }

    public int? Gxdlmslimiterid { get; set; }

    public int? Profileid { get; set; }

    public string? Starttime { get; set; }

    public int? Duration { get; set; }

    public DateTime? Capturetime { get; set; }

    public virtual Gxdlmslimiter? Gxdlmslimiter { get; set; }
}
