using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsregister
{
    public int Gxdlmsregisterid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Unit { get; set; }

    public double? Scaler { get; set; }

    public int? ValueType { get; set; }

    public int? ValueUitype { get; set; }

    public double? ValueContent { get; set; }

    public virtual Objects Object { get; set; } = null!;
}
