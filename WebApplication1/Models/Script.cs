using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Script
{
    public int Scriptid { get; set; }

    public int? Gxdlmsscripttableid { get; set; }

    public int? Actiontype { get; set; }

    public int? Objecttype { get; set; }

    public string? Actionln { get; set; }

    public int? Actionindex { get; set; }

    public int? Parameterdatatype { get; set; }

    public string? Parameter { get; set; }

    public virtual Gxdlmsscripttable? Gxdlmsscripttable { get; set; }
}
