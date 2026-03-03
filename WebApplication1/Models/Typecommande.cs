using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Typecommande
{
    public int Idtype { get; set; }

    public string Libelletype { get; set; } = null!;

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}
