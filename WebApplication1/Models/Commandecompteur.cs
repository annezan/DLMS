using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Commandecompteur
{
    public int Id { get; set; }

    public string Compteurid { get; set; } = null!;

    public int Idcommande { get; set; }

    public DateTime Dateenr { get; set; }

    public string? Libellegroupe { get; set; }

    public string Resultats { get; set; } = null!;

    public DateTime Dateenrresultat { get; set; }

    public virtual Compteur Compteur { get; set; } = null!;

    public virtual Commande IdcommandeNavigation { get; set; } = null!;
}
