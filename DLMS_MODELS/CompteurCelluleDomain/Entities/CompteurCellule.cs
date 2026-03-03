using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CompteurCelluleDomain.Entities;

public class CompteurCellule : AuditableEntity
{
    public int Id { get; set; }
    public int CompteurId { get; set; }
    public int CelluleId { get; set; }

    public Compteur Compteur { get; set; }
    public Cellule Cellule { get; set; }
}
