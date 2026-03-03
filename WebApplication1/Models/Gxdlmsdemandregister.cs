using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsdemandregister
{
    public int Gxdlmsdemandregisterid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Currentaveragevaluetype { get; set; }

    public int? Currentaveragevalueuitype { get; set; }

    public int? Currentaveragevaluecontent { get; set; }

    public int? Lastaveragevaluetype { get; set; }

    public int? Lastaveragevalueuitype { get; set; }

    public int? LastaveragevalueContent { get; set; }

    public double? Scaler { get; set; }

    public int? Unit { get; set; }

    public int? Statustype { get; set; }

    public DateTime? Capturetime { get; set; }

    public DateTime? Starttimecurrent { get; set; }

    public int? Period { get; set; }

    public int? Numberofperiods { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
