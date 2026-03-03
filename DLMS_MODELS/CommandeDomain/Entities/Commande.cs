using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.TypecommandeDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CommandeDomain.Entities;

public partial class Commande : AuditableEntity
{
    public int Id { get; set; }

    public string Libellecommande { get; set; } = null!;

    public DateTime? Dateexec { get; set; }

    public DateTime? Datefin { get; set; }

    public string Statut { get; set; }
    public int? Numeroprofile { get; set; }
    public int? Nombreentree { get; set; }
    public DateTime? Datedebut { get; set; }
    public DateTime? Dateexp { get; set; }
    public int TypecommandeId { get; set; }
    public virtual ICollection<CommandeCompteur> CommandeCompteur { get; set; } = new List<CommandeCompteur>();

    public virtual Typecommande Typecommande { get; set; } = null!;
}
