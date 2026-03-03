using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsprofilgeneric
{
    public int Id { get; set; }

    public int Codeobisid { get; set; }

    public string Compteurid { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string Origine { get; set; } = null!;

    public string? Type { get; set; }

    public virtual CodeObi Codeobis { get; set; } = null!;

    public virtual Compteur Compteur { get; set; } = null!;
}
