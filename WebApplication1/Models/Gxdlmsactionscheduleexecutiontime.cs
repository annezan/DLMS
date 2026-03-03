using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsactionscheduleexecutiontime
{
    public int Executiontimeid { get; set; }

    public int? Gxdlmsactionscheduleid { get; set; }

    public string? Time { get; set; }

    public virtual Gxdlmsactionschedule? Gxdlmsactionschedule { get; set; }
}
