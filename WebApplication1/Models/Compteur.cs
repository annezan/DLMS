using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Compteur
{
    public string Id { get; set; } = null!;

    public string NumeroCompteur { get; set; } = null!;

    public string? MarqueCompteurLibelle { get; set; }

    public int? MarqueCompteurId { get; set; }

    public string? TypeCompteurLibelle { get; set; }

    public int? TypeCompteurId { get; set; }

    public string? StatutCompteurLibelle { get; set; }

    public int? StatutCompteurId { get; set; }

    public string? EtatCompteurLibelle { get; set; }

    public int? EtatCompteurId { get; set; }

    public int? NombreCadrant { get; set; }

    public int? CoefLecture { get; set; }

    public string? AnneeFabrication { get; set; }

    public int? IndexActuel { get; set; }

    public DateTime? DatePremierePose { get; set; }

    public DateTime? DatePoseActuelle { get; set; }

    public int? Age { get; set; }

    public int? NombreFile { get; set; }

    public string? DonneesPublic { get; set; }

    public string? DonneesRead { get; set; }

    public string? DonneesManaged { get; set; }

    public bool? Status { get; set; }

    public string AddedUser { get; set; } = null!;

    public string? UpdatedUser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? EnergyProfilePeriod { get; set; }

    public string? CrcFirmware { get; set; }

    public string? VersionFirmware { get; set; }

    public string? VersionFirmwareModem { get; set; }

    public string? AdresseIp { get; set; }

    public string? Phases { get; set; }

    public string? Tarif { get; set; }

    public string? TechnicalProfilePeriod { get; set; }

    public string? TimeDifference { get; set; }

    public string? DataConcentrator { get; set; }

    public string? TypeOfTransport { get; set; }

    public string? Typecompteur { get; set; }

    public string? Idfabricant { get; set; }

    public string? Etatcontacteur { get; set; }

    public virtual ICollection<Commandecompteur> Commandecompteurs { get; set; } = new List<Commandecompteur>();

    public virtual ICollection<Gxdlmsprofilgeneric> Gxdlmsprofilgenerics { get; set; } = new List<Gxdlmsprofilgeneric>();

}
