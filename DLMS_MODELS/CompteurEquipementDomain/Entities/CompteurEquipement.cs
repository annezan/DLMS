using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.EquipementDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CompteurEquipementDomain.Entities;

public class CompteurEquipement : AuditableEntity
{
    public int Id { get; set; }
    public int CompteurId { get; set; }

    public int EquipementId { get; set; }

    public Compteur Compteur { get; set; }
    public Equipement Equipement { get; set; }



}
