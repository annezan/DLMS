using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsactionschedule
{
    public int Gxdlmsactionscheduleid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Objecttype { get; set; }

    public string? ExecutedscriptLn { get; set; }

    public int? Executedscriptselector { get; set; }

    public int? Actiontype { get; set; }

    public virtual ICollection<Gxdlmsactionscheduleexecutiontime> Gxdlmsactionscheduleexecutiontimes { get; set; } = new List<Gxdlmsactionscheduleexecutiontime>();

    public virtual Objects Object { get; set; } = null!;
}
