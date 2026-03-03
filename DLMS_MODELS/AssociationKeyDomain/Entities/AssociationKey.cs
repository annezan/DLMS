using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.AssociationKeyDomain.Entities;

public class AssociationKey : AuditableEntity
{
    public int Id { get; set; }
    public string CompteurId { get; set; }

    public string Type { get; set; } = null!;

    public string Keyvalue { get; set; } = null!;

    public string Keyname { get; set; } = null!;

    public string? Pwd { get; set; }

    
    //public virtual Compteur? Compteur { get; set; }
}
