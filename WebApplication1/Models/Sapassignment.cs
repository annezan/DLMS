using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Sapassignment
{
    public int Sapassignmentid { get; set; }

    public int? Gxdlmssapassignmentid { get; set; }

    public int? Sapid { get; set; }

    public string? Sapname { get; set; }

    public virtual Gxdlmssapassignment? Gxdlmssapassignment { get; set; }
}
