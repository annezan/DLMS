using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CommandeCompteurDomain.Entities;

public partial class CommandeCompteur : AuditableEntity
{
    public int Id { get; set; }

    public int CompteurId { get; set; }

    public int CommandeId { get; set; }

    public string? Libellegroupe { get; set; }
    
    public int NumeroTentative { get; set; } = 0;
    
    public virtual Compteur Compteur { get; set; } = null!;

    public virtual Commande Commande { get; set; } = null!;

    public virtual ICollection<ResultatCommandeCompteur> ResultatCommandeCompteurs { get; set; } = new List<ResultatCommandeCompteur>();
}
