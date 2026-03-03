using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsgprssetuprequestedqo
{
    public int Qosid { get; set; }

    public int? Gxdlmsgprssetupid { get; set; }

    public int? Precedence { get; set; }

    public int? Delay { get; set; }

    public int? Reliability { get; set; }

    public int? Peakthroughput { get; set; }

    public int? Meanthroughput { get; set; }

    public virtual Gxdlmsgprssetup? Gxdlmsgprssetup { get; set; }
}
