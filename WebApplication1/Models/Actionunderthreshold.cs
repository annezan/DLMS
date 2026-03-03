using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Actionunderthreshold
{
    public int Actionunderid { get; set; }

    public int? Gxdlmslimiterid { get; set; }

    public string? Logicalname { get; set; }

    public int? Scriptselector { get; set; }

    public DateTime? Capturetime { get; set; }

    public virtual Gxdlmslimiter? Gxdlmslimiter { get; set; }
}
