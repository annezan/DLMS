using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.TypecommandeDomain.Entities;

public partial class Typecommande 
{
    public int Id { get; set; }

    public string Libelletype { get; set; } = null!;

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}
