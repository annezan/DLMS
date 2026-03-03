using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Objectrelation
{
    public int Relationid { get; set; }

    public int Parentobjectid { get; set; }

    public int Childobjectid { get; set; }

    public string? Relationtype { get; set; }

    public virtual Objects Childobject { get; set; } = null!;

    public virtual Objects Parentobject { get; set; } = null!;
}
