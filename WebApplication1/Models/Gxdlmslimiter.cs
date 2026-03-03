using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmslimiter
{
    public int Gxdlmslimiterid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public double? Thresholdactive { get; set; }

    public double? Thresholdnormal { get; set; }

    public double? Thresholdemergency { get; set; }

    public int? Minoverthresholdduration { get; set; }

    public int? Minunderthresholdduration { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Actionoverthreshold> Actionoverthresholds { get; set; } = new List<Actionoverthreshold>();

    public virtual ICollection<Actionunderthreshold> Actionunderthresholds { get; set; } = new List<Actionunderthreshold>();

    public virtual ICollection<Gxdlmslimiteremergencyprofile> Gxdlmslimiteremergencyprofiles { get; set; } = new List<Gxdlmslimiteremergencyprofile>();

    public virtual ICollection<Gxdlmslimitermonitoredvalue> Gxdlmslimitermonitoredvalues { get; set; } = new List<Gxdlmslimitermonitoredvalue>();

    public virtual Objects Object { get; set; } = null!;
}
