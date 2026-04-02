using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.FabricantDomain.Entities;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CompteurDomain.Entities;

public partial class Compteur : AuditableEntity
{
    public int Id { get; set; }
    public string IdCompteur { get; set; }
    public string NumeroCompteur { get; set; } = null!;
    public string? MarqueCompteur { get; set; }

    public DateTime? DatePremierePose { get; set; }

    public DateTime? DatePoseActuelle { get; set; }

    public string? EnergyProfilePeriod { get; set; }

    public string? CrcFirmware { get; set; }

    public string? VersionFirmware { get; set; }

    public string? VersionFirmwareModem { get; set; }


    public string? Phases { get; set; }

    public string? Tarif { get; set; }

    public string? TechnicalProfilePeriod { get; set; }

    public string? TimeDifference { get; set; }
    public string? TypeOfTransport { get; set; }

    public string? Typecompteur { get; set; }

    public int FabriquantId { get; set; }

    public string? Etatcontacteur { get; set; }

    public double? RapportTC { get; set; }
    public double? RapportTT { get; set; }
    public double? TCNumerateur { get; set; }
    public double? TCDenominateur { get; set; }
    public double? TTNumerateur { get; set; }
    public double? TTDenominateur { get; set; }

    //public virtual ICollection<AssociationKey> AssociationKeys { get; set; } = new List<AssociationKey>();

    public Fabricant Fabriquant { get; set; }
    public virtual ICollection<CompteurEquipement> CompteurEquipement { get; set; }
    public virtual ICollection<CompteurCellule> CompteurCellules { get; set; } = new List<CompteurCellule>();

    public virtual ICollection<CommandeCompteur> CommandeCompteur { get; set; } = new List<CommandeCompteur>();

    //public virtual ICollection<Gxdlmsprofilgenericdetail> Gxdlmsprofilgenericdetail { get; set; } = new List<Gxdlmsprofilgenericdetail>();
    //public virtual ICollection<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevent { get; set; } = new List<Gxdlmsprofilgenericdetailsevent>();

}
