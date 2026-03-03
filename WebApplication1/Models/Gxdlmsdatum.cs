using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsdatum
{
    public int Gxdlmsdataid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? ValueType { get; set; }

    public int? ValueUitype { get; set; }

    public string? ValueContent { get; set; }

    public string NumeroCompteur { get; set; } = null!;

    public virtual ICollection<Gxdlmsassociationobjectlist> Gxdlmsassociationobjectlists { get; set; } = new List<Gxdlmsassociationobjectlist>();

    public virtual Objects Object { get; set; } = null!;
}
