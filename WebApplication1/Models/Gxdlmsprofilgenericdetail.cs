using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsprofilgenericdetail
{
    public int Id { get; set; }

    public int Codeobisid { get; set; }

    public string Value { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int? Profilgenericid { get; set; }

    public virtual CodeObi Codeobis { get; set; } = null!;
}
