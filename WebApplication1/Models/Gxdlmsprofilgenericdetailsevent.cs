using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsprofilgenericdetailsevent
{
    public int Id { get; set; }

    public int Codeobisid { get; set; }

    public string Value { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int Profilgenericid { get; set; }

    public virtual CodeObi Codeobis { get; set; } = null!;
}
