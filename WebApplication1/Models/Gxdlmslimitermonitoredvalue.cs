using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmslimitermonitoredvalue
{
    public int Monitoredvalueid { get; set; }

    public int? Gxdlmslimiterid { get; set; }

    public int? Objecttype { get; set; }

    public string? Logicalname { get; set; }

    public int? Index { get; set; }

    public virtual Gxdlmslimiter? Gxdlmslimiter { get; set; }
}
