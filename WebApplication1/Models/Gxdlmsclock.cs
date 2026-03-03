using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsclock
{
    public int Gxdlmsclockid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public DateTime? Time { get; set; }

    public int? Timezone { get; set; }

    public int? Status { get; set; }

    public string? Begin { get; set; }

    public string? End { get; set; }

    public int? Deviation { get; set; }

    public bool? Enabled { get; set; }

    public int? Clockbase { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
