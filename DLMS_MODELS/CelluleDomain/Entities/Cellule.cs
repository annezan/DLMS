using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.PosteDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CelluleDomain.Entities;

public partial class Cellule : AuditableEntity
{
    public int Id { get; set; }

    public string? Type { get; set; }
    public string? ValeurTension { get; set; }
    public string? Libelle { get; set; }
    public string Adresse { get; set; }
    
    // Clé étrangère vers Poste
    public int PosteId { get; set; }
    
    // Navigation property
    public Poste Poste { get; set; }
    
    // Relation many-to-many avec Compteur via CompteurCellule
    public ICollection<CompteurCellule> CelluleCompteurs { get; set; } = new List<CompteurCellule>();
}

