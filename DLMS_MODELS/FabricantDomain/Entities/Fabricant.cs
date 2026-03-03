using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.FabricantDomain.Entities;

public partial class Fabricant : AuditableEntity
{
    public int Id { get; set; }

    public string Libelle { get; set; }
    
    public virtual ICollection<Compteur> Compteurs { get; set; }
   

}
