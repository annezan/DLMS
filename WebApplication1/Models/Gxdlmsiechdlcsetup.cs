using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsiechdlcsetup
{
    public int Gxdlmsiechdlcsetupid { get; set; }

    public int Objectid { get; set; }

    public int? Version { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Speed { get; set; }

    public int? Windowsizetx { get; set; }

    public int? Windowsizerx { get; set; }

    public int? Maxinfolengthtx { get; set; }

    public int? Maxinfolengthrx { get; set; }

    public int? Intercharactertimeout { get; set; }

    public int? Inactivitytimeout { get; set; }

    public string? Deviceaddress { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
