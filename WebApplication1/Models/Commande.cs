using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Commande
{
    public int Idcommande { get; set; }

    public string Libellecommande { get; set; } = null!;

    public DateTime? Dateexec { get; set; }

    public DateTime? Datefin { get; set; }

    public int? Statut { get; set; }

    public int Idtype { get; set; }

    public virtual ICollection<Commandecompteur> Commandecompteurs { get; set; } = new List<Commandecompteur>();

    public virtual Typecommande IdtypeNavigation { get; set; } = null!;
}
