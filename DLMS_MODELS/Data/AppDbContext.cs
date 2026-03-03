using System;
using System.Collections.Generic;
using DLMS_MODELS.Models;
using Microsoft.EntityFrameworkCore;

namespace DLMS_MODELS.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abonnement> Abonnements { get; set; }

    public virtual DbSet<AbonnementMoyenPaiement> AbonnementMoyenPaiements { get; set; }

    public virtual DbSet<AccordCommercial> AccordCommercials { get; set; }

    public virtual DbSet<Actionoverthreshold> Actionoverthresholds { get; set; }

    public virtual DbSet<Actionunderthreshold> Actionunderthresholds { get; set; }

    public virtual DbSet<AdressesClient> AdressesClients { get; set; }

    public virtual DbSet<AdressesDemande> AdressesDemandes { get; set; }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<Appareil> Appareils { get; set; }

    public virtual DbSet<AppareilDemande> AppareilDemandes { get; set; }

    public virtual DbSet<Applicationcontext> Applicationcontexts { get; set; }

    public virtual DbSet<AssociationKey> AssociationKeys { get; set; }

    public virtual DbSet<Authenticationmechanism> Authenticationmechanisms { get; set; }

    public virtual DbSet<Avi> Avis { get; set; }

    public virtual DbSet<AvisClientDevisMetre> AvisClientDevisMetres { get; set; }

    public virtual DbSet<AvisClientFraisDemande> AvisClientFraisDemandes { get; set; }

    public virtual DbSet<AvisCompteRenduEnqueteTerrain> AvisCompteRenduEnqueteTerrains { get; set; }

    public virtual DbSet<AvisCompteRenduEnqueteTerrainRemarque> AvisCompteRenduEnqueteTerrainRemarques { get; set; }

    public virtual DbSet<AvisCompteRenduMetre> AvisCompteRenduMetres { get; set; }

    public virtual DbSet<AvisCompteRenduTravaux> AvisCompteRenduTravauxes { get; set; }

    public virtual DbSet<AvisCompteRenduTravauxAction> AvisCompteRenduTravauxActions { get; set; }

    public virtual DbSet<AvisDemande> AvisDemandes { get; set; }

    public virtual DbSet<AvisDemandeMotifRejet> AvisDemandeMotifRejets { get; set; }

    public virtual DbSet<AvisDemandeRemarque> AvisDemandeRemarques { get; set; }

    public virtual DbSet<AvisDevisMetre> AvisDevisMetres { get; set; }

    public virtual DbSet<AvisDevisMetreDocument> AvisDevisMetreDocuments { get; set; }

    public virtual DbSet<AvisDevisMetreMotifRejet> AvisDevisMetreMotifRejets { get; set; }

    public virtual DbSet<AvisDevisMetreRemarque> AvisDevisMetreRemarques { get; set; }

    public virtual DbSet<AvoirClient> AvoirClients { get; set; }

    public virtual DbSet<BilletPiece> BilletPieces { get; set; }

    public virtual DbSet<Billetage> Billetages { get; set; }

    public virtual DbSet<BilletageDetail> BilletageDetails { get; set; }

    public virtual DbSet<BranchementEau> BranchementEaus { get; set; }

    public virtual DbSet<BranchementEauPdl> BranchementEauPdls { get; set; }

    public virtual DbSet<Ca> Cas { get; set; }

    public virtual DbSet<Caisse> Caisses { get; set; }

    public virtual DbSet<CaisseUser> CaisseUsers { get; set; }

    public virtual DbSet<CalibreCompteur> CalibreCompteurs { get; set; }

    public virtual DbSet<Campagne> Campagnes { get; set; }

    public virtual DbSet<CampagneGroupeFacturation> CampagneGroupeFacturations { get; set; }

    public virtual DbSet<CategorieClient> CategorieClients { get; set; }

    public virtual DbSet<CategorieTravaux> CategorieTravauxes { get; set; }

    public virtual DbSet<CautionAbonnement> CautionAbonnements { get; set; }

    public virtual DbSet<ChaineRelance> ChaineRelances { get; set; }

    public virtual DbSet<ChaineRelanceEtape> ChaineRelanceEtapes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<City1> Cities1 { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientPiecesFourny> ClientPiecesFournies { get; set; }

    public virtual DbSet<ClientRolesPersonnesPhysique> ClientRolesPersonnesPhysiques { get; set; }

    public virtual DbSet<CodeObi> CodeObis { get; set; }

    public virtual DbSet<CodeOperation> CodeOperations { get; set; }

    public virtual DbSet<CodeOperationCompte> CodeOperationComptes { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<Commandecompteur> Commandecompteurs { get; set; }

    public virtual DbSet<Compte> Comptes { get; set; }

    public virtual DbSet<CompteRenduEnqueteTerrain> CompteRenduEnqueteTerrains { get; set; }

    public virtual DbSet<CompteRenduEnqueteTerrainIntervenant> CompteRenduEnqueteTerrainIntervenants { get; set; }

    public virtual DbSet<CompteRenduMetre> CompteRenduMetres { get; set; }

    public virtual DbSet<CompteRenduMetreIntervenant> CompteRenduMetreIntervenants { get; set; }

    public virtual DbSet<CompteRenduMetrePointsPiquage> CompteRenduMetrePointsPiquages { get; set; }

    public virtual DbSet<CompteRenduTravaux> CompteRenduTravauxes { get; set; }

    public virtual DbSet<CompteRenduTravauxBilanMateriel> CompteRenduTravauxBilanMateriels { get; set; }

    public virtual DbSet<CompteRenduTravauxIntervenant> CompteRenduTravauxIntervenants { get; set; }

    public virtual DbSet<CompteRenduTravauxMaterielRecupere> CompteRenduTravauxMaterielRecuperes { get; set; }

    public virtual DbSet<Compteur> Compteurs { get; set; }

    public virtual DbSet<CompteurProfil> CompteurProfils { get; set; }

    public virtual DbSet<CompteurProfilParametre> CompteurProfilParametres { get; set; }

    public virtual DbSet<Continent> Continents { get; set; }

    public virtual DbSet<Continent1> Continents1 { get; set; }

    public virtual DbSet<Contrat> Contrats { get; set; }

    public virtual DbSet<ContratSignataire> ContratSignataires { get; set; }

    public virtual DbSet<CoreConfig> CoreConfigs { get; set; }

    public virtual DbSet<CoreLang> CoreLangs { get; set; }

    public virtual DbSet<CoreNotification> CoreNotifications { get; set; }

    public virtual DbSet<CoreParameter> CoreParameters { get; set; }

    public virtual DbSet<CoreTheme> CoreThemes { get; set; }

    public virtual DbSet<CoreTranslation> CoreTranslations { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Country1> Countries1 { get; set; }

    public virtual DbSet<CoutDemande> CoutDemandes { get; set; }

    public virtual DbSet<Dayactionspassive> Dayactionspassives { get; set; }

    public virtual DbSet<Dayprofiletableactive> Dayprofiletableactives { get; set; }

    public virtual DbSet<Dayprofiletablepassive> Dayprofiletablepassives { get; set; }

    public virtual DbSet<Demande> Demandes { get; set; }

    public virtual DbSet<DemandeDocument> DemandeDocuments { get; set; }

    public virtual DbSet<DemandePiecesFourny> DemandePiecesFournies { get; set; }

    public virtual DbSet<DemandeRolesPersonnesPhysique> DemandeRolesPersonnesPhysiques { get; set; }

    public virtual DbSet<Denomination> Denominations { get; set; }

    public virtual DbSet<DevisMetre> DevisMetres { get; set; }

    public virtual DbSet<DevisMetreMateriel> DevisMetreMateriels { get; set; }

    public virtual DbSet<DevisMetreMaterielsDetail> DevisMetreMaterielsDetails { get; set; }

    public virtual DbSet<DevisMetreTravaux> DevisMetreTravauxes { get; set; }

    public virtual DbSet<DevisMetreTravauxAdditionnel> DevisMetreTravauxAdditionnels { get; set; }

    public virtual DbSet<DevisMetreTravauxAdditionnelsDetail> DevisMetreTravauxAdditionnelsDetails { get; set; }

    public virtual DbSet<DevisMetreTravauxDetail> DevisMetreTravauxDetails { get; set; }

    public virtual DbSet<DiametreCompteur> DiametreCompteurs { get; set; }

    public virtual DbSet<DiametreConduite> DiametreConduites { get; set; }

    public virtual DbSet<DiametreTuyau> DiametreTuyaus { get; set; }

    public virtual DbSet<EcritureComptable> EcritureComptables { get; set; }

    public virtual DbSet<EcritureComptableDetail> EcritureComptableDetails { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityParameter> EntityParameters { get; set; }

    public virtual DbSet<EntityType> EntityTypes { get; set; }

    public virtual DbSet<EntityUser> EntityUsers { get; set; }

    public virtual DbSet<EquipeIntervention> EquipeInterventions { get; set; }

    public virtual DbSet<EquipeInterventionAgent> EquipeInterventionAgents { get; set; }

    public virtual DbSet<EquipeInterventionVehicule> EquipeInterventionVehicules { get; set; }

    public virtual DbSet<Etalonnage> Etalonnages { get; set; }

    public virtual DbSet<EtatAbonnement> EtatAbonnements { get; set; }

    public virtual DbSet<EtatBranchement> EtatBranchements { get; set; }

    public virtual DbSet<EtatCompteur> EtatCompteurs { get; set; }

    public virtual DbSet<Extension> Extensions { get; set; }

    public virtual DbSet<ExtensionsBeneficiaire> ExtensionsBeneficiaires { get; set; }

    public virtual DbSet<ExtensionsPointsPiquage> ExtensionsPointsPiquages { get; set; }

    public virtual DbSet<FactureClient> FactureClients { get; set; }

    public virtual DbSet<FactureClientDetail> FactureClientDetails { get; set; }

    public virtual DbSet<Fonction> Fonctions { get; set; }

    public virtual DbSet<Form> Forms { get; set; }

    public virtual DbSet<FraisDemande> FraisDemandes { get; set; }

    public virtual DbSet<Fraude> Fraudes { get; set; }

    public virtual DbSet<FraudeEnqueteur> FraudeEnqueteurs { get; set; }

    public virtual DbSet<FraudeTypeUtilise> FraudeTypeUtilises { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupType> GroupTypes { get; set; }

    public virtual DbSet<GroupeFacturation> GroupeFacturations { get; set; }

    public virtual DbSet<Gxdlmsactionschedule> Gxdlmsactionschedules { get; set; }

    public virtual DbSet<Gxdlmsactionscheduleexecutiontime> Gxdlmsactionscheduleexecutiontimes { get; set; }

    public virtual DbSet<Gxdlmsactivitycalendar> Gxdlmsactivitycalendars { get; set; }

    public virtual DbSet<Gxdlmsassociationlogicalname> Gxdlmsassociationlogicalnames { get; set; }

    public virtual DbSet<Gxdlmsassociationobjectlist> Gxdlmsassociationobjectlists { get; set; }

    public virtual DbSet<Gxdlmsautoconnect> Gxdlmsautoconnects { get; set; }

    public virtual DbSet<Gxdlmsclock> Gxdlmsclocks { get; set; }

    public virtual DbSet<Gxdlmsdatum> Gxdlmsdata { get; set; }

    public virtual DbSet<Gxdlmsdemandregister> Gxdlmsdemandregisters { get; set; }

    public virtual DbSet<Gxdlmsdisconnectcontrol> Gxdlmsdisconnectcontrols { get; set; }

    public virtual DbSet<Gxdlmsextendedregister> Gxdlmsextendedregisters { get; set; }

    public virtual DbSet<Gxdlmsgprssetup> Gxdlmsgprssetups { get; set; }

    public virtual DbSet<Gxdlmsgprssetupdefaultqo> Gxdlmsgprssetupdefaultqos { get; set; }

    public virtual DbSet<Gxdlmsgprssetuprequestedqo> Gxdlmsgprssetuprequestedqos { get; set; }

    public virtual DbSet<Gxdlmsgsmdiagnostic> Gxdlmsgsmdiagnostics { get; set; }

    public virtual DbSet<Gxdlmsgsmdiagnosticadjacentcell> Gxdlmsgsmdiagnosticadjacentcells { get; set; }

    public virtual DbSet<Gxdlmsgsmdiagnosticcellinfo> Gxdlmsgsmdiagnosticcellinfos { get; set; }

    public virtual DbSet<Gxdlmsiechdlcsetup> Gxdlmsiechdlcsetups { get; set; }

    public virtual DbSet<Gxdlmsieclocalportsetup> Gxdlmsieclocalportsetups { get; set; }

    public virtual DbSet<Gxdlmslimiter> Gxdlmslimiters { get; set; }

    public virtual DbSet<Gxdlmslimiteremergencyprofile> Gxdlmslimiteremergencyprofiles { get; set; }

    public virtual DbSet<Gxdlmslimitermonitoredvalue> Gxdlmslimitermonitoredvalues { get; set; }

    public virtual DbSet<Gxdlmsprofilgeneric> Gxdlmsprofilgenerics { get; set; }

    public virtual DbSet<Gxdlmsprofilgenericdetail> Gxdlmsprofilgenericdetails { get; set; }

    public virtual DbSet<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevents { get; set; }

    public virtual DbSet<Gxdlmsregister> Gxdlmsregisters { get; set; }

    public virtual DbSet<Gxdlmssapassignment> Gxdlmssapassignments { get; set; }

    public virtual DbSet<Gxdlmsscripttable> Gxdlmsscripttables { get; set; }

    public virtual DbSet<Gxdlmssecuritysetup> Gxdlmssecuritysetups { get; set; }

    public virtual DbSet<Gxdlmsspecialdaystable> Gxdlmsspecialdaystables { get; set; }

    public virtual DbSet<LiaisonCompteurTransfo> LiaisonCompteurTransfos { get; set; }

    public virtual DbSet<LibelleTop> LibelleTops { get; set; }

    public virtual DbSet<LotRi> LotRis { get; set; }

    public virtual DbSet<Magasin> Magasins { get; set; }

    public virtual DbSet<MagasinStock> MagasinStocks { get; set; }

    public virtual DbSet<MarqueCompteur> MarqueCompteurs { get; set; }

    public virtual DbSet<MarqueVehicule> MarqueVehicules { get; set; }

    public virtual DbSet<MaterielDevi> MaterielDevis { get; set; }

    public virtual DbSet<MaterielsTypeDemandePardiametre> MaterielsTypeDemandePardiametres { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MethodePoseCompteur> MethodePoseCompteurs { get; set; }

    public virtual DbSet<MetreRefection> MetreRefections { get; set; }

    public virtual DbSet<MetreTravauxAdditionnel> MetreTravauxAdditionnels { get; set; }

    public virtual DbSet<MetreTravauxAfaire> MetreTravauxAfaires { get; set; }

    public virtual DbSet<ModeApplicationTarif> ModeApplicationTarifs { get; set; }

    public virtual DbSet<ModeCalcul> ModeCalculs { get; set; }

    public virtual DbSet<ModeFacturation> ModeFacturations { get; set; }

    public virtual DbSet<ModePaiement> ModePaiements { get; set; }

    public virtual DbSet<ModeReglement> ModeReglements { get; set; }

    public virtual DbSet<ModeleVehicule> ModeleVehicules { get; set; }

    public virtual DbSet<Moratoire> Moratoires { get; set; }

    public virtual DbSet<MoratoireDetail> MoratoireDetails { get; set; }

    public virtual DbSet<MotifAnnulation> MotifAnnulations { get; set; }

    public virtual DbSet<MotifDemande> MotifDemandes { get; set; }

    public virtual DbSet<MotifImpossibilite> MotifImpossibilites { get; set; }

    public virtual DbSet<MotifPoseCompteur> MotifPoseCompteurs { get; set; }

    public virtual DbSet<MotifRejet> MotifRejets { get; set; }

    public virtual DbSet<MotifReleve> MotifReleves { get; set; }

    public virtual DbSet<MoyenPaiement> MoyenPaiements { get; set; }

    public virtual DbSet<Nationnalite> Nationnalites { get; set; }

    public virtual DbSet<NatureBranchement> NatureBranchements { get; set; }

    public virtual DbSet<NatureConduite> NatureConduites { get; set; }

    public virtual DbSet<NaturePiece> NaturePieces { get; set; }

    public virtual DbSet<NatureTuyau> NatureTuyaus { get; set; }

    public virtual DbSet<Object> Objects { get; set; }

    public virtual DbSet<Objectrelation> Objectrelations { get; set; }

    public virtual DbSet<OutputsModel> OutputsModels { get; set; }

    public virtual DbSet<Paiement> Paiements { get; set; }

    public virtual DbSet<PaiementMoyensUtilise> PaiementMoyensUtilises { get; set; }

    public virtual DbSet<ParamCaution> ParamCautions { get; set; }

    public virtual DbSet<PartenairePaiement> PartenairePaiements { get; set; }

    public virtual DbSet<Pdl> Pdls { get; set; }

    public virtual DbSet<Periode> Periodes { get; set; }

    public virtual DbSet<PeriodeFacturation> PeriodeFacturations { get; set; }

    public virtual DbSet<PeriodiciteMoratoire> PeriodiciteMoratoires { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PersonnesMoralesLiee> PersonnesMoralesLiees { get; set; }

    public virtual DbSet<PersonnesMoralesPiece> PersonnesMoralesPieces { get; set; }

    public virtual DbSet<PersonnesPhysiquePiece> PersonnesPhysiquePieces { get; set; }

    public virtual DbSet<PersonnesPhysiquesLiee> PersonnesPhysiquesLiees { get; set; }

    public virtual DbSet<PhaseCompteur> PhaseCompteurs { get; set; }

    public virtual DbSet<PointsPiquage> PointsPiquages { get; set; }

    public virtual DbSet<PoseCompteur> PoseCompteurs { get; set; }

    public virtual DbSet<Poste> Postes { get; set; }

    public virtual DbSet<Postetransformation> Postetransformations { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileMenu> ProfileMenus { get; set; }

    public virtual DbSet<ProfilePermission> ProfilePermissions { get; set; }

    public virtual DbSet<ProgrammationTravaux> ProgrammationTravauxes { get; set; }

    public virtual DbSet<PuissanceInstallee> PuissanceInstallees { get; set; }

    public virtual DbSet<PuissanceSouscrite> PuissanceSouscrites { get; set; }

    public virtual DbSet<Quartier> Quartiers { get; set; }

    public virtual DbSet<RechercheTarif> RechercheTarifs { get; set; }

    public virtual DbSet<Redevance> Redevances { get; set; }

    public virtual DbSet<ReglageCompteur> ReglageCompteurs { get; set; }

    public virtual DbSet<Reglement> Reglements { get; set; }

    public virtual DbSet<Regroupement> Regroupements { get; set; }

    public virtual DbSet<Relance> Relances { get; set; }

    public virtual DbSet<RelanceClient> RelanceClients { get; set; }

    public virtual DbSet<RelatedEntity> RelatedEntities { get; set; }

    public virtual DbSet<RelationType> RelationTypes { get; set; }

    public virtual DbSet<Releve> Releves { get; set; }

    public virtual DbSet<ReleveCa> ReleveCas { get; set; }

    public virtual DbSet<Remarque> Remarques { get; set; }

    public virtual DbSet<RemiseBanqueCaisse> RemiseBanqueCaisses { get; set; }

    public virtual DbSet<RemiseBanqueCaisseDetail> RemiseBanqueCaisseDetails { get; set; }

    public virtual DbSet<RoleAgent> RoleAgents { get; set; }

    public virtual DbSet<RolesPersonnePhysique> RolesPersonnePhysiques { get; set; }

    public virtual DbSet<RubriqueFacture> RubriqueFactures { get; set; }

    public virtual DbSet<RubriqueFraude> RubriqueFraudes { get; set; }

    public virtual DbSet<Sapassignment> Sapassignments { get; set; }

    public virtual DbSet<SchemaComptable> SchemaComptables { get; set; }

    public virtual DbSet<Script> Scripts { get; set; }

    public virtual DbSet<Seasonprofileactive> Seasonprofileactives { get; set; }

    public virtual DbSet<Seasonprofilepassive> Seasonprofilepassives { get; set; }

    public virtual DbSet<Secteur> Secteurs { get; set; }

    public virtual DbSet<SortieMaterielsTravaux> SortieMaterielsTravauxes { get; set; }

    public virtual DbSet<SortieMaterielsTravauxDetail> SortieMaterielsTravauxDetails { get; set; }

    public virtual DbSet<Specialdayentry> Specialdayentries { get; set; }

    public virtual DbSet<StatutAbonnement> StatutAbonnements { get; set; }

    public virtual DbSet<StatutAvoir> StatutAvoirs { get; set; }

    public virtual DbSet<StatutBranchement> StatutBranchements { get; set; }

    public virtual DbSet<StatutCampagne> StatutCampagnes { get; set; }

    public virtual DbSet<StatutCompteur> StatutCompteurs { get; set; }

    public virtual DbSet<StatutDemande> StatutDemandes { get; set; }

    public virtual DbSet<StatutFacture> StatutFactures { get; set; }

    public virtual DbSet<StatutFrai> StatutFrais { get; set; }

    public virtual DbSet<StatutJuridique> StatutJuridiques { get; set; }

    public virtual DbSet<StatutMoratoire> StatutMoratoires { get; set; }

    public virtual DbSet<StatutPaiement> StatutPaiements { get; set; }

    public virtual DbSet<StatutReclamation> StatutReclamations { get; set; }

    public virtual DbSet<StatutReglement> StatutReglements { get; set; }

    public virtual DbSet<StatutReleve> StatutReleves { get; set; }

    public virtual DbSet<StatutScelle> StatutScelles { get; set; }

    public virtual DbSet<StatutTransfert> StatutTransferts { get; set; }

    public virtual DbSet<StatutTravaux> StatutTravauxes { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<StepsProfile> StepsProfiles { get; set; }

    public virtual DbSet<StepsTransition> StepsTransitions { get; set; }

    public virtual DbSet<TarifFacturation> TarifFacturations { get; set; }

    public virtual DbSet<TarifFacturationDetail> TarifFacturationDetails { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TasksStatus> TasksStatuses { get; set; }

    public virtual DbSet<Taxe> Taxes { get; set; }

    public virtual DbSet<Tournee> Tournees { get; set; }

    public virtual DbSet<TourneeAgent> TourneeAgents { get; set; }

    public virtual DbSet<TypeAccord> TypeAccords { get; set; }

    public virtual DbSet<TypeActionDemande> TypeActionDemandes { get; set; }

    public virtual DbSet<TypeActionEnqueteSatisfaction> TypeActionEnqueteSatisfactions { get; set; }

    public virtual DbSet<TypeActionInventaire> TypeActionInventaires { get; set; }

    public virtual DbSet<TypeActionRecouvrement> TypeActionRecouvrements { get; set; }

    public virtual DbSet<TypeActiviteCompteClient> TypeActiviteCompteClients { get; set; }

    public virtual DbSet<TypeAvisDevisMetre> TypeAvisDevisMetres { get; set; }

    public virtual DbSet<TypeBilletPiece> TypeBilletPieces { get; set; }

    public virtual DbSet<TypeBilletage> TypeBilletages { get; set; }

    public virtual DbSet<TypeBranchement> TypeBranchements { get; set; }

    public virtual DbSet<TypeBranchementParProduit> TypeBranchementParProduits { get; set; }

    public virtual DbSet<TypeCa> TypeCas { get; set; }

    public virtual DbSet<TypeCaisse> TypeCaisses { get; set; }

    public virtual DbSet<TypeCampagne> TypeCampagnes { get; set; }

    public virtual DbSet<TypeCarburantVehicule> TypeCarburantVehicules { get; set; }

    public virtual DbSet<TypeCentre> TypeCentres { get; set; }

    public virtual DbSet<TypeClient> TypeClients { get; set; }

    public virtual DbSet<TypeComptage> TypeComptages { get; set; }

    public virtual DbSet<TypeCompte> TypeComptes { get; set; }

    public virtual DbSet<TypeCompteRendu> TypeCompteRendus { get; set; }

    public virtual DbSet<TypeCompteur> TypeCompteurs { get; set; }

    public virtual DbSet<TypeCompteurComptage> TypeCompteurComptages { get; set; }

    public virtual DbSet<TypeConsommation> TypeConsommations { get; set; }

    public virtual DbSet<TypeContrat> TypeContrats { get; set; }

    public virtual DbSet<TypeControle> TypeControles { get; set; }

    public virtual DbSet<TypeCoupure> TypeCoupures { get; set; }

    public virtual DbSet<TypeDemande> TypeDemandes { get; set; }

    public virtual DbSet<TypeDemandePiece> TypeDemandePieces { get; set; }

    public virtual DbSet<TypeDemandeProcess> TypeDemandeProcesses { get; set; }

    public virtual DbSet<TypeDemandeProduit> TypeDemandeProduits { get; set; }

    public virtual DbSet<TypeDemandeSpecification> TypeDemandeSpecifications { get; set; }

    public virtual DbSet<TypeDepannage> TypeDepannages { get; set; }

    public virtual DbSet<TypeDevise> TypeDevises { get; set; }

    public virtual DbSet<TypeDisjoncteur> TypeDisjoncteurs { get; set; }

    public virtual DbSet<TypeDisjoncteurParCalibreCompteur> TypeDisjoncteurParCalibreCompteurs { get; set; }

    public virtual DbSet<TypeDocument> TypeDocuments { get; set; }

    public virtual DbSet<TypeFacturation> TypeFacturations { get; set; }

    public virtual DbSet<TypeFacturationAbonne> TypeFacturationAbonnes { get; set; }

    public virtual DbSet<TypeFacture> TypeFactures { get; set; }

    public virtual DbSet<TypeFrai> TypeFrais { get; set; }

    public virtual DbSet<TypeFraude> TypeFraudes { get; set; }

    public virtual DbSet<TypeInstallation> TypeInstallations { get; set; }

    public virtual DbSet<TypeLienproduit> TypeLienproduits { get; set; }

    public virtual DbSet<TypeLienredevance> TypeLienredevances { get; set; }

    public virtual DbSet<TypeLot> TypeLots { get; set; }

    public virtual DbSet<TypeMagasin> TypeMagasins { get; set; }

    public virtual DbSet<TypeMateriel> TypeMateriels { get; set; }

    public virtual DbSet<TypeMessage> TypeMessages { get; set; }

    public virtual DbSet<TypeNotification> TypeNotifications { get; set; }

    public virtual DbSet<TypeObjetReglement> TypeObjetReglements { get; set; }

    public virtual DbSet<TypePanne> TypePannes { get; set; }

    public virtual DbSet<TypePartenairePaiement> TypePartenairePaiements { get; set; }

    public virtual DbSet<TypePiece> TypePieces { get; set; }

    public virtual DbSet<TypeProbleme> TypeProblemes { get; set; }

    public virtual DbSet<TypeProvision> TypeProvisions { get; set; }

    public virtual DbSet<TypeReclamation> TypeReclamations { get; set; }

    public virtual DbSet<TypeRecour> TypeRecours { get; set; }

    public virtual DbSet<TypeRedevance> TypeRedevances { get; set; }

    public virtual DbSet<TypeRefection> TypeRefections { get; set; }

    public virtual DbSet<TypeRemise> TypeRemises { get; set; }

    public virtual DbSet<TypeRemiseBanque> TypeRemiseBanques { get; set; }

    public virtual DbSet<TypeTarif> TypeTarifs { get; set; }

    public virtual DbSet<TypeTaxe> TypeTaxes { get; set; }

    public virtual DbSet<TypeTimbre> TypeTimbres { get; set; }

    public virtual DbSet<TypeTransformateur> TypeTransformateurs { get; set; }

    public virtual DbSet<TypeTravaux> TypeTravauxes { get; set; }

    public virtual DbSet<TypeTravauxAdditionnel> TypeTravauxAdditionnels { get; set; }

    public virtual DbSet<TypeTuyau> TypeTuyaus { get; set; }

    public virtual DbSet<Typecommande> Typecommandes { get; set; }

    public virtual DbSet<UniteComptage> UniteComptages { get; set; }

    public virtual DbSet<UsagePrincipal> UsagePrincipals { get; set; }

    public virtual DbSet<UsageSecondaire> UsageSecondaires { get; set; }

    public virtual DbSet<UsageSecondaireParTypeClient> UsageSecondaireParTypeClients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<VariableTarif> VariableTarifs { get; set; }

    public virtual DbSet<Vehicule> Vehicules { get; set; }

    public virtual DbSet<Weekprofiletableactive> Weekprofiletableactives { get; set; }

    public virtual DbSet<Weekprofiletablepassive> Weekprofiletablepassives { get; set; }

    public virtual DbSet<WsCoupon> WsCoupons { get; set; }

    public virtual DbSet<WsLocality> WsLocalities { get; set; }

    public virtual DbSet<WsLocality1> WsLocalities1 { get; set; }

    public virtual DbSet<WsUserAddress> WsUserAddresses { get; set; }

    public virtual DbSet<Xdlmscontextinfo> Xdlmscontextinfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=149.202.45.76;Port=5444;Database=sgci;Username=postgres;Password=Gas@4766;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("enum_EcritureComptableDetail_sens", new[] { "D", "C" })
            .HasPostgresEnum("sc_sge", "enum_CodeOperationCompte_sens", new[] { "C", "D" })
            .HasPostgresEnum("sc_sge", "enum_EcritureComptableDetail_sens", new[] { "D", "C" })
            .HasPostgresEnum("sc_sge", "enum_Relance_type_relance", new[] { "AUTO", "MANUEL" })
            .HasPostgresEnum("sc_sge", "enum_SchemaComptable_sens", new[] { "C", "D" });

        modelBuilder.Entity<Abonnement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Abonnement_pkey");

            entity.ToTable("Abonnement", "sc_sge");

            entity.HasIndex(e => e.NumeroAbonnement, "Abonnement_numero_abonnement_key").IsUnique();

            entity.HasIndex(e => e.NumeroAbonnement, "Abonnement_numero_abonnement_key1").IsUnique();

            entity.HasIndex(e => e.NumeroAbonnement, "Abonnement_numero_abonnement_key2").IsUnique();

            entity.HasIndex(e => e.NumeroAbonnement, "Abonnement_numero_abonnement_key3").IsUnique();

            entity.HasIndex(e => e.NumeroAbonnement, "Abonnement_numero_abonnement_key4").IsUnique();

            entity.HasIndex(e => e.NumeroAbonnement, "Abonnement_numero_abonnement_key5").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BranchementEauPdlId).HasColumnName("branchement_eau_pdl_id");
            entity.Property(e => e.Caution).HasColumnName("caution");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.ChaineRelanceId).HasColumnName("chaine_relance_id");
            entity.Property(e => e.ChaineRelanceLibelle)
                .HasMaxLength(255)
                .HasColumnName("chaine_relance_libelle");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateAbonnement).HasColumnName("date_abonnement");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.ExempteRelance).HasColumnName("exempte_relance");
            entity.Property(e => e.GroupeFacturationId).HasColumnName("groupe_facturation_id");
            entity.Property(e => e.GroupeFacturationLibelle)
                .HasMaxLength(255)
                .HasColumnName("groupe_facturation_libelle");
            entity.Property(e => e.ModeFacturationId).HasColumnName("mode_facturation_id");
            entity.Property(e => e.ModeFacturationLibelle)
                .HasMaxLength(255)
                .HasColumnName("mode_facturation_libelle");
            entity.Property(e => e.MoyenPaiementId).HasColumnName("moyen_paiement_id");
            entity.Property(e => e.MoyenPaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("moyen_paiement_libelle");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasColumnName("nom_centre");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NumeroAbonnement)
                .HasMaxLength(255)
                .HasColumnName("numero_abonnement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.Ordre).HasColumnName("ordre");
            entity.Property(e => e.PeriodeFacturationId).HasColumnName("periode_facturation_id");
            entity.Property(e => e.PeriodeFacturationLibelle)
                .HasMaxLength(255)
                .HasColumnName("periode_facturation_libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.ProduitLibelle)
                .HasMaxLength(255)
                .HasColumnName("produit_libelle");
            entity.Property(e => e.RegroupementId)
                .HasMaxLength(255)
                .HasColumnName("regroupement_id");
            entity.Property(e => e.RegroupementLibelle)
                .HasMaxLength(255)
                .HasColumnName("regroupement_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutAbonnementId).HasColumnName("statut_abonnement_id");
            entity.Property(e => e.StatutAbonnementLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_abonnement_libelle");
            entity.Property(e => e.TiersPayeurMoralId)
                .HasMaxLength(255)
                .HasColumnName("tiers_payeur_moral_id");
            entity.Property(e => e.TiersPayeurMoralNom)
                .HasMaxLength(255)
                .HasColumnName("tiers_payeur_moral_nom");
            entity.Property(e => e.TiersPayeurPhysiqueId)
                .HasMaxLength(255)
                .HasColumnName("tiers_payeur_physique_id");
            entity.Property(e => e.TiersPayeurPhysiqueNom)
                .HasMaxLength(255)
                .HasColumnName("tiers_payeur_physique_nom");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UsageSecondaireId).HasColumnName("usage_secondaire_id");
            entity.Property(e => e.UsageSecondaireLibelle)
                .HasMaxLength(255)
                .HasColumnName("usage_secondaire_libelle");

            entity.HasOne(d => d.BranchementEauPdl).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.BranchementEauPdlId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Abonnement_branchement_eau_pdl_id_fkey");

            entity.HasOne(d => d.ChaineRelance).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.ChaineRelanceId)
                .HasConstraintName("Abonnement_chaine_relance_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("Abonnement_client_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Abonnement_demande_id_fkey");

            entity.HasOne(d => d.GroupeFacturation).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.GroupeFacturationId)
                .HasConstraintName("Abonnement_groupe_facturation_id_fkey");

            entity.HasOne(d => d.ModeFacturation).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.ModeFacturationId)
                .HasConstraintName("Abonnement_mode_facturation_id_fkey");

            entity.HasOne(d => d.MoyenPaiement).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.MoyenPaiementId)
                .HasConstraintName("Abonnement_moyen_paiement_id_fkey");

            entity.HasOne(d => d.PeriodeFacturation).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.PeriodeFacturationId)
                .HasConstraintName("Abonnement_periode_facturation_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Abonnement_produit_id_fkey");

            entity.HasOne(d => d.Regroupement).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.RegroupementId)
                .HasConstraintName("Abonnement_regroupement_id_fkey");

            entity.HasOne(d => d.StatutAbonnement).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.StatutAbonnementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Abonnement_statut_abonnement_id_fkey");

            entity.HasOne(d => d.TiersPayeurMoral).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.TiersPayeurMoralId)
                .HasConstraintName("Abonnement_tiers_payeur_moral_id_fkey");

            entity.HasOne(d => d.TiersPayeurPhysique).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.TiersPayeurPhysiqueId)
                .HasConstraintName("Abonnement_tiers_payeur_physique_id_fkey");

            entity.HasOne(d => d.UsageSecondaire).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.UsageSecondaireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Abonnement_usage_secondaire_id_fkey");
        });

        modelBuilder.Entity<AbonnementMoyenPaiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AbonnementMoyenPaiement_pkey");

            entity.ToTable("AbonnementMoyenPaiement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbonnementId)
                .HasMaxLength(255)
                .HasColumnName("abonnement_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MoyenPaiementId).HasColumnName("moyen_paiement_id");
            entity.Property(e => e.MoyenPaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("moyen_paiement_libelle");
            entity.Property(e => e.ReferenceMoyenPaiement)
                .HasMaxLength(255)
                .HasColumnName("reference_moyen_paiement");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Abonnement).WithMany(p => p.AbonnementMoyenPaiements)
                .HasForeignKey(d => d.AbonnementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AbonnementMoyenPaiement_abonnement_id_fkey");

            entity.HasOne(d => d.MoyenPaiement).WithMany(p => p.AbonnementMoyenPaiements)
                .HasForeignKey(d => d.MoyenPaiementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AbonnementMoyenPaiement_moyen_paiement_id_fkey");
        });

        modelBuilder.Entity<AccordCommercial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AccordCommercial_pkey");

            entity.ToTable("AccordCommercial", "sc_sge");

            entity.HasIndex(e => e.NumeroAccord, "AccordCommercial_numero_accord_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateAccord).HasColumnName("date_accord");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FactureId)
                .HasMaxLength(255)
                .HasColumnName("facture_id");
            entity.Property(e => e.MontantMiniPaye).HasColumnName("montant_mini_paye");
            entity.Property(e => e.NumeroAccord)
                .HasMaxLength(255)
                .HasColumnName("numero_accord");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroFacture)
                .HasMaxLength(255)
                .HasColumnName("numero_facture");
            entity.Property(e => e.NumeroRecuPaiement)
                .HasMaxLength(255)
                .HasColumnName("numero_recu_paiement");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeAccordId).HasColumnName("type_accord_id");
            entity.Property(e => e.TypeAccordLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_accord_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Client).WithMany(p => p.AccordCommercials)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AccordCommercial_client_id_fkey");

            entity.HasOne(d => d.Facture).WithMany(p => p.AccordCommercials)
                .HasForeignKey(d => d.FactureId)
                .HasConstraintName("AccordCommercial_facture_id_fkey");

            entity.HasOne(d => d.TypeAccord).WithMany(p => p.AccordCommercials)
                .HasForeignKey(d => d.TypeAccordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AccordCommercial_type_accord_id_fkey");
        });

        modelBuilder.Entity<Actionoverthreshold>(entity =>
        {
            entity.HasKey(e => e.Actionoverid).HasName("actionoverthreshold_pkey");

            entity.ToTable("actionoverthreshold", "sc_sge");

            entity.Property(e => e.Actionoverid).HasColumnName("actionoverid");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Gxdlmslimiterid).HasColumnName("gxdlmslimiterid");
            entity.Property(e => e.Logicalname)
                .HasMaxLength(50)
                .HasColumnName("logicalname");
            entity.Property(e => e.Scriptselector).HasColumnName("scriptselector");

            entity.HasOne(d => d.Gxdlmslimiter).WithMany(p => p.Actionoverthresholds)
                .HasForeignKey(d => d.Gxdlmslimiterid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("actionoverthreshold_gxdlmslimiterid_fkey");
        });

        modelBuilder.Entity<Actionunderthreshold>(entity =>
        {
            entity.HasKey(e => e.Actionunderid).HasName("actionunderthreshold_pkey");

            entity.ToTable("actionunderthreshold", "sc_sge");

            entity.Property(e => e.Actionunderid).HasColumnName("actionunderid");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Gxdlmslimiterid).HasColumnName("gxdlmslimiterid");
            entity.Property(e => e.Logicalname)
                .HasMaxLength(50)
                .HasColumnName("logicalname");
            entity.Property(e => e.Scriptselector).HasColumnName("scriptselector");

            entity.HasOne(d => d.Gxdlmslimiter).WithMany(p => p.Actionunderthresholds)
                .HasForeignKey(d => d.Gxdlmslimiterid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("actionunderthreshold_gxdlmslimiterid_fkey");
        });

        modelBuilder.Entity<AdressesClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AdressesClient_pkey");

            entity.ToTable("AdressesClient", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdresseComplementaire)
                .HasMaxLength(255)
                .HasColumnName("adresse_complementaire");
            entity.Property(e => e.Carre)
                .HasMaxLength(255)
                .HasColumnName("carre");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CommuneId)
                .HasMaxLength(255)
                .HasColumnName("commune_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Etage)
                .HasMaxLength(255)
                .HasColumnName("etage");
            entity.Property(e => e.Ilot)
                .HasMaxLength(255)
                .HasColumnName("ilot");
            entity.Property(e => e.Lot)
                .HasMaxLength(255)
                .HasColumnName("lot");
            entity.Property(e => e.Porte)
                .HasMaxLength(255)
                .HasColumnName("porte");
            entity.Property(e => e.QuartierId).HasColumnName("quartier_id");
            entity.Property(e => e.Rue)
                .HasMaxLength(255)
                .HasColumnName("rue");
            entity.Property(e => e.SecteurId).HasColumnName("secteur_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VilleId)
                .HasMaxLength(255)
                .HasColumnName("ville_id");

            entity.HasOne(d => d.Client).WithMany(p => p.AdressesClients)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("AdressesClient_client_id_fkey");

            entity.HasOne(d => d.Commune).WithMany(p => p.AdressesClients)
                .HasForeignKey(d => d.CommuneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdressesClient_commune_id_fkey");

            entity.HasOne(d => d.Quartier).WithMany(p => p.AdressesClients)
                .HasForeignKey(d => d.QuartierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdressesClient_quartier_id_fkey");

            entity.HasOne(d => d.Secteur).WithMany(p => p.AdressesClients)
                .HasForeignKey(d => d.SecteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdressesClient_secteur_id_fkey");

            entity.HasOne(d => d.Ville).WithMany(p => p.AdressesClients)
                .HasForeignKey(d => d.VilleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdressesClient_ville_id_fkey");
        });

        modelBuilder.Entity<AdressesDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AdressesDemande_pkey");

            entity.ToTable("AdressesDemande", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdresseClientId).HasColumnName("adresse_client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AdresseClient).WithMany(p => p.AdressesDemandes)
                .HasForeignKey(d => d.AdresseClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdressesDemande_adresse_client_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.AdressesDemandes)
                .HasForeignKey(d => d.DemandeId)
                .HasConstraintName("AdressesDemande_demande_id_fkey");
        });

        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Agent_pkey");

            entity.ToTable("Agent", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FonctionId).HasColumnName("fonction_id");
            entity.Property(e => e.FonctionLibelle)
                .HasMaxLength(255)
                .HasColumnName("fonction_libelle");
            entity.Property(e => e.Matricule)
                .HasMaxLength(255)
                .HasColumnName("matricule");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasColumnName("nom_centre");
            entity.Property(e => e.Prenoms)
                .HasMaxLength(255)
                .HasColumnName("prenoms");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Telephone)
                .HasMaxLength(255)
                .HasColumnName("telephone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Whatsapp)
                .HasMaxLength(255)
                .HasColumnName("whatsapp");

            entity.HasOne(d => d.Fonction).WithMany(p => p.Agents)
                .HasForeignKey(d => d.FonctionId)
                .HasConstraintName("Agent_fonction_id_fkey");
        });

        modelBuilder.Entity<Appareil>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Appareil_pkey");

            entity.ToTable("Appareil", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Puissance).HasColumnName("puissance");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TempsUtilisation).HasColumnName("temps_utilisation");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<AppareilDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AppareilDemande_pkey");

            entity.ToTable("AppareilDemande", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AppareilId).HasColumnName("appareil_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Appareil).WithMany(p => p.AppareilDemandes)
                .HasForeignKey(d => d.AppareilId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AppareilDemande_appareil_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.AppareilDemandes)
                .HasForeignKey(d => d.DemandeId)
                .HasConstraintName("AppareilDemande_demande_id_fkey");
        });

        modelBuilder.Entity<Applicationcontext>(entity =>
        {
            entity.HasKey(e => e.Contextid).HasName("applicationcontext_pkey");

            entity.ToTable("applicationcontext", "sc_sge");

            entity.Property(e => e.Contextid).HasColumnName("contextid");
            entity.Property(e => e.Applicationcontext1).HasColumnName("applicationcontext");
            entity.Property(e => e.Associationid).HasColumnName("associationid");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Countryname).HasColumnName("countryname");
            entity.Property(e => e.Dlmsua).HasColumnName("dlmsua");
            entity.Property(e => e.Identifiedorganization).HasColumnName("identifiedorganization");
            entity.Property(e => e.Jointisoctt).HasColumnName("jointisoctt");

            entity.HasOne(d => d.Association).WithMany(p => p.Applicationcontexts)
                .HasForeignKey(d => d.Associationid)
                .HasConstraintName("fk_association_context");
        });

        modelBuilder.Entity<AssociationKey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AssociationKeys_pkey");

            entity.ToTable("AssociationKeys", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompteurId).HasMaxLength(255);
            entity.Property(e => e.DateCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_created");
            entity.Property(e => e.Keyname)
                .HasMaxLength(255)
                .HasColumnName("keyname");
            entity.Property(e => e.Levelauthentication)
                .HasMaxLength(50)
                .HasColumnName("levelauthentication");
            entity.Property(e => e.Pwd)
                .HasMaxLength(255)
                .HasColumnName("pwd");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.Uniqueid)
                .HasMaxLength(255)
                .HasColumnName("uniqueid");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");

            entity.HasOne(d => d.Compteur).WithMany(p => p.AssociationKeys)
                .HasForeignKey(d => d.CompteurId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("AssociationId_Compteur_fkey");
        });

        modelBuilder.Entity<Authenticationmechanism>(entity =>
        {
            entity.HasKey(e => e.Mechanismid).HasName("authenticationmechanism_pkey");

            entity.ToTable("authenticationmechanism", "sc_sge");

            entity.Property(e => e.Mechanismid).HasColumnName("mechanismid");
            entity.Property(e => e.Associationid).HasColumnName("associationid");
            entity.Property(e => e.Authenticationmechanismname).HasColumnName("authenticationmechanismname");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Countryname).HasColumnName("countryname");
            entity.Property(e => e.Dlmsua).HasColumnName("dlmsua");
            entity.Property(e => e.Identifiedorganization).HasColumnName("identifiedorganization");
            entity.Property(e => e.Jointisoctt).HasColumnName("jointisoctt");

            entity.HasOne(d => d.Association).WithMany(p => p.Authenticationmechanisms)
                .HasForeignKey(d => d.Associationid)
                .HasConstraintName("fk_authentication");
        });

        modelBuilder.Entity<Avi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Avis_pkey");

            entity.ToTable("Avis", "sc_sge");

            entity.HasIndex(e => e.Code, "avis_code_idx");

            entity.HasIndex(e => e.Famille, "avis_famille_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<AvisClientDevisMetre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisClientDevisMetre_pkey");

            entity.ToTable("AvisClientDevisMetre", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.AvisLibelle)
                .HasMaxLength(255)
                .HasColumnName("avis_libelle");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Commentaire).HasColumnName("commentaire");
            entity.Property(e => e.ConfirmDemandeModif).HasColumnName("confirm_demande_modif");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DevisId)
                .HasMaxLength(255)
                .HasColumnName("devis_id");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.ModificationDemandee).HasColumnName("modification_demandee");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.MoyenPaiementId).HasColumnName("moyen_paiement_id");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisClientDevisMetres)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisClientDevisMetre_avis_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.AvisClientDevisMetres)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisClientDevisMetre_client_id_fkey");

            entity.HasOne(d => d.Devis).WithMany(p => p.AvisClientDevisMetres)
                .HasForeignKey(d => d.DevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisClientDevisMetre_devis_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.AvisClientDevisMetres)
                .HasForeignKey(d => d.MotifRejetId)
                .HasConstraintName("AvisClientDevisMetre_motif_rejet_id_fkey");

            entity.HasOne(d => d.MoyenPaiement).WithMany(p => p.AvisClientDevisMetres)
                .HasForeignKey(d => d.MoyenPaiementId)
                .HasConstraintName("AvisClientDevisMetre_moyen_paiement_id_fkey");
        });

        modelBuilder.Entity<AvisClientFraisDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisClientFraisDemande_pkey");

            entity.ToTable("AvisClientFraisDemande", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.AvisLibelle)
                .HasMaxLength(255)
                .HasColumnName("avis_libelle");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Commentaire).HasColumnName("commentaire");
            entity.Property(e => e.ConfirmDemandeModif).HasColumnName("confirm_demande_modif");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.FraisDemandeId)
                .HasMaxLength(255)
                .HasColumnName("frais_demande_id");
            entity.Property(e => e.ModificationDemandee).HasColumnName("modification_demandee");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisClientFraisDemandes)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisClientFraisDemande_avis_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.AvisClientFraisDemandes)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisClientFraisDemande_client_id_fkey");

            entity.HasOne(d => d.FraisDemande).WithMany(p => p.AvisClientFraisDemandes)
                .HasForeignKey(d => d.FraisDemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisClientFraisDemande_frais_demande_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.AvisClientFraisDemandes)
                .HasForeignKey(d => d.MotifRejetId)
                .HasConstraintName("AvisClientFraisDemande_motif_rejet_id_fkey");
        });

        modelBuilder.Entity<AvisCompteRenduEnqueteTerrain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisCompteRenduEnqueteTerrain_pkey");

            entity.ToTable("AvisCompteRenduEnqueteTerrain", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Annotations)
                .HasMaxLength(255)
                .HasColumnName("annotations");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.EtatBranchementId).HasColumnName("etat_branchement_id");
            entity.Property(e => e.LibelleAvis)
                .HasMaxLength(255)
                .HasColumnName("libelle_avis");
            entity.Property(e => e.MatriculeActeur)
                .HasMaxLength(255)
                .HasColumnName("matricule_acteur");
            entity.Property(e => e.NomActeur)
                .HasMaxLength(255)
                .HasColumnName("nom_acteur");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisCompteRenduEnqueteTerrains)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduEnqueteTerrain_avis_id_fkey");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.AvisCompteRenduEnqueteTerrains)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduEnqueteTerrain_compte_rendu_id_fkey");
        });

        modelBuilder.Entity<AvisCompteRenduEnqueteTerrainRemarque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisCompteRenduEnqueteTerrainRemarque_pkey");

            entity.ToTable("AvisCompteRenduEnqueteTerrainRemarque", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisCompteRenduId)
                .HasMaxLength(255)
                .HasColumnName("avis_compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.RemarqueId).HasColumnName("remarque_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AvisCompteRendu).WithMany(p => p.AvisCompteRenduEnqueteTerrainRemarques)
                .HasForeignKey(d => d.AvisCompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduEnqueteTerrainRemarq_avis_compte_rendu_id_fkey10");

            entity.HasOne(d => d.Remarque).WithMany(p => p.AvisCompteRenduEnqueteTerrainRemarques)
                .HasForeignKey(d => d.RemarqueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduEnqueteTerrainRemarque_remarque_id_fkey");
        });

        modelBuilder.Entity<AvisCompteRenduMetre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisCompteRenduMetre_pkey");

            entity.ToTable("AvisCompteRenduMetre", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Annotations)
                .HasMaxLength(255)
                .HasColumnName("annotations");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.LibelleAvis)
                .HasMaxLength(255)
                .HasColumnName("libelle_avis");
            entity.Property(e => e.MatriculeActeur)
                .HasMaxLength(255)
                .HasColumnName("matricule_acteur");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.NomActeur)
                .HasMaxLength(255)
                .HasColumnName("nom_acteur");
            entity.Property(e => e.NumeroSchema)
                .HasMaxLength(255)
                .HasColumnName("numero_schema");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlSchema)
                .HasMaxLength(255)
                .HasColumnName("url_schema");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisCompteRenduMetres)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduMetre_avis_id_fkey");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.AvisCompteRenduMetres)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduMetre_compte_rendu_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.AvisCompteRenduMetres)
                .HasForeignKey(d => d.MotifRejetId)
                .HasConstraintName("AvisCompteRenduMetre_motif_rejet_id_fkey");
        });

        modelBuilder.Entity<AvisCompteRenduTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisCompteRenduTravaux_pkey");

            entity.ToTable("AvisCompteRenduTravaux", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.AvisLibelle)
                .HasMaxLength(255)
                .HasColumnName("avis_libelle");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Commentaire)
                .HasMaxLength(255)
                .HasColumnName("commentaire");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateAvis).HasColumnName("date_avis");
            entity.Property(e => e.DateTravaux).HasColumnName("date_travaux");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutTravauxId).HasColumnName("statut_travaux_id");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisCompteRenduTravauxes)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduTravaux_avis_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.AvisCompteRenduTravauxes)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduTravaux_client_id_fkey");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.AvisCompteRenduTravauxes)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduTravaux_compte_rendu_id_fkey");

            entity.HasOne(d => d.StatutTravaux).WithMany(p => p.AvisCompteRenduTravauxes)
                .HasForeignKey(d => d.StatutTravauxId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduTravaux_statut_travaux_id_fkey");
        });

        modelBuilder.Entity<AvisCompteRenduTravauxAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisCompteRenduTravauxAction_pkey");

            entity.ToTable("AvisCompteRenduTravauxAction", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisCompteRenduId)
                .HasMaxLength(255)
                .HasColumnName("avis_compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeActionId).HasColumnName("type_action_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AvisCompteRendu).WithMany(p => p.AvisCompteRenduTravauxActions)
                .HasForeignKey(d => d.AvisCompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduTravauxAction_avis_compte_rendu_id_fkey");

            entity.HasOne(d => d.TypeAction).WithMany(p => p.AvisCompteRenduTravauxActions)
                .HasForeignKey(d => d.TypeActionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisCompteRenduTravauxAction_type_action_id_fkey");
        });

        modelBuilder.Entity<AvisDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDemande_pkey");

            entity.ToTable("AvisDemande", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Annotations)
                .HasMaxLength(255)
                .HasColumnName("annotations");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.Commentaire)
                .HasMaxLength(255)
                .HasColumnName("commentaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DatePrevuRdv).HasColumnName("date_prevu_rdv");
            entity.Property(e => e.DatePrevuRetraitDevis).HasColumnName("date_prevu_retrait_devis");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.LibelleAvis)
                .HasMaxLength(255)
                .HasColumnName("libelle_avis");
            entity.Property(e => e.MatriculeActeur)
                .HasMaxLength(255)
                .HasColumnName("matricule_acteur");
            entity.Property(e => e.NomActeur)
                .HasMaxLength(255)
                .HasColumnName("nom_acteur");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.TypeRecoursId).HasColumnName("type_recours_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.ZonePourvuEau)
                .HasDefaultValue(true)
                .HasColumnName("zone_pourvu_eau");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisDemandes)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDemande_avis_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.AvisDemandes)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDemande_demande_id_fkey");

            entity.HasOne(d => d.TypeRecours).WithMany(p => p.AvisDemandes)
                .HasForeignKey(d => d.TypeRecoursId)
                .HasConstraintName("AvisDemande_type_recours_id_fkey");
        });

        modelBuilder.Entity<AvisDemandeMotifRejet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDemandeMotifRejet_pkey");

            entity.ToTable("AvisDemandeMotifRejet", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisDemandeId)
                .HasMaxLength(255)
                .HasColumnName("avis_demande_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AvisDemande).WithMany(p => p.AvisDemandeMotifRejets)
                .HasForeignKey(d => d.AvisDemandeId)
                .HasConstraintName("AvisDemandeMotifRejet_avis_demande_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.AvisDemandeMotifRejets)
                .HasForeignKey(d => d.MotifRejetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDemandeMotifRejet_motif_rejet_id_fkey");
        });

        modelBuilder.Entity<AvisDemandeRemarque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDemandeRemarque_pkey");

            entity.ToTable("AvisDemandeRemarque", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisDemandeId)
                .HasMaxLength(255)
                .HasColumnName("avis_demande_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.RemarqueId).HasColumnName("remarque_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AvisDemande).WithMany(p => p.AvisDemandeRemarques)
                .HasForeignKey(d => d.AvisDemandeId)
                .HasConstraintName("AvisDemandeRemarque_avis_demande_id_fkey");

            entity.HasOne(d => d.Remarque).WithMany(p => p.AvisDemandeRemarques)
                .HasForeignKey(d => d.RemarqueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDemandeRemarque_remarque_id_fkey");
        });

        modelBuilder.Entity<AvisDevisMetre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDevisMetre_pkey");

            entity.ToTable("AvisDevisMetre", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Annotations)
                .HasMaxLength(255)
                .HasColumnName("annotations");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.AvisLibelle)
                .HasMaxLength(255)
                .HasColumnName("avis_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DataObject)
                .HasColumnType("json")
                .HasColumnName("data_object");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DevisId)
                .HasMaxLength(255)
                .HasColumnName("devis_id");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeName)
                .HasMaxLength(255)
                .HasColumnName("etape_name");
            entity.Property(e => e.MatriculeActeur)
                .HasMaxLength(255)
                .HasColumnName("matricule_acteur");
            entity.Property(e => e.NomActeur)
                .HasMaxLength(255)
                .HasColumnName("nom_acteur");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TacheId)
                .HasMaxLength(255)
                .HasColumnName("tache_id");
            entity.Property(e => e.TypeAvisId).HasColumnName("type_avis_id");
            entity.Property(e => e.TypeAvisLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_avis_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Avis).WithMany(p => p.AvisDevisMetres)
                .HasForeignKey(d => d.AvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetre_avis_id_fkey");

            entity.HasOne(d => d.Devis).WithMany(p => p.AvisDevisMetres)
                .HasForeignKey(d => d.DevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetre_devis_id_fkey");

            entity.HasOne(d => d.TypeAvis).WithMany(p => p.AvisDevisMetres)
                .HasForeignKey(d => d.TypeAvisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetre_type_avis_id_fkey");
        });

        modelBuilder.Entity<AvisDevisMetreDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDevisMetreDocument_pkey");

            entity.ToTable("AvisDevisMetreDocument", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisDevisId)
                .HasMaxLength(255)
                .HasColumnName("avis_devis_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.IsNotifier).HasColumnName("is_notifier");
            entity.Property(e => e.NotificationSend)
                .HasDefaultValue(0)
                .HasComment("0 = notif pas encore envoyé; 1 = notif envoyé; -1 = echec d'envoie notif")
                .HasColumnName("notification_send");
            entity.Property(e => e.NumeroDocument)
                .HasMaxLength(255)
                .HasColumnName("numero_document");
            entity.Property(e => e.RefFichier)
                .HasMaxLength(255)
                .HasColumnName("ref_fichier");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDocumentId).HasColumnName("type_document_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlFichier)
                .HasMaxLength(255)
                .HasColumnName("url_fichier");

            entity.HasOne(d => d.AvisDevis).WithMany(p => p.AvisDevisMetreDocuments)
                .HasForeignKey(d => d.AvisDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetreDocument_avis_devis_id_fkey");

            entity.HasOne(d => d.TypeDocument).WithMany(p => p.AvisDevisMetreDocuments)
                .HasForeignKey(d => d.TypeDocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetreDocument_type_document_id_fkey");
        });

        modelBuilder.Entity<AvisDevisMetreMotifRejet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDevisMetreMotifRejet_pkey");

            entity.ToTable("AvisDevisMetreMotifRejet", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisDevisId)
                .HasMaxLength(255)
                .HasColumnName("avis_devis_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AvisDevis).WithMany(p => p.AvisDevisMetreMotifRejets)
                .HasForeignKey(d => d.AvisDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetreMotifRejet_avis_devis_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.AvisDevisMetreMotifRejets)
                .HasForeignKey(d => d.MotifRejetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetreMotifRejet_motif_rejet_id_fkey");
        });

        modelBuilder.Entity<AvisDevisMetreRemarque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvisDevisMetreRemarque_pkey");

            entity.ToTable("AvisDevisMetreRemarque", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisDevisId)
                .HasMaxLength(255)
                .HasColumnName("avis_devis_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.RemarqueId).HasColumnName("remarque_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.AvisDevis).WithMany(p => p.AvisDevisMetreRemarques)
                .HasForeignKey(d => d.AvisDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetreRemarque_avis_devis_id_fkey");

            entity.HasOne(d => d.Remarque).WithMany(p => p.AvisDevisMetreRemarques)
                .HasForeignKey(d => d.RemarqueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvisDevisMetreRemarque_remarque_id_fkey");
        });

        modelBuilder.Entity<AvoirClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvoirClient_pkey");

            entity.ToTable("AvoirClient", "sc_sge");

            entity.HasIndex(e => e.NumeroAvoir, "avoir_client_numero_avoir");

            entity.HasIndex(e => e.NumeroClient, "avoir_client_numero_client");

            entity.HasIndex(e => e.NumeroDemande, "avoir_client_numero_demande");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminId)
                .HasMaxLength(255)
                .HasColumnName("admin_id");
            entity.Property(e => e.AgencePaiementCode)
                .HasMaxLength(255)
                .HasColumnName("agence_paiement_code");
            entity.Property(e => e.AgencePaiementId)
                .HasMaxLength(255)
                .HasColumnName("agence_paiement_id");
            entity.Property(e => e.AgencePaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("agence_paiement_libelle");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CodeCentre)
                .HasMaxLength(255)
                .HasColumnName("code_centre");
            entity.Property(e => e.CodeOperationCode)
                .HasMaxLength(255)
                .HasColumnName("code_operation_code");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CodeOperationLibelle)
                .HasMaxLength(255)
                .HasColumnName("code_operation_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateAvoir).HasColumnName("date_avoir");
            entity.Property(e => e.DateDemande).HasColumnName("date_demande");
            entity.Property(e => e.DateExigibilite).HasColumnName("date_exigibilite");
            entity.Property(e => e.DateFlag).HasColumnName("date_flag");
            entity.Property(e => e.DateValeur).HasColumnName("date_valeur");
            entity.Property(e => e.DateVisa).HasColumnName("date_visa");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsSolde)
                .HasDefaultValue(false)
                .HasColumnName("is_solde");
            entity.Property(e => e.LibelleTopCode)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_code");
            entity.Property(e => e.LibelleTopId).HasColumnName("libelle_top_id");
            entity.Property(e => e.LibelleTopLibelle)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_libelle");
            entity.Property(e => e.MatriculeAdmin)
                .HasMaxLength(255)
                .HasColumnName("matricule_admin");
            entity.Property(e => e.MoisComptable)
                .HasMaxLength(255)
                .HasColumnName("mois_comptable");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.MotifAnnulationId).HasColumnName("motif_annulation_id");
            entity.Property(e => e.MotifAnnulationLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_annulation_libelle");
            entity.Property(e => e.MoyenPaiementId).HasColumnName("moyen_paiement_id");
            entity.Property(e => e.MoyenPaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("moyen_paiement_libelle");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasColumnName("nom_centre");
            entity.Property(e => e.NonEncaissable)
                .HasDefaultValue(false)
                .HasColumnName("non_encaissable");
            entity.Property(e => e.NumeroAvoir)
                .HasMaxLength(255)
                .HasColumnName("numero_avoir");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.Origine)
                .HasMaxLength(255)
                .HasColumnName("origine");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutAvoirId).HasColumnName("statut_avoir_id");
            entity.Property(e => e.StatutAvoirLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_avoir_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Client).WithMany(p => p.AvoirClients)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("AvoirClient_client_id_fkey");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.AvoirClients)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AvoirClient_code_operation_id_fkey");

            entity.HasOne(d => d.LibelleTop).WithMany(p => p.AvoirClients)
                .HasForeignKey(d => d.LibelleTopId)
                .HasConstraintName("AvoirClient_libelle_top_id_fkey");

            entity.HasOne(d => d.MotifAnnulation).WithMany(p => p.AvoirClients)
                .HasForeignKey(d => d.MotifAnnulationId)
                .HasConstraintName("AvoirClient_motif_annulation_id_fkey");

            entity.HasOne(d => d.MoyenPaiement).WithMany(p => p.AvoirClients)
                .HasForeignKey(d => d.MoyenPaiementId)
                .HasConstraintName("AvoirClient_moyen_paiement_id_fkey");

            entity.HasOne(d => d.StatutAvoir).WithMany(p => p.AvoirClients)
                .HasForeignKey(d => d.StatutAvoirId)
                .HasConstraintName("AvoirClient_statut_avoir_id_fkey");
        });

        modelBuilder.Entity<BilletPiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BilletPiece_pkey");

            entity.ToTable("BilletPiece", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Devise)
                .HasMaxLength(255)
                .HasColumnName("devise");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeBilletPieceId).HasColumnName("type_billet_piece_id");
            entity.Property(e => e.TypeDeviseId).HasColumnName("type_devise_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Valeur)
                .HasMaxLength(255)
                .HasColumnName("valeur");

            entity.HasOne(d => d.TypeBilletPiece).WithMany(p => p.BilletPieces)
                .HasForeignKey(d => d.TypeBilletPieceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BilletPiece_type_billet_piece_id_fkey");

            entity.HasOne(d => d.TypeDevise).WithMany(p => p.BilletPieces)
                .HasForeignKey(d => d.TypeDeviseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BilletPiece_type_devise_id_fkey");
        });

        modelBuilder.Entity<Billetage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Billetage_pkey");

            entity.ToTable("Billetage", "sc_sge");

            entity.HasIndex(e => e.NumeroBilletage, "Billetage_numero_billetage_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Caissier)
                .HasMaxLength(255)
                .HasColumnName("caissier");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateBilletage).HasColumnName("date_billetage");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Ecart)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("ecart");
            entity.Property(e => e.MontantBilletage).HasColumnName("montant_billetage");
            entity.Property(e => e.MontantCheque).HasColumnName("montant_cheque");
            entity.Property(e => e.MontantEspece).HasColumnName("montant_espece");
            entity.Property(e => e.NombreBillet).HasColumnName("nombre_billet");
            entity.Property(e => e.NombreCheque).HasColumnName("nombre_cheque");
            entity.Property(e => e.NombrePiece).HasColumnName("nombre_piece");
            entity.Property(e => e.NumeroBilletage)
                .HasMaxLength(255)
                .HasColumnName("numero_billetage");
            entity.Property(e => e.NumeroCaisse)
                .HasMaxLength(255)
                .HasColumnName("numero_caisse");
            entity.Property(e => e.SoldeCaise).HasColumnName("solde_caise");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeBilletageId).HasColumnName("type_billetage_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeBilletage).WithMany(p => p.Billetages)
                .HasForeignKey(d => d.TypeBilletageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Billetage_type_billetage_id_fkey");
        });

        modelBuilder.Entity<BilletageDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BilletageDetail_pkey");

            entity.ToTable("BilletageDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BilletPieceId).HasColumnName("billet_piece_id");
            entity.Property(e => e.BilletageId).HasColumnName("billetage_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.BilletPiece).WithMany(p => p.BilletageDetails)
                .HasForeignKey(d => d.BilletPieceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BilletageDetail_billet_piece_id_fkey");

            entity.HasOne(d => d.Billetage).WithMany(p => p.BilletageDetails)
                .HasForeignKey(d => d.BilletageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BilletageDetail_billetage_id_fkey");
        });

        modelBuilder.Entity<BranchementEau>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BranchementEau_pkey");

            entity.ToTable("BranchementEau", "sc_sge");

            entity.HasIndex(e => e.NumeroBranchement, "BranchementEau_numero_branchement_key").IsUnique();

            entity.HasIndex(e => e.NumeroBranchement, "BranchementEau_numero_branchement_key1").IsUnique();

            entity.HasIndex(e => e.NumeroBranchement, "BranchementEau_numero_branchement_key2").IsUnique();

            entity.HasIndex(e => e.NumeroBranchement, "BranchementEau_numero_branchement_key3").IsUnique();

            entity.HasIndex(e => e.NumeroBranchement, "BranchementEau_numero_branchement_key4").IsUnique();

            entity.HasIndex(e => e.NumeroBranchement, "BranchementEau_numero_branchement_key5").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdresseComplementaire)
                .HasMaxLength(255)
                .HasColumnName("adresse_complementaire");
            entity.Property(e => e.Carre)
                .HasMaxLength(255)
                .HasColumnName("carre");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CommuneId)
                .HasMaxLength(255)
                .HasColumnName("commune_id");
            entity.Property(e => e.CommuneLibelle)
                .HasMaxLength(255)
                .HasColumnName("commune_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.DiametreConduiteId).HasColumnName("diametre_conduite_id");
            entity.Property(e => e.DiametreTuyauId).HasColumnName("diametre_tuyau_id");
            entity.Property(e => e.Etage)
                .HasMaxLength(255)
                .HasColumnName("etage");
            entity.Property(e => e.Ilot)
                .HasMaxLength(255)
                .HasColumnName("ilot");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.LineaireFacade).HasColumnName("lineaire_facade");
            entity.Property(e => e.LineaireTuyau).HasColumnName("lineaire_tuyau");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Lot)
                .HasMaxLength(255)
                .HasColumnName("lot");
            entity.Property(e => e.NatureConduiteId).HasColumnName("nature_conduite_id");
            entity.Property(e => e.NatureTuyauId).HasColumnName("nature_tuyau_id");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasColumnName("nom_centre");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NomProprietaire)
                .HasMaxLength(255)
                .HasColumnName("nom_proprietaire");
            entity.Property(e => e.NombreHabitant).HasColumnName("nombre_habitant");
            entity.Property(e => e.NombrePointDeau).HasColumnName("nombre_point_deau");
            entity.Property(e => e.NumeroAbonnementVoisin).HasColumnName("numero_abonnement_voisin");
            entity.Property(e => e.NumeroBranchement)
                .HasMaxLength(255)
                .HasColumnName("numero_branchement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroSchema)
                .HasMaxLength(255)
                .HasColumnName("numero_schema");
            entity.Property(e => e.Ordre).HasColumnName("ordre");
            entity.Property(e => e.Porte)
                .HasMaxLength(255)
                .HasColumnName("porte");
            entity.Property(e => e.QuartierId).HasColumnName("quartier_id");
            entity.Property(e => e.QuartierLibelle)
                .HasMaxLength(255)
                .HasColumnName("quartier_libelle");
            entity.Property(e => e.ReferenceExtension)
                .HasMaxLength(255)
                .HasColumnName("reference_extension");
            entity.Property(e => e.Rue)
                .HasMaxLength(255)
                .HasColumnName("rue");
            entity.Property(e => e.SecteurId).HasColumnName("secteur_id");
            entity.Property(e => e.SecteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("secteur_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutBranchementId).HasColumnName("statut_branchement_id");
            entity.Property(e => e.StatutBranchementLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_branchement_libelle");
            entity.Property(e => e.TourneeCode)
                .HasMaxLength(255)
                .HasColumnName("tournee_code");
            entity.Property(e => e.TourneeId).HasColumnName("tournee_id");
            entity.Property(e => e.TourneeLibelle)
                .HasMaxLength(255)
                .HasColumnName("tournee_libelle");
            entity.Property(e => e.TypeBranchementId).HasColumnName("type_branchement_id");
            entity.Property(e => e.TypeBranchementLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_branchement_libelle");
            entity.Property(e => e.TypeTravauxId).HasColumnName("type_travaux_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlSchema)
                .HasMaxLength(255)
                .HasColumnName("url_schema");
            entity.Property(e => e.VilleId)
                .HasMaxLength(255)
                .HasColumnName("ville_id");
            entity.Property(e => e.VilleLibelle)
                .HasMaxLength(255)
                .HasColumnName("ville_libelle");

            entity.HasOne(d => d.Client).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("BranchementEau_client_id_fkey");

            entity.HasOne(d => d.Commune).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.CommuneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_commune_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_demande_id_fkey");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.DiametreCompteurId)
                .HasConstraintName("BranchementEau_diametre_compteur_id_fkey");

            entity.HasOne(d => d.DiametreConduite).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.DiametreConduiteId)
                .HasConstraintName("BranchementEau_diametre_conduite_id_fkey");

            entity.HasOne(d => d.DiametreTuyau).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.DiametreTuyauId)
                .HasConstraintName("BranchementEau_diametre_tuyau_id_fkey");

            entity.HasOne(d => d.NatureConduite).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.NatureConduiteId)
                .HasConstraintName("BranchementEau_nature_conduite_id_fkey");

            entity.HasOne(d => d.NatureTuyau).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.NatureTuyauId)
                .HasConstraintName("BranchementEau_nature_tuyau_id_fkey");

            entity.HasOne(d => d.Quartier).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.QuartierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_quartier_id_fkey");

            entity.HasOne(d => d.Secteur).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.SecteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_secteur_id_fkey");

            entity.HasOne(d => d.StatutBranchement).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.StatutBranchementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_statut_branchement_id_fkey");

            entity.HasOne(d => d.Tournee).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.TourneeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_tournee_id_fkey");

            entity.HasOne(d => d.TypeBranchement).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.TypeBranchementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_type_branchement_id_fkey");

            entity.HasOne(d => d.TypeTravaux).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.TypeTravauxId)
                .HasConstraintName("BranchementEau_type_travaux_id_fkey");

            entity.HasOne(d => d.Ville).WithMany(p => p.BranchementEaus)
                .HasForeignKey(d => d.VilleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchementEau_ville_id_fkey");
        });

        modelBuilder.Entity<BranchementEauPdl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BranchementEauPdl_pkey");

            entity.ToTable("BranchementEauPdl", "sc_sge");

            entity.HasIndex(e => new { e.BranchementEauId, e.PdlId }, "BranchementEauPdl_branchement_eau_id_pdl_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BranchementEauId)
                .HasMaxLength(255)
                .HasColumnName("branchement_eau_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.PdlId).HasColumnName("pdl_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.BranchementEau).WithMany(p => p.BranchementEauPdls)
                .HasForeignKey(d => d.BranchementEauId)
                .HasConstraintName("BranchementEauPdl_branchement_eau_id_fkey");

            entity.HasOne(d => d.Pdl).WithMany(p => p.BranchementEauPdls)
                .HasForeignKey(d => d.PdlId)
                .HasConstraintName("BranchementEauPdl_pdl_id_fkey");
        });

        modelBuilder.Entity<Ca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cas_pkey");

            entity.ToTable("Cas", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CodeParent)
                .HasMaxLength(255)
                .HasColumnName("code_parent");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.IsLieCompteur).HasColumnName("is_lie_compteur");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeCasId).HasColumnName("type_cas_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeCas).WithMany(p => p.Cas)
                .HasForeignKey(d => d.TypeCasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Cas_type_cas_id_fkey");
        });

        modelBuilder.Entity<Caisse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Caisse_pkey");

            entity.ToTable("Caisse", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Acquit).HasColumnName("acquit");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Bordereau)
                .HasMaxLength(255)
                .HasColumnName("bordereau");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.Compte)
                .HasMaxLength(255)
                .HasColumnName("compte");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Fond).HasColumnName("fond");
            entity.Property(e => e.IsAttribue).HasColumnName("is_attribue");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Numero)
                .HasMaxLength(255)
                .HasColumnName("numero");
            entity.Property(e => e.SoldeEncours).HasColumnName("solde_encours");
            entity.Property(e => e.SoldeVeille).HasColumnName("solde_veille");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeCaisseId).HasColumnName("type_caisse_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CaisseUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CaisseUser_pkey");

            entity.ToTable("CaisseUser", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CaisseId)
                .HasMaxLength(255)
                .HasColumnName("caisse_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Caisse).WithMany(p => p.CaisseUsers)
                .HasForeignKey(d => d.CaisseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CaisseUser_caisse_id_fkey");
        });

        modelBuilder.Entity<CalibreCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CalibreCompteur_pkey");

            entity.ToTable("CalibreCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.PhaseCompteurId).HasColumnName("phase_compteur_id");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.PhaseCompteur).WithMany(p => p.CalibreCompteurs)
                .HasForeignKey(d => d.PhaseCompteurId)
                .HasConstraintName("CalibreCompteur_phase_compteur_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.CalibreCompteurs)
                .HasForeignKey(d => d.ProduitId)
                .HasConstraintName("CalibreCompteur_produit_id_fkey");
        });

        modelBuilder.Entity<Campagne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Campagne_pkey");

            entity.ToTable("Campagne", "sc_sge");

            entity.HasIndex(e => e.Code, "Campagne_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "Campagne_code_key9").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminId)
                .HasMaxLength(255)
                .HasColumnName("admin_id");
            entity.Property(e => e.AdminMatricule)
                .HasMaxLength(255)
                .HasColumnName("admin_matricule");
            entity.Property(e => e.AdminNom)
                .HasMaxLength(255)
                .HasColumnName("admin_nom");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CentreNom)
                .HasMaxLength(255)
                .HasColumnName("centre_nom");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutEffective).HasColumnName("date_debut_effective");
            entity.Property(e => e.DateDebutPrevu).HasColumnName("date_debut_prevu");
            entity.Property(e => e.DateFinEffective).HasColumnName("date_fin_effective");
            entity.Property(e => e.DateFinPrevu).HasColumnName("date_fin_prevu");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.NombreJourReleve).HasColumnName("nombre_jour_releve");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.PeriodeId).HasColumnName("periode_id");
            entity.Property(e => e.PeriodeLibelle)
                .HasMaxLength(255)
                .HasColumnName("periode_libelle");
            entity.Property(e => e.PrevisionQuantiteFacture).HasColumnName("prevision_quantite_facture");
            entity.Property(e => e.PrevisionVolumeFacture).HasColumnName("prevision_volume_facture");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutCampagneId).HasColumnName("statut_campagne_id");
            entity.Property(e => e.StatutCampagneLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_campagne_libelle");
            entity.Property(e => e.TypeCampagneId).HasColumnName("type_campagne_id");
            entity.Property(e => e.TypeCampagneLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_campagne_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Periode).WithMany(p => p.Campagnes)
                .HasForeignKey(d => d.PeriodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Campagne_periode_id_fkey");

            entity.HasOne(d => d.StatutCampagne).WithMany(p => p.Campagnes)
                .HasForeignKey(d => d.StatutCampagneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Campagne_statut_campagne_id_fkey");

            entity.HasOne(d => d.TypeCampagne).WithMany(p => p.Campagnes)
                .HasForeignKey(d => d.TypeCampagneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Campagne_type_campagne_id_fkey");
        });

        modelBuilder.Entity<CampagneGroupeFacturation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CampagneGroupeFacturation_pkey");

            entity.ToTable("CampagneGroupeFacturation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CampagneId)
                .HasMaxLength(255)
                .HasColumnName("campagne_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.GroupeFacturationId).HasColumnName("groupe_facturation_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Campagne).WithMany(p => p.CampagneGroupeFacturations)
                .HasForeignKey(d => d.CampagneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CampagneGroupeFacturation_campagne_id_fkey");

            entity.HasOne(d => d.GroupeFacturation).WithMany(p => p.CampagneGroupeFacturations)
                .HasForeignKey(d => d.GroupeFacturationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CampagneGroupeFacturation_groupe_facturation_id_fkey");
        });

        modelBuilder.Entity<CategorieClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CategorieClient_pkey");

            entity.ToTable("CategorieClient", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CategorieTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CategorieTravaux_pkey");

            entity.ToTable("CategorieTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CautionAbonnement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CautionAbonnement_pkey");

            entity.ToTable("CautionAbonnement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbonnementId)
                .HasMaxLength(255)
                .HasColumnName("abonnement_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Abonnement).WithMany(p => p.CautionAbonnements)
                .HasForeignKey(d => d.AbonnementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CautionAbonnement_abonnement_id_fkey");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.CautionAbonnements)
                .HasForeignKey(d => d.DiametreCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CautionAbonnement_diametre_compteur_id_fkey");
        });

        modelBuilder.Entity<ChaineRelance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ChaineRelance_pkey");

            entity.ToTable("ChaineRelance", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ChaineRelanceEtape>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ChaineRelanceEtape_pkey");

            entity.ToTable("ChaineRelanceEtape", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ChaineRelanceId).HasColumnName("chaine_relance_id");
            entity.Property(e => e.ChaineRelanceLibelle)
                .HasMaxLength(255)
                .HasColumnName("chaine_relance_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Delai)
                .HasComment("en jour")
                .HasColumnName("delai");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Etape)
                .HasMaxLength(255)
                .HasColumnName("etape");
            entity.Property(e => e.Ordre).HasColumnName("ordre");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.TypeDemandeLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_demande_libelle");
            entity.Property(e => e.TypeDocumentId).HasColumnName("type_document_id");
            entity.Property(e => e.TypeDocumentLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_document_libelle");
            entity.Property(e => e.TypeFraisId).HasColumnName("type_frais_id");
            entity.Property(e => e.TypeFraisLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_frais_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.ChaineRelance).WithMany(p => p.ChaineRelanceEtapes)
                .HasForeignKey(d => d.ChaineRelanceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ChaineRelanceEtape_chaine_relance_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.ChaineRelanceEtapes)
                .HasForeignKey(d => d.TypeDemandeId)
                .HasConstraintName("ChaineRelanceEtape_type_demande_id_fkey");

            entity.HasOne(d => d.TypeDocument).WithMany(p => p.ChaineRelanceEtapes)
                .HasForeignKey(d => d.TypeDocumentId)
                .HasConstraintName("ChaineRelanceEtape_type_document_id_fkey");

            entity.HasOne(d => d.TypeFrais).WithMany(p => p.ChaineRelanceEtapes)
                .HasForeignKey(d => d.TypeFraisId)
                .HasConstraintName("ChaineRelanceEtape_type_frais_id_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("City_pkey");

            entity.ToTable("City");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CountryId)
                .HasMaxLength(255)
                .HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("City_country_id_fkey");
        });

        modelBuilder.Entity<City1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("City_pkey");

            entity.ToTable("City", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CountryId)
                .HasMaxLength(255)
                .HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Country).WithMany(p => p.City1s)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("City_country_id_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Client_pkey");

            entity.ToTable("Client", "sc_sge");

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key1").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key10").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key11").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key12").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key13").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key14").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key15").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key16").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key17").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key18").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key19").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key2").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key20").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key21").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key22").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key23").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key24").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key25").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key26").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key27").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key28").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key29").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key3").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key30").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key31").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key32").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key33").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key4").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key5").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key6").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key7").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key8").IsUnique();

            entity.HasIndex(e => e.NumeroClient, "Client_numero_client_key9").IsUnique();

            entity.HasIndex(e => e.CategorieClientId, "client_categorie_client_id_idx");

            entity.HasIndex(e => e.CategorieClientLibelle, "client_categorie_client_libelle_idx");

            entity.HasIndex(e => e.ClientExonere, "client_client_exonere_idx");

            entity.HasIndex(e => e.DateNaissance, "client_date_naissance_idx");

            entity.HasIndex(e => e.Email, "client_email_idx");

            entity.HasIndex(e => e.Nom, "client_nom_idx");

            entity.HasIndex(e => e.NumeroAgrement, "client_numero_agrement_idx");

            entity.HasIndex(e => e.NumeroClient, "client_numero_client_idx");

            entity.HasIndex(e => e.NumeroFiscal, "client_numero_fiscal_idx");

            entity.HasIndex(e => e.NumeroIdentificationUnique, "client_numero_identification_unique_idx");

            entity.HasIndex(e => e.NumeroRccm, "client_numero_rccm_idx");

            entity.HasIndex(e => e.Prenoms, "client_prenoms_idx");

            entity.HasIndex(e => e.Telephone, "client_telephone_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BoitePostal)
                .HasMaxLength(255)
                .HasColumnName("boite_postal");
            entity.Property(e => e.CategorieClientId).HasColumnName("categorie_client_id");
            entity.Property(e => e.CategorieClientLibelle)
                .HasMaxLength(255)
                .HasColumnName("categorie_client_libelle");
            entity.Property(e => e.ClientExonere)
                .HasDefaultValue(false)
                .HasColumnName("client_exonere");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateNaissance)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("date_naissance");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DenominationId).HasColumnName("denomination_id");
            entity.Property(e => e.DenominationLibelle)
                .HasMaxLength(255)
                .HasColumnName("denomination_libelle");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fax)
                .HasMaxLength(255)
                .HasColumnName("fax");
            entity.Property(e => e.LienFacebook)
                .HasMaxLength(255)
                .HasColumnName("lien_facebook");
            entity.Property(e => e.LienLinkedin)
                .HasMaxLength(255)
                .HasColumnName("lien_linkedin");
            entity.Property(e => e.LieuNaissance)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("lieu_naissance");
            entity.Property(e => e.NationnaliteId).HasColumnName("nationnalite_id");
            entity.Property(e => e.NationnaliteLibelle)
                .HasMaxLength(255)
                .HasColumnName("nationnalite_libelle");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("nom");
            entity.Property(e => e.NomSecondaire)
                .HasMaxLength(255)
                .HasColumnName("nom_secondaire");
            entity.Property(e => e.NumeroAbonne)
                .HasMaxLength(255)
                .HasColumnName("numero_abonne");
            entity.Property(e => e.NumeroAgrement)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_agrement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroFiscal)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_fiscal");
            entity.Property(e => e.NumeroIdentificationUnique)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_identification_unique");
            entity.Property(e => e.NumeroRccm)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_rccm");
            entity.Property(e => e.Prenoms)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("prenoms");
            entity.Property(e => e.PrenomsSecondaire)
                .HasMaxLength(255)
                .HasColumnName("prenoms_secondaire");
            entity.Property(e => e.RaisonSociale)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("raison_sociale");
            entity.Property(e => e.Sigle)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("sigle");
            entity.Property(e => e.SiteWeb)
                .HasMaxLength(255)
                .HasColumnName("site_web");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Surnom)
                .HasMaxLength(255)
                .HasColumnName("surnom");
            entity.Property(e => e.Telephone)
                .HasMaxLength(255)
                .HasColumnName("telephone");
            entity.Property(e => e.TelephoneFixe)
                .HasMaxLength(255)
                .HasColumnName("telephone_fixe");
            entity.Property(e => e.TypeClientId).HasColumnName("type_client_id");
            entity.Property(e => e.TypeClientLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_client_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Whatsapp)
                .HasMaxLength(255)
                .HasColumnName("whatsapp");

            entity.HasOne(d => d.CategorieClient).WithMany(p => p.Clients)
                .HasForeignKey(d => d.CategorieClientId)
                .HasConstraintName("Client_categorie_client_id_fkey");

            entity.HasOne(d => d.Denomination).WithMany(p => p.Clients)
                .HasForeignKey(d => d.DenominationId)
                .HasConstraintName("Client_denomination_id_fkey");

            entity.HasOne(d => d.Nationnalite).WithMany(p => p.Clients)
                .HasForeignKey(d => d.NationnaliteId)
                .HasConstraintName("Client_nationnalite_id_fkey");

            entity.HasOne(d => d.TypeClient).WithMany(p => p.Clients)
                .HasForeignKey(d => d.TypeClientId)
                .HasConstraintName("Client_type_client_id_fkey");
        });

        modelBuilder.Entity<ClientPiecesFourny>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClientPiecesFournies_pkey");

            entity.ToTable("ClientPiecesFournies", "sc_sge");

            entity.HasIndex(e => new { e.TypePieceId, e.ClientId }, "ClientPiecesFournies_type_piece_id_client_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEtablissement).HasColumnName("date_etablissement");
            entity.Property(e => e.DateExpiration).HasColumnName("date_expiration");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EntiteEtablissement)
                .HasMaxLength(255)
                .HasColumnName("entite_etablissement");
            entity.Property(e => e.NumeroPiece)
                .HasMaxLength(255)
                .HasColumnName("numero_piece");
            entity.Property(e => e.RefFichier)
                .HasMaxLength(255)
                .HasColumnName("ref_fichier");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypePieceId).HasColumnName("type_piece_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlFichier)
                .HasMaxLength(255)
                .HasColumnName("url_fichier");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientPiecesFournies)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("ClientPiecesFournies_client_id_fkey");

            entity.HasOne(d => d.TypePiece).WithMany(p => p.ClientPiecesFournies)
                .HasForeignKey(d => d.TypePieceId)
                .HasConstraintName("ClientPiecesFournies_type_piece_id_fkey");
        });

        modelBuilder.Entity<ClientRolesPersonnesPhysique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClientRolesPersonnesPhysique_pkey");

            entity.ToTable("ClientRolesPersonnesPhysique", "sc_sge");

            entity.HasIndex(e => new { e.PersonnePhysiqueLieeId, e.ClientId }, "ClientRolesPersonnesPhysique_personne_physique_liee_id_clie_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.PersonnePhysiqueLieeId)
                .HasMaxLength(255)
                .HasColumnName("personne_physique_liee_id");
            entity.Property(e => e.RolePersonnePhysiqueId).HasColumnName("role_personne_physique_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientRolesPersonnesPhysiques)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("ClientRolesPersonnesPhysique_client_id_fkey");

            entity.HasOne(d => d.PersonnePhysiqueLiee).WithMany(p => p.ClientRolesPersonnesPhysiques)
                .HasForeignKey(d => d.PersonnePhysiqueLieeId)
                .HasConstraintName("ClientRolesPersonnesPhysique_personne_physique_liee_id_fkey");

            entity.HasOne(d => d.RolePersonnePhysique).WithMany(p => p.ClientRolesPersonnesPhysiques)
                .HasForeignKey(d => d.RolePersonnePhysiqueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientRolesPersonnesPhysique_role_personne_physique_id_fkey");
        });

        modelBuilder.Entity<CodeObi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CodeObis_pkey");

            entity.ToTable("CodeObis", "sc_sge");

            entity.HasIndex(e => e.Category, "code_obis_category");

            entity.HasIndex(e => e.Code, "code_obis_code");

            entity.HasIndex(e => e.Status, "code_obis_status");

            entity.HasIndex(e => e.TagPath, "code_obis_tag_path");

            entity.HasIndex(e => e.Value, "code_obis_valeur");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TagPath)
                .HasMaxLength(255)
                .HasColumnName("tag_path");
            entity.Property(e => e.TagType)
                .HasMaxLength(255)
                .HasColumnName("tag_type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");
        });

        modelBuilder.Entity<CodeOperation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CodeOperation_pkey");

            entity.ToTable("CodeOperation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Isod)
                .HasMaxLength(255)
                .HasColumnName("ISOD");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.LibelleCourt)
                .HasMaxLength(255)
                .HasColumnName("libelle_court");
            entity.Property(e => e.Sens)
                .HasMaxLength(255)
                .HasColumnName("sens");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CodeOperationCompte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CodeOperationCompte_pkey");

            entity.ToTable("CodeOperationCompte", "sc_sge");

            entity.HasIndex(e => e.CodeOperationCode, "code_operation_compte_code_operation_code");

            entity.HasIndex(e => e.CodeOperationId, "code_operation_compte_code_operation_id");

            entity.HasIndex(e => e.CompteCode, "code_operation_compte_compte_code");

            entity.HasIndex(e => e.CompteId, "code_operation_compte_compte_id");

            entity.HasIndex(e => e.CompteNumero, "code_operation_compte_compte_numero");

            entity.HasIndex(e => e.Status, "code_operation_compte_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeOperationCode)
                .HasMaxLength(255)
                .HasColumnName("code_operation_code");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CodeOperationLibelle)
                .HasMaxLength(255)
                .HasColumnName("code_operation_libelle");
            entity.Property(e => e.CompteCode)
                .HasMaxLength(255)
                .HasColumnName("compte_code");
            entity.Property(e => e.CompteId).HasColumnName("compte_id");
            entity.Property(e => e.CompteLibelle)
                .HasMaxLength(255)
                .HasColumnName("compte_libelle");
            entity.Property(e => e.CompteNumero)
                .HasMaxLength(255)
                .HasColumnName("compte_numero");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.CodeOperationComptes)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CodeOperationCompte_code_operation_id_fkey");

            entity.HasOne(d => d.Compte).WithMany(p => p.CodeOperationComptes)
                .HasForeignKey(d => d.CompteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CodeOperationCompte_compte_id_fkey");
        });

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.Idcommande).HasName("commande_pkey");

            entity.ToTable("commande", "sc_sge");

            entity.Property(e => e.Idcommande).HasColumnName("idcommande");
            entity.Property(e => e.Dateexec)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateexec");
            entity.Property(e => e.Datefin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datefin");
            entity.Property(e => e.Idtype).HasColumnName("idtype");
            entity.Property(e => e.Libellecommande)
                .HasMaxLength(255)
                .HasColumnName("libellecommande");
            entity.Property(e => e.Statut).HasColumnName("statut");

            entity.HasOne(d => d.IdtypeNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.Idtype)
                .HasConstraintName("commande_idtype_fkey");
        });

        modelBuilder.Entity<Commandecompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("commandecompteurs_pkey");

            entity.ToTable("commandecompteurs", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Compteurid)
                .HasMaxLength(255)
                .HasColumnName("compteurid");
            entity.Property(e => e.Dateenr)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateenr");
            entity.Property(e => e.Dateenrresultat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateenrresultat");
            entity.Property(e => e.Idcommande).HasColumnName("idcommande");
            entity.Property(e => e.Libellegroupe)
                .HasMaxLength(255)
                .HasColumnName("libellegroupe");
            entity.Property(e => e.Resultats)
                .HasMaxLength(255)
                .HasColumnName("resultats");

            entity.HasOne(d => d.Compteur).WithMany(p => p.Commandecompteurs)
                .HasForeignKey(d => d.Compteurid)
                .HasConstraintName("commandecompteurs_compteurid_fkey");

            entity.HasOne(d => d.IdcommandeNavigation).WithMany(p => p.Commandecompteurs)
                .HasForeignKey(d => d.Idcommande)
                .HasConstraintName("commandecompteurs_idcommande_fkey");
        });

        modelBuilder.Entity<Compte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Compte_pkey");

            entity.ToTable("Compte", "sc_sge");

            entity.HasIndex(e => e.Code, "compte_code");

            entity.HasIndex(e => e.Numero, "compte_numero");

            entity.HasIndex(e => e.Status, "compte_status");

            entity.HasIndex(e => e.TypeId, "compte_type_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Numero)
                .HasMaxLength(255)
                .HasColumnName("numero");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Type).WithMany(p => p.Comptes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Compte_type_id_fkey");
        });

        modelBuilder.Entity<CompteRenduEnqueteTerrain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduEnqueteTerrain_pkey");

            entity.ToTable("CompteRenduEnqueteTerrain", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbonnementExistant)
                .HasDefaultValue(true)
                .HasColumnName("abonnement_existant");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BranchementExistant)
                .HasDefaultValue(true)
                .HasColumnName("branchement_existant");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEnquete).HasColumnName("date_enquete");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.EnquetePossible)
                .HasDefaultValue(true)
                .HasColumnName("enquete_possible");
            entity.Property(e => e.EtatAbonnementId).HasColumnName("etat_abonnement_id");
            entity.Property(e => e.EtatBranchementId).HasColumnName("etat_branchement_id");
            entity.Property(e => e.MotifImpossibiliteId).HasColumnName("motif_impossibilite_id");
            entity.Property(e => e.Observation)
                .HasMaxLength(255)
                .HasColumnName("observation");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Demande).WithMany(p => p.CompteRenduEnqueteTerrains)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduEnqueteTerrain_demande_id_fkey");

            entity.HasOne(d => d.EtatAbonnement).WithMany(p => p.CompteRenduEnqueteTerrains)
                .HasForeignKey(d => d.EtatAbonnementId)
                .HasConstraintName("CompteRenduEnqueteTerrain_etat_abonnement_id_fkey");

            entity.HasOne(d => d.EtatBranchement).WithMany(p => p.CompteRenduEnqueteTerrains)
                .HasForeignKey(d => d.EtatBranchementId)
                .HasConstraintName("CompteRenduEnqueteTerrain_etat_branchement_id_fkey");
        });

        modelBuilder.Entity<CompteRenduEnqueteTerrainIntervenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduEnqueteTerrainIntervenant_pkey");

            entity.ToTable("CompteRenduEnqueteTerrainIntervenant", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserCentre)
                .HasMaxLength(255)
                .HasColumnName("user_centre");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.UserMatricule)
                .HasMaxLength(255)
                .HasColumnName("user_matricule");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
            entity.Property(e => e.UserSite)
                .HasMaxLength(255)
                .HasColumnName("user_site");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.CompteRenduEnqueteTerrainIntervenants)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduEnqueteTerrainIntervenant_compte_rendu_id_fkey");
        });

        modelBuilder.Entity<CompteRenduMetre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduMetre_pkey");

            entity.ToTable("CompteRenduMetre", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.DiametreConduiteId).HasColumnName("diametre_conduite_id");
            entity.Property(e => e.DiametreTuyauId).HasColumnName("diametre_tuyau_id");
            entity.Property(e => e.Latitude)
                .HasMaxLength(255)
                .HasColumnName("latitude");
            entity.Property(e => e.LineaireFacade).HasColumnName("lineaire_facade");
            entity.Property(e => e.LineaireTuyau).HasColumnName("lineaire_tuyau");
            entity.Property(e => e.Logitude)
                .HasMaxLength(255)
                .HasColumnName("logitude");
            entity.Property(e => e.NatureConduiteId).HasColumnName("nature_conduite_id");
            entity.Property(e => e.NatureTuyauId).HasColumnName("nature_tuyau_id");
            entity.Property(e => e.NombreHabitant).HasColumnName("nombre_habitant");
            entity.Property(e => e.NombrePointDeau).HasColumnName("nombre_point_deau");
            entity.Property(e => e.NumeroAbonnementVoisin).HasColumnName("numero_abonnement_voisin");
            entity.Property(e => e.NumeroSchema)
                .HasMaxLength(255)
                .HasColumnName("numero_schema");
            entity.Property(e => e.Observations)
                .HasMaxLength(255)
                .HasColumnName("observations");
            entity.Property(e => e.ReferenceExtension)
                .HasMaxLength(255)
                .HasColumnName("reference_extension");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeBranchementId).HasColumnName("type_branchement_id");
            entity.Property(e => e.TypeTravauxId).HasColumnName("type_travaux_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlSchema)
                .HasMaxLength(255)
                .HasColumnName("url_schema");
            entity.Property(e => e.UsagePrincipalId).HasColumnName("usage_principal_id");
            entity.Property(e => e.UsageSecondaireId).HasColumnName("usage_secondaire_id");

            entity.HasOne(d => d.Demande).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduMetre_demande_id_fkey");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.DiametreCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduMetre_diametre_compteur_id_fkey");

            entity.HasOne(d => d.DiametreConduite).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.DiametreConduiteId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_diametre_conduite_id_fkey");

            entity.HasOne(d => d.DiametreTuyau).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.DiametreTuyauId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_diametre_tuyau_id_fkey");

            entity.HasOne(d => d.NatureConduite).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.NatureConduiteId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_nature_conduite_id_fkey");

            entity.HasOne(d => d.NatureTuyau).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.NatureTuyauId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_nature_tuyau_id_fkey");

            entity.HasOne(d => d.TypeBranchement).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.TypeBranchementId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_type_branchement_id_fkey");

            entity.HasOne(d => d.TypeTravaux).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.TypeTravauxId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_type_travaux_id_fkey");

            entity.HasOne(d => d.UsagePrincipal).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.UsagePrincipalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_usage_principal_id_fkey");

            entity.HasOne(d => d.UsageSecondaire).WithMany(p => p.CompteRenduMetres)
                .HasForeignKey(d => d.UsageSecondaireId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CompteRenduMetre_usage_secondaire_id_fkey");
        });

        modelBuilder.Entity<CompteRenduMetreIntervenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduMetreIntervenant_pkey");

            entity.ToTable("CompteRenduMetreIntervenant", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserCentre)
                .HasMaxLength(255)
                .HasColumnName("user_centre");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.UserMatricule)
                .HasMaxLength(255)
                .HasColumnName("user_matricule");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
            entity.Property(e => e.UserSite)
                .HasMaxLength(255)
                .HasColumnName("user_site");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.CompteRenduMetreIntervenants)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduMetreIntervenant_compte_rendu_id_fkey");
        });

        modelBuilder.Entity<CompteRenduMetrePointsPiquage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduMetrePointsPiquage_pkey");

            entity.ToTable("CompteRenduMetrePointsPiquage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.PointsPiquageId).HasColumnName("points_piquage_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.CompteRenduMetrePointsPiquages)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduMetrePointsPiquage_compte_rendu_id_fkey");

            entity.HasOne(d => d.PointsPiquage).WithMany(p => p.CompteRenduMetrePointsPiquages)
                .HasForeignKey(d => d.PointsPiquageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduMetrePointsPiquage_points_piquage_id_fkey");
        });

        modelBuilder.Entity<CompteRenduTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduTravaux_pkey");

            entity.ToTable("CompteRenduTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutEffective).HasColumnName("date_debut_effective");
            entity.Property(e => e.DateDebutPrevu).HasColumnName("date_debut_prevu");
            entity.Property(e => e.DateFinEffective).HasColumnName("date_fin_effective");
            entity.Property(e => e.DateFinPrevu).HasColumnName("date_fin_prevu");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.Latitude)
                .HasMaxLength(255)
                .HasColumnName("latitude");
            entity.Property(e => e.Logitude)
                .HasMaxLength(255)
                .HasColumnName("logitude");
            entity.Property(e => e.NumeroBi)
                .HasMaxLength(255)
                .HasColumnName("numero_bi");
            entity.Property(e => e.NumeroBs)
                .HasMaxLength(255)
                .HasColumnName("numero_bs");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Demande).WithMany(p => p.CompteRenduTravauxes)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduTravaux_demande_id_fkey");
        });

        modelBuilder.Entity<CompteRenduTravauxBilanMateriel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduTravauxBilanMateriel_pkey");

            entity.ToTable("CompteRenduTravauxBilanMateriel", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Ecart).HasColumnName("ecart");
            entity.Property(e => e.MaterielCoutUnitaire).HasColumnName("materiel_cout_unitaire");
            entity.Property(e => e.MaterielDevisId).HasColumnName("materiel_devis_id");
            entity.Property(e => e.MaterielLibelle)
                .HasMaxLength(255)
                .HasColumnName("materiel_libelle");
            entity.Property(e => e.QuantiteSortie).HasColumnName("quantite_sortie");
            entity.Property(e => e.QuantiteUtilisee).HasColumnName("quantite_utilisee");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.CompteRenduTravauxBilanMateriels)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduTravauxBilanMateriel_compte_rendu_id_fkey");

            entity.HasOne(d => d.MaterielDevis).WithMany(p => p.CompteRenduTravauxBilanMateriels)
                .HasForeignKey(d => d.MaterielDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduTravauxBilanMateriel_materiel_devis_id_fkey");
        });

        modelBuilder.Entity<CompteRenduTravauxIntervenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduTravauxIntervenant_pkey");

            entity.ToTable("CompteRenduTravauxIntervenant", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserCentre)
                .HasMaxLength(255)
                .HasColumnName("user_centre");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.UserMatricule)
                .HasMaxLength(255)
                .HasColumnName("user_matricule");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
            entity.Property(e => e.UserSite)
                .HasMaxLength(255)
                .HasColumnName("user_site");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.CompteRenduTravauxIntervenants)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduTravauxIntervenant_compte_rendu_id_fkey");
        });

        modelBuilder.Entity<CompteRenduTravauxMaterielRecupere>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteRenduTravauxMaterielRecupere_pkey");

            entity.ToTable("CompteRenduTravauxMaterielRecupere", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduId).HasColumnName("compte_rendu_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MaterielCoutEstime).HasColumnName("materiel_cout_estime");
            entity.Property(e => e.MaterielDevisId).HasColumnName("materiel_devis_id");
            entity.Property(e => e.MaterielLibelle)
                .HasMaxLength(255)
                .HasColumnName("materiel_libelle");
            entity.Property(e => e.MontantTotalEstime).HasColumnName("montant_total_estime");
            entity.Property(e => e.QuantiteRecupere).HasColumnName("quantite_recupere");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CompteRendu).WithMany(p => p.CompteRenduTravauxMaterielRecuperes)
                .HasForeignKey(d => d.CompteRenduId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduTravauxMaterielRecupere_compte_rendu_id_fkey");

            entity.HasOne(d => d.MaterielDevis).WithMany(p => p.CompteRenduTravauxMaterielRecuperes)
                .HasForeignKey(d => d.MaterielDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteRenduTravauxMaterielRecupere_materiel_devis_id_fkey");
        });

        modelBuilder.Entity<Compteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Compteur_pkey");

            entity.ToTable("Compteur", "sc_sge");

            entity.HasIndex(e => e.NumeroCompteur, "Compteur_numero_compteur_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdresseIp)
                .HasMaxLength(100)
                .HasColumnName("adresse_ip");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.AnneeFabrication)
                .HasMaxLength(255)
                .HasColumnName("annee_fabrication");
            entity.Property(e => e.CoefLecture).HasColumnName("coef_lecture");
            entity.Property(e => e.CrcFirmware)
                .HasMaxLength(50)
                .HasColumnName("crc_firmware");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DataConcentrator)
                .HasMaxLength(50)
                .HasColumnName("data_concentrator");
            entity.Property(e => e.DatePoseActuelle).HasColumnName("date_pose_actuelle");
            entity.Property(e => e.DatePremierePose).HasColumnName("date_premiere_pose");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DonneesManaged)
                .HasColumnType("jsonb")
                .HasColumnName("donnees_managed");
            entity.Property(e => e.DonneesPublic)
                .HasColumnType("jsonb")
                .HasColumnName("donnees_public");
            entity.Property(e => e.DonneesRead)
                .HasColumnType("jsonb")
                .HasColumnName("donnees_read");
            entity.Property(e => e.EnergyProfilePeriod)
                .HasMaxLength(10)
                .HasColumnName("energy_profile_period");
            entity.Property(e => e.EtatCompteurId).HasColumnName("etat_compteur_id");
            entity.Property(e => e.EtatCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("etat_compteur_libelle");
            entity.Property(e => e.Etatcontacteur)
                .HasMaxLength(255)
                .HasColumnName("etatcontacteur");
            entity.Property(e => e.Idfabricant)
                .HasMaxLength(255)
                .HasColumnName("idfabricant");
            entity.Property(e => e.IndexActuel).HasColumnName("index_actuel");
            entity.Property(e => e.MarqueCompteurId).HasColumnName("marque_compteur_id");
            entity.Property(e => e.MarqueCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("marque_compteur_libelle");
            entity.Property(e => e.NombreCadrant).HasColumnName("nombre_cadrant");
            entity.Property(e => e.NombreFile).HasColumnName("nombre_file");
            entity.Property(e => e.NumeroCompteur)
                .HasMaxLength(255)
                .HasColumnName("numero_compteur");
            entity.Property(e => e.Phases)
                .HasMaxLength(10)
                .HasColumnName("phases");
            entity.Property(e => e.Port)
                .HasMaxLength(10)
                .HasColumnName("port");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutCompteurId).HasColumnName("statut_compteur_id");
            entity.Property(e => e.StatutCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_compteur_libelle");
            entity.Property(e => e.Tarif)
                .HasMaxLength(10)
                .HasColumnName("tarif");
            entity.Property(e => e.TechnicalProfilePeriod)
                .HasMaxLength(10)
                .HasColumnName("technical_profile_period");
            entity.Property(e => e.TimeDifference)
                .HasMaxLength(255)
                .HasColumnName("time_difference");
            entity.Property(e => e.TypeCompteurId).HasColumnName("type_compteur_id");
            entity.Property(e => e.TypeCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_compteur_libelle");
            entity.Property(e => e.TypeOfTransport)
                .HasMaxLength(50)
                .HasColumnName("type_of_transport");
            entity.Property(e => e.Typecompteur)
                .HasMaxLength(255)
                .HasColumnName("typecompteur");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VersionFirmware)
                .HasMaxLength(255)
                .HasColumnName("version_firmware");
            entity.Property(e => e.VersionFirmwareModem)
                .HasMaxLength(255)
                .HasColumnName("version_firmware_modem");

            entity.HasOne(d => d.EtatCompteur).WithMany(p => p.Compteurs)
                .HasForeignKey(d => d.EtatCompteurId)
                .HasConstraintName("Compteur_etat_compteur_id_fkey");

            entity.HasOne(d => d.MarqueCompteur).WithMany(p => p.Compteurs)
                .HasForeignKey(d => d.MarqueCompteurId)
                .HasConstraintName("Compteur_marque_compteur_id_fkey");

            entity.HasOne(d => d.StatutCompteur).WithMany(p => p.Compteurs)
                .HasForeignKey(d => d.StatutCompteurId)
                .HasConstraintName("Compteur_statut_compteur_id_fkey");

            entity.HasOne(d => d.TypeCompteur).WithMany(p => p.Compteurs)
                .HasForeignKey(d => d.TypeCompteurId)
                .HasConstraintName("Compteur_type_compteur_id_fkey");
        });

        modelBuilder.Entity<CompteurProfil>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteurProfil_pkey");

            entity.ToTable("CompteurProfil", "sc_sge");

            entity.HasIndex(e => e.Nom, "CompteurProfil_nom_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteurId)
                .HasMaxLength(255)
                .HasColumnName("compteur_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Mdp)
                .HasMaxLength(255)
                .HasColumnName("mdp");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CompteurProfilParametre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CompteurProfilParametre_pkey");

            entity.ToTable("CompteurProfilParametre", "sc_sge");

            entity.HasIndex(e => e.Cle, "CompteurProfilParametre_cle_key").IsUnique();

            entity.HasIndex(e => e.Valeur, "CompteurProfilParametre_valeur_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Cle)
                .HasMaxLength(255)
                .HasColumnName("cle");
            entity.Property(e => e.CompteurProfilId).HasColumnName("compteur_profil_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Valeur)
                .HasMaxLength(255)
                .HasColumnName("valeur");

            entity.HasOne(d => d.CompteurProfil).WithMany(p => p.CompteurProfilParametres)
                .HasForeignKey(d => d.CompteurProfilId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CompteurProfilParametre_compteur_profil_id_fkey");
        });

        modelBuilder.Entity<Continent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Continent_pkey");

            entity.ToTable("Continent");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Continent1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Continent_pkey");

            entity.ToTable("Continent", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Contrat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Contrat_pkey");

            entity.ToTable("Contrat", "sc_sge");

            entity.HasIndex(e => e.NumeroContrat, "Contrat_numero_contrat_key").IsUnique();

            entity.HasIndex(e => e.NumeroContrat, "Contrat_numero_contrat_key1").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DataObject)
                .HasColumnType("json")
                .HasColumnName("data_object");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.NumeroContrat)
                .HasMaxLength(255)
                .HasColumnName("numero_contrat");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.ProduitLibelle)
                .HasMaxLength(255)
                .HasColumnName("produit_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeContratId).HasColumnName("type_contrat_id");
            entity.Property(e => e.TypeContratLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_contrat_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.Contrats)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contrat_produit_id_fkey");

            entity.HasOne(d => d.TypeContrat).WithMany(p => p.Contrats)
                .HasForeignKey(d => d.TypeContratId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contrat_type_contrat_id_fkey");
        });

        modelBuilder.Entity<ContratSignataire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ContratSignataires_pkey");

            entity.ToTable("ContratSignataires", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ContratId)
                .HasMaxLength(255)
                .HasColumnName("contrat_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateNaissance).HasColumnName("date_naissance");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.Prenoms)
                .HasMaxLength(255)
                .HasColumnName("prenoms");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Telephone)
                .HasMaxLength(255)
                .HasColumnName("telephone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Contrat).WithMany(p => p.ContratSignataires)
                .HasForeignKey(d => d.ContratId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ContratSignataires_contrat_id_fkey");
        });

        modelBuilder.Entity<CoreConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoreConfig_pkey");

            entity.ToTable("CoreConfig");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ApiBaseUrl)
                .HasMaxLength(255)
                .HasColumnName("api_base_url");
            entity.Property(e => e.AppDefaultLang)
                .HasMaxLength(255)
                .HasDefaultValueSql("'fr'::character varying")
                .HasColumnName("app_default_lang");
            entity.Property(e => e.AppName)
                .HasMaxLength(255)
                .HasColumnName("app_name");
            entity.Property(e => e.AppVersion)
                .HasMaxLength(255)
                .HasColumnName("app_version");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EmailFrom)
                .HasMaxLength(255)
                .HasColumnName("email_from");
            entity.Property(e => e.EmailHost)
                .HasMaxLength(255)
                .HasColumnName("email_host");
            entity.Property(e => e.EmailPassword)
                .HasMaxLength(255)
                .HasColumnName("email_password");
            entity.Property(e => e.EmailPort)
                .HasMaxLength(255)
                .HasColumnName("email_port");
            entity.Property(e => e.EmailService)
                .HasMaxLength(255)
                .HasDefaultValueSql("'gmail'::character varying")
                .HasColumnName("email_service");
            entity.Property(e => e.EmailUsername)
                .HasMaxLength(255)
                .HasColumnName("email_username");
            entity.Property(e => e.ExchangeKey)
                .HasMaxLength(255)
                .HasDefaultValueSql("'orilove'::character varying")
                .HasColumnName("exchange_key");
            entity.Property(e => e.NotifEmail)
                .HasDefaultValue(false)
                .HasColumnName("notif_email");
            entity.Property(e => e.NotifPush)
                .HasDefaultValue(false)
                .HasColumnName("notif_push");
            entity.Property(e => e.NotifSms)
                .HasDefaultValue(false)
                .HasColumnName("notif_sms");
            entity.Property(e => e.OauthFacebook)
                .HasDefaultValue(false)
                .HasColumnName("oauth_facebook");
            entity.Property(e => e.OauthGoogle)
                .HasDefaultValue(false)
                .HasColumnName("oauth_google");
            entity.Property(e => e.OauthSso)
                .HasDefaultValue(false)
                .HasColumnName("oauth_sso");
            entity.Property(e => e.SessionUserCryptKey)
                .HasMaxLength(255)
                .HasColumnName("session_user_crypt_key");
            entity.Property(e => e.SessionUserKey)
                .HasMaxLength(255)
                .HasColumnName("session_user_key");
            entity.Property(e => e.SmsFrom)
                .HasMaxLength(255)
                .HasColumnName("sms_from");
            entity.Property(e => e.SmsHost)
                .HasMaxLength(255)
                .HasColumnName("sms_host");
            entity.Property(e => e.SmsPassword)
                .HasMaxLength(255)
                .HasColumnName("sms_password");
            entity.Property(e => e.SmsPort)
                .HasMaxLength(255)
                .HasColumnName("sms_port");
            entity.Property(e => e.SmsUsername)
                .HasMaxLength(255)
                .HasColumnName("sms_username");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UploadDir)
                .HasMaxLength(255)
                .HasColumnName("upload_dir");
            entity.Property(e => e.WebsocketBaseUrl)
                .HasMaxLength(255)
                .HasColumnName("websocket_base_url");
        });

        modelBuilder.Entity<CoreLang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoreLang_pkey");

            entity.ToTable("CoreLang");

            entity.HasIndex(e => e.Code, "CoreLang_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key13").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key14").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key15").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key16").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key17").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key18").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key19").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key20").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key21").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key22").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key23").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key24").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key25").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key26").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key27").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key28").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key29").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key30").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key31").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key32").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key33").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key34").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key35").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key36").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key37").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "CoreLang_code_key9").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Flag)
                .HasMaxLength(255)
                .HasColumnName("flag");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CoreNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoreNotification_pkey");

            entity.ToTable("CoreNotification");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Lang)
                .HasMaxLength(255)
                .HasDefaultValueSql("'fr'::character varying")
                .HasColumnName("lang");
            entity.Property(e => e.Level)
                .HasMaxLength(255)
                .HasColumnName("level");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("subject");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CoreParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoreParameter_pkey");

            entity.ToTable("CoreParameter");

            entity.HasIndex(e => e.Key, "CoreParameter_key_key").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key1").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key10").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key11").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key12").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key13").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key14").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key15").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key16").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key17").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key18").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key19").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key2").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key20").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key21").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key22").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key23").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key24").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key25").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key26").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key27").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key28").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key29").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key3").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key30").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key31").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key32").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key33").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key34").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key35").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key36").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key37").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key38").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key39").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key4").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key40").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key41").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key42").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key43").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key44").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key45").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key46").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key5").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key6").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key7").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key8").IsUnique();

            entity.HasIndex(e => e.Key, "CoreParameter_key_key9").IsUnique();

            entity.HasIndex(e => e.Category, "coreparameter_category_idx");

            entity.HasIndex(e => e.Family, "coreparameter_family_idx");

            entity.HasIndex(e => e.IsLoadOnstart, "coreparameter_is_load_onstart_idx");

            entity.HasIndex(e => e.ParentKey, "coreparameter_parent_key_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Family)
                .HasMaxLength(255)
                .HasColumnName("family");
            entity.Property(e => e.IsLoadOnstart)
                .HasDefaultValue(true)
                .HasColumnName("is_load_onstart");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.ParentKey)
                .HasMaxLength(255)
                .HasColumnName("parent_key");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");
        });

        modelBuilder.Entity<CoreTheme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoreTheme_pkey");

            entity.ToTable("CoreTheme");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.HeaderBgcolor)
                .HasMaxLength(255)
                .HasColumnName("header_bgcolor");
            entity.Property(e => e.HeaderLangs)
                .HasDefaultValue(true)
                .HasColumnName("header_langs");
            entity.Property(e => e.HeaderLogo)
                .HasMaxLength(255)
                .HasColumnName("header_logo");
            entity.Property(e => e.HeaderNotifications)
                .HasDefaultValue(false)
                .HasColumnName("header_notifications");
            entity.Property(e => e.HeaderSearchbar)
                .HasDefaultValue(false)
                .HasColumnName("header_searchbar");
            entity.Property(e => e.HeaderTextcolor)
                .HasMaxLength(255)
                .HasColumnName("header_textcolor");
            entity.Property(e => e.HeaderUseractions)
                .HasDefaultValue(true)
                .HasColumnName("header_useractions");
            entity.Property(e => e.LayoutMainnav)
                .HasDefaultValue(false)
                .HasColumnName("layout_mainnav");
            entity.Property(e => e.LayoutOrientation)
                .HasMaxLength(255)
                .HasColumnName("layout_orientation");
            entity.Property(e => e.LayoutPrimarycolor)
                .HasMaxLength(255)
                .HasColumnName("layout_primarycolor");
            entity.Property(e => e.LayoutSidenav)
                .HasDefaultValue(true)
                .HasColumnName("layout_sidenav");
            entity.Property(e => e.LayoutTextcolor)
                .HasMaxLength(255)
                .HasColumnName("layout_textcolor");
            entity.Property(e => e.LayoutTheme)
                .HasMaxLength(255)
                .HasColumnName("layout_theme");
            entity.Property(e => e.MainnavBgcolor)
                .HasMaxLength(255)
                .HasColumnName("mainnav_bgcolor");
            entity.Property(e => e.MainnavFontsize)
                .HasMaxLength(255)
                .HasDefaultValueSql("'normal'::character varying")
                .HasColumnName("mainnav_fontsize");
            entity.Property(e => e.MainnavFontweight)
                .HasMaxLength(255)
                .HasDefaultValueSql("'normal'::character varying")
                .HasColumnName("mainnav_fontweight");
            entity.Property(e => e.MainnavTextcolor)
                .HasMaxLength(255)
                .HasDefaultValueSql("'#fff'::character varying")
                .HasColumnName("mainnav_textcolor");
            entity.Property(e => e.SidenavBgcolor)
                .HasMaxLength(255)
                .HasColumnName("sidenav_bgcolor");
            entity.Property(e => e.SidenavCollapsed)
                .HasDefaultValue(false)
                .HasColumnName("sidenav_collapsed");
            entity.Property(e => e.SidenavCollapsible)
                .HasDefaultValue(true)
                .HasColumnName("sidenav_collapsible");
            entity.Property(e => e.SidenavTextcolor)
                .HasMaxLength(255)
                .HasColumnName("sidenav_textcolor");
            entity.Property(e => e.SidenavWidth)
                .HasMaxLength(255)
                .HasColumnName("sidenav_width");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<CoreTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoreTranslation_pkey");

            entity.ToTable("CoreTranslation");

            entity.HasIndex(e => e.Key, "coretranslation_key_idx");

            entity.HasIndex(e => e.LangCode, "coretranslation_lang_code_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.LangCode)
                .HasMaxLength(255)
                .HasColumnName("lang_code");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.LangCodeNavigation).WithMany(p => p.CoreTranslations)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.LangCode)
                .HasConstraintName("CoreTranslation_lang_code_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Country_pkey");

            entity.ToTable("Country");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.ContinentId)
                .HasMaxLength(255)
                .HasColumnName("continent_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Flag)
                .HasMaxLength(255)
                .HasColumnName("flag");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PhonePrefix)
                .HasMaxLength(255)
                .HasDefaultValueSql("'+225'::character varying")
                .HasColumnName("phone_prefix");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Continent).WithMany(p => p.Countries)
                .HasForeignKey(d => d.ContinentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Country_continent_id_fkey");
        });

        modelBuilder.Entity<Country1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Country_pkey");

            entity.ToTable("Country", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.ContinentId)
                .HasMaxLength(255)
                .HasColumnName("continent_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Flag)
                .HasMaxLength(255)
                .HasColumnName("flag");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PhonePrefix)
                .HasMaxLength(255)
                .HasDefaultValueSql("'+225'::character varying")
                .HasColumnName("phone_prefix");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Continent).WithMany(p => p.Country1s)
                .HasForeignKey(d => d.ContinentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Country_continent_id_fkey");
        });

        modelBuilder.Entity<CoutDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CoutDemande_pkey");

            entity.ToTable("CoutDemande", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CategorieClientCode)
                .HasMaxLength(255)
                .HasColumnName("categorie_client_code");
            entity.Property(e => e.CategorieClientId).HasColumnName("categorie_client_id");
            entity.Property(e => e.CategorieClientLibelle)
                .HasMaxLength(255)
                .HasColumnName("categorie_client_libelle");
            entity.Property(e => e.CentreCode)
                .HasMaxLength(255)
                .HasColumnName("centre_code");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CodeOperationCode)
                .HasMaxLength(255)
                .HasColumnName("code_operation_code");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.ProduitLibelle)
                .HasMaxLength(255)
                .HasColumnName("produit_libelle");
            entity.Property(e => e.PuissanceSouscriteCode)
                .HasMaxLength(255)
                .HasColumnName("puissance_souscrite_code");
            entity.Property(e => e.PuissanceSouscriteId).HasColumnName("puissance_souscrite_id");
            entity.Property(e => e.PuissanceSouscriteLibelle)
                .HasMaxLength(255)
                .HasColumnName("puissance_souscrite_libelle");
            entity.Property(e => e.ReglageCompteurCode)
                .HasMaxLength(255)
                .HasColumnName("reglage_compteur_code");
            entity.Property(e => e.ReglageCompteurId).HasColumnName("reglage_compteur_id");
            entity.Property(e => e.ReglageCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("reglage_compteur_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TaxeCode)
                .HasMaxLength(255)
                .HasColumnName("taxe_code");
            entity.Property(e => e.TaxeId).HasColumnName("taxe_id");
            entity.Property(e => e.TaxeLibelle)
                .HasMaxLength(255)
                .HasColumnName("taxe_libelle");
            entity.Property(e => e.TypeDemandeCode)
                .HasMaxLength(255)
                .HasColumnName("type_demande_code");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.TypeDemandeLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_demande_libelle");
            entity.Property(e => e.TypeTarifCode)
                .HasMaxLength(255)
                .HasColumnName("type_tarif_code");
            entity.Property(e => e.TypeTarifId).HasColumnName("type_tarif_id");
            entity.Property(e => e.TypeTarifLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_tarif_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CategorieClient).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.CategorieClientId)
                .HasConstraintName("CoutDemande_categorie_client_id_fkey");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CoutDemande_code_operation_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CoutDemande_produit_id_fkey");

            entity.HasOne(d => d.PuissanceSouscrite).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.PuissanceSouscriteId)
                .HasConstraintName("CoutDemande_puissance_souscrite_id_fkey");

            entity.HasOne(d => d.ReglageCompteur).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.ReglageCompteurId)
                .HasConstraintName("CoutDemande_reglage_compteur_id_fkey");

            entity.HasOne(d => d.Taxe).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.TaxeId)
                .HasConstraintName("CoutDemande_taxe_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.TypeDemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CoutDemande_type_demande_id_fkey");

            entity.HasOne(d => d.TypeTarif).WithMany(p => p.CoutDemandes)
                .HasForeignKey(d => d.TypeTarifId)
                .HasConstraintName("CoutDemande_type_tarif_id_fkey");
        });

        modelBuilder.Entity<Dayactionspassive>(entity =>
        {
            entity.HasKey(e => e.Actionpassiveid).HasName("dayactionspassive_pkey");

            entity.ToTable("dayactionspassive", "sc_sge");

            entity.Property(e => e.Actionpassiveid).HasColumnName("actionpassiveid");
            entity.Property(e => e.Dayprofilepassiveid).HasColumnName("dayprofilepassiveid");
            entity.Property(e => e.Logicalname)
                .HasMaxLength(50)
                .HasColumnName("logicalname");
            entity.Property(e => e.Selector).HasColumnName("selector");
            entity.Property(e => e.Start)
                .HasMaxLength(50)
                .HasColumnName("start");

            entity.HasOne(d => d.Dayprofilepassive).WithMany(p => p.Dayactionspassives)
                .HasForeignKey(d => d.Dayprofilepassiveid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("dayactionspassive_dayprofilepassiveid_fkey");
        });

        modelBuilder.Entity<Dayprofiletableactive>(entity =>
        {
            entity.HasKey(e => e.Dayprofileactiveid).HasName("dayprofiletableactive_pkey");

            entity.ToTable("dayprofiletableactive", "sc_sge");

            entity.Property(e => e.Dayprofileactiveid).HasColumnName("dayprofileactiveid");
            entity.Property(e => e.Dayid).HasColumnName("dayid");
            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");

            entity.HasOne(d => d.Gxdlmsactivitycalendar).WithMany(p => p.Dayprofiletableactives)
                .HasForeignKey(d => d.Gxdlmsactivitycalendarid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("dayprofiletableactive_gxdlmsactivitycalendarid_fkey");
        });

        modelBuilder.Entity<Dayprofiletablepassive>(entity =>
        {
            entity.HasKey(e => e.Dayprofilepassiveid).HasName("dayprofiletablepassive_pkey");

            entity.ToTable("dayprofiletablepassive", "sc_sge");

            entity.Property(e => e.Dayprofilepassiveid).HasColumnName("dayprofilepassiveid");
            entity.Property(e => e.Dayid).HasColumnName("dayid");
            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");

            entity.HasOne(d => d.Gxdlmsactivitycalendar).WithMany(p => p.Dayprofiletablepassives)
                .HasForeignKey(d => d.Gxdlmsactivitycalendarid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("dayprofiletablepassive_gxdlmsactivitycalendarid_fkey");
        });

        modelBuilder.Entity<Demande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Demande_pkey");

            entity.ToTable("Demande", "sc_sge");

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key1").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key10").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key11").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key12").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key13").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key14").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key15").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key16").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key17").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key18").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key19").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key2").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key20").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key21").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key22").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key23").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key24").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key25").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key26").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key27").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key28").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key29").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key3").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key30").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key31").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key32").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key4").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key5").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key6").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key7").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key8").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "Demande_numero_demande_key9").IsUnique();

            entity.HasIndex(e => e.NumeroDemande, "demande_numero_demande_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminId)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas saisie par le client")
                .HasColumnName("admin_id");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas saisie par le client")
                .HasColumnName("centre_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CodeCentre)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas saisie par le client")
                .HasColumnName("code_centre");
            entity.Property(e => e.CodeProduit)
                .HasMaxLength(255)
                .HasColumnName("code_produit");
            entity.Property(e => e.CodeTypeDemande)
                .HasMaxLength(255)
                .HasColumnName("code_type_demande");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateCaisse).HasColumnName("date_caisse");
            entity.Property(e => e.DateDemande).HasColumnName("date_demande");
            entity.Property(e => e.DateFinTraitement).HasColumnName("date_fin_traitement");
            entity.Property(e => e.DatePrevuRdv).HasColumnName("date_prevu_rdv");
            entity.Property(e => e.DatePrevuRetraitDevis).HasColumnName("date_prevu_retrait_devis");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DiametreCompteurId)
                .HasComment("Obligatoire si le produit est eau")
                .HasColumnName("diametre_compteur_id");
            entity.Property(e => e.EmailNotification)
                .HasMaxLength(255)
                .HasColumnName("email_notification");
            entity.Property(e => e.EntiteDemandeurId)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas initié par le client (is_demande_client == false). ça peut être le service qui initie la demande")
                .HasColumnName("entite_demandeur_id");
            entity.Property(e => e.EtapeCode)
                .HasMaxLength(255)
                .HasColumnName("etape_code");
            entity.Property(e => e.EtapeId)
                .HasMaxLength(255)
                .HasColumnName("etape_id");
            entity.Property(e => e.EtapeLibelle)
                .HasMaxLength(255)
                .HasColumnName("etape_libelle");
            entity.Property(e => e.IsDemandeClient)
                .HasDefaultValue(true)
                .HasColumnName("is_demande_client");
            entity.Property(e => e.IsProprietaire)
                .HasDefaultValue(true)
                .HasColumnName("is_proprietaire");
            entity.Property(e => e.LibelleDiametreCompteur)
                .HasMaxLength(255)
                .HasComment("Obligatoire si le produit est eau")
                .HasColumnName("libelle_diametre_compteur");
            entity.Property(e => e.LibelleMotifDemande)
                .HasMaxLength(255)
                .HasColumnName("libelle_motif_demande");
            entity.Property(e => e.LibelleProduit)
                .HasMaxLength(255)
                .HasColumnName("libelle_produit");
            entity.Property(e => e.LibellePuissanceSouscrite)
                .HasMaxLength(255)
                .HasComment("Obligatoire si le produit est electricité")
                .HasColumnName("libelle_puissance_souscrite");
            entity.Property(e => e.LibelleReglageCompteur)
                .HasMaxLength(255)
                .HasColumnName("libelle_reglage_compteur");
            entity.Property(e => e.LibelleStatutDemande)
                .HasMaxLength(255)
                .HasColumnName("libelle_statut_demande");
            entity.Property(e => e.LibelleTypeComptage)
                .HasMaxLength(255)
                .HasColumnName("libelle_type_comptage");
            entity.Property(e => e.LibelleTypeDemande)
                .HasMaxLength(255)
                .HasColumnName("libelle_type_demande");
            entity.Property(e => e.MatriculeAdmin)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas saisie par le client")
                .HasColumnName("matricule_admin");
            entity.Property(e => e.MotifDemandeId).HasColumnName("motif_demande_id");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas saisie par le client")
                .HasColumnName("nom_centre");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NomEntiteDemandeur)
                .HasMaxLength(255)
                .HasComment("Obligatoire si la demande n'est pas initié par le client (is_demande_client == false). ça peut être le service qui initie la demande")
                .HasColumnName("nom_entite_demandeur");
            entity.Property(e => e.NombreFoyer).HasColumnName("nombre_foyer");
            entity.Property(e => e.NotificationAcceptStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_accept_status");
            entity.Property(e => e.NotificationCloseStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_close_status");
            entity.Property(e => e.NotificationRejectStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_reject_status");
            entity.Property(e => e.NotificationSaveStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_save_status");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroDemandeInitiale)
                .HasMaxLength(255)
                .HasColumnName("numero_demande_initiale");
            entity.Property(e => e.NumeroDevis).HasColumnName("numero_devis");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.PuissanceSouscriteId)
                .HasComment("Obligatoire si le produit est electricité")
                .HasColumnName("puissance_souscrite_id");
            entity.Property(e => e.ReglageCompteurId).HasColumnName("reglage_compteur_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutDemandeId).HasColumnName("statut_demande_id");
            entity.Property(e => e.TelephoneNotification)
                .HasMaxLength(255)
                .HasColumnName("telephone_notification");
            entity.Property(e => e.TypeComptageId).HasColumnName("type_comptage_id");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Client).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Demande_client_id_fkey");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.DiametreCompteurId)
                .HasConstraintName("Demande_diametre_compteur_id_fkey");

            entity.HasOne(d => d.MotifDemande).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.MotifDemandeId)
                .HasConstraintName("Demande_motif_demande_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.ProduitId)
                .HasConstraintName("Demande_produit_id_fkey");

            entity.HasOne(d => d.PuissanceSouscrite).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.PuissanceSouscriteId)
                .HasConstraintName("Demande_puissance_souscrite_id_fkey");

            entity.HasOne(d => d.ReglageCompteur).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.ReglageCompteurId)
                .HasConstraintName("Demande_reglage_compteur_id_fkey");

            entity.HasOne(d => d.StatutDemande).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.StatutDemandeId)
                .HasConstraintName("Demande_statut_demande_id_fkey");

            entity.HasOne(d => d.TypeComptage).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.TypeComptageId)
                .HasConstraintName("Demande_type_comptage_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.TypeDemandeId)
                .HasConstraintName("Demande_type_demande_id_fkey");
        });

        modelBuilder.Entity<DemandeDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DemandeDocument_pkey");

            entity.ToTable("DemandeDocument", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.IsNotifier).HasColumnName("is_notifier");
            entity.Property(e => e.NotificationSend)
                .HasDefaultValue(0)
                .HasComment("0 = notif pas encore envoyé; 1 = notif envoyé; -1 = echec d'envoie notif")
                .HasColumnName("notification_send");
            entity.Property(e => e.NumeroDocument)
                .HasMaxLength(255)
                .HasColumnName("numero_document");
            entity.Property(e => e.RefFichier)
                .HasMaxLength(255)
                .HasColumnName("ref_fichier");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDocumentId).HasColumnName("type_document_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlFichier)
                .HasMaxLength(255)
                .HasColumnName("url_fichier");

            entity.HasOne(d => d.Demande).WithMany(p => p.DemandeDocuments)
                .HasForeignKey(d => d.DemandeId)
                .HasConstraintName("DemandeDocument_demande_id_fkey");

            entity.HasOne(d => d.TypeDocument).WithMany(p => p.DemandeDocuments)
                .HasForeignKey(d => d.TypeDocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DemandeDocument_type_document_id_fkey");
        });

        modelBuilder.Entity<DemandePiecesFourny>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DemandePiecesFournies_pkey");

            entity.ToTable("DemandePiecesFournies", "sc_sge");

            entity.HasIndex(e => new { e.TypePieceId, e.DemandeId }, "DemandePiecesFournies_type_piece_id_demande_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEtablissement).HasColumnName("date_etablissement");
            entity.Property(e => e.DateExpiration).HasColumnName("date_expiration");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.EntiteEtablissement)
                .HasMaxLength(255)
                .HasColumnName("entite_etablissement");
            entity.Property(e => e.NumeroPiece)
                .HasMaxLength(255)
                .HasColumnName("numero_piece");
            entity.Property(e => e.RefFichier)
                .HasMaxLength(255)
                .HasColumnName("ref_fichier");
            entity.Property(e => e.Signataire)
                .HasMaxLength(255)
                .HasColumnName("signataire");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypePieceId).HasColumnName("type_piece_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlFichier)
                .HasMaxLength(255)
                .HasColumnName("url_fichier");

            entity.HasOne(d => d.Demande).WithMany(p => p.DemandePiecesFournies)
                .HasForeignKey(d => d.DemandeId)
                .HasConstraintName("DemandePiecesFournies_demande_id_fkey");

            entity.HasOne(d => d.TypePiece).WithMany(p => p.DemandePiecesFournies)
                .HasForeignKey(d => d.TypePieceId)
                .HasConstraintName("DemandePiecesFournies_type_piece_id_fkey");
        });

        modelBuilder.Entity<DemandeRolesPersonnesPhysique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DemandeRolesPersonnesPhysique_pkey");

            entity.ToTable("DemandeRolesPersonnesPhysique", "sc_sge");

            entity.HasIndex(e => new { e.PersonnePhysiqueLieeId, e.DemandeId }, "DemandeRolesPersonnesPhysique_personne_physique_liee_id_dem_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.PersonnePhysiqueLieeId)
                .HasMaxLength(255)
                .HasColumnName("personne_physique_liee_id");
            entity.Property(e => e.RolePersonnePhysiqueId).HasColumnName("role_personne_physique_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Demande).WithMany(p => p.DemandeRolesPersonnesPhysiques)
                .HasForeignKey(d => d.DemandeId)
                .HasConstraintName("DemandeRolesPersonnesPhysique_demande_id_fkey");

            entity.HasOne(d => d.PersonnePhysiqueLiee).WithMany(p => p.DemandeRolesPersonnesPhysiques)
                .HasForeignKey(d => d.PersonnePhysiqueLieeId)
                .HasConstraintName("DemandeRolesPersonnesPhysique_personne_physique_liee_id_fkey");

            entity.HasOne(d => d.RolePersonnePhysique).WithMany(p => p.DemandeRolesPersonnesPhysiques)
                .HasForeignKey(d => d.RolePersonnePhysiqueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DemandeRolesPersonnesPhysique_role_personne_physique_id_fkey");
        });

        modelBuilder.Entity<Denomination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Denomination_pkey");

            entity.ToTable("Denomination", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<DevisMetre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetre_pkey");

            entity.ToTable("DevisMetre", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEmission).HasColumnName("date_emission");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MetreId).HasColumnName("metre_id");
            entity.Property(e => e.MontantCaution)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant_caution");
            entity.Property(e => e.MontantTotalHt).HasColumnName("montant_total_ht");
            entity.Property(e => e.MontantTotalTtc).HasColumnName("montant_total_ttc");
            entity.Property(e => e.MontantTotalTva).HasColumnName("montant_total_tva");
            entity.Property(e => e.QuantiteTotal).HasColumnName("quantite_total");
            entity.Property(e => e.Reference)
                .HasMaxLength(255)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Metre).WithMany(p => p.DevisMetres)
                .HasForeignKey(d => d.MetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetre_metre_id_fkey");
        });

        modelBuilder.Entity<DevisMetreMateriel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetreMateriel_pkey");

            entity.ToTable("DevisMetreMateriel", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEmission).HasColumnName("date_emission");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DevisMetreId)
                .HasMaxLength(255)
                .HasColumnName("devis_metre_id");
            entity.Property(e => e.MontantTotalHt).HasColumnName("montant_total_ht");
            entity.Property(e => e.MontantTotalTtc).HasColumnName("montant_total_ttc");
            entity.Property(e => e.MontantTotalTva).HasColumnName("montant_total_tva");
            entity.Property(e => e.QuantiteTotal).HasColumnName("quantite_total");
            entity.Property(e => e.Reference)
                .HasMaxLength(255)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DevisMetre).WithMany(p => p.DevisMetreMateriels)
                .HasForeignKey(d => d.DevisMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetreMateriel_devis_metre_id_fkey");
        });

        modelBuilder.Entity<DevisMetreMaterielsDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetreMaterielsDetail_pkey");

            entity.ToTable("DevisMetreMaterielsDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeMateriel)
                .HasMaxLength(255)
                .HasColumnName("code_materiel");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DesignationMateriel)
                .HasMaxLength(255)
                .HasColumnName("designation_materiel");
            entity.Property(e => e.DevisMetreId).HasColumnName("devis_metre_id");
            entity.Property(e => e.MontantHt).HasColumnName("montant_ht");
            entity.Property(e => e.MontantTtc).HasColumnName("montant_ttc");
            entity.Property(e => e.MontantTva).HasColumnName("montant_tva");
            entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TauxTva).HasColumnName("taux_tva");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DevisMetre).WithMany(p => p.DevisMetreMaterielsDetails)
                .HasForeignKey(d => d.DevisMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetreMaterielsDetail_devis_metre_id_fkey");
        });

        modelBuilder.Entity<DevisMetreTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetreTravaux_pkey");

            entity.ToTable("DevisMetreTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEmission).HasColumnName("date_emission");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DevisMetreId)
                .HasMaxLength(255)
                .HasColumnName("devis_metre_id");
            entity.Property(e => e.MontantTotalHt).HasColumnName("montant_total_ht");
            entity.Property(e => e.MontantTotalTtc).HasColumnName("montant_total_ttc");
            entity.Property(e => e.MontantTotalTva).HasColumnName("montant_total_tva");
            entity.Property(e => e.QuantiteTotal).HasColumnName("quantite_total");
            entity.Property(e => e.Reference)
                .HasMaxLength(255)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DevisMetre).WithMany(p => p.DevisMetreTravauxes)
                .HasForeignKey(d => d.DevisMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetreTravaux_devis_metre_id_fkey");
        });

        modelBuilder.Entity<DevisMetreTravauxAdditionnel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetreTravauxAdditionnel_pkey");

            entity.ToTable("DevisMetreTravauxAdditionnel", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEmission).HasColumnName("date_emission");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DevisMetreId)
                .HasMaxLength(255)
                .HasColumnName("devis_metre_id");
            entity.Property(e => e.MontantTotalHt).HasColumnName("montant_total_ht");
            entity.Property(e => e.MontantTotalTtc).HasColumnName("montant_total_ttc");
            entity.Property(e => e.MontantTotalTva).HasColumnName("montant_total_tva");
            entity.Property(e => e.QuantiteTotal).HasColumnName("quantite_total");
            entity.Property(e => e.Reference)
                .HasMaxLength(255)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DevisMetre).WithMany(p => p.DevisMetreTravauxAdditionnels)
                .HasForeignKey(d => d.DevisMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetreTravauxAdditionnel_devis_metre_id_fkey");
        });

        modelBuilder.Entity<DevisMetreTravauxAdditionnelsDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetreTravauxAdditionnelsDetail_pkey");

            entity.ToTable("DevisMetreTravauxAdditionnelsDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeTravaux)
                .HasMaxLength(255)
                .HasColumnName("code_travaux");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DesignationTravaux)
                .HasMaxLength(255)
                .HasColumnName("designation_travaux");
            entity.Property(e => e.DevisMetreId).HasColumnName("devis_metre_id");
            entity.Property(e => e.MontantHt).HasColumnName("montant_ht");
            entity.Property(e => e.MontantTtc).HasColumnName("montant_ttc");
            entity.Property(e => e.MontantTva).HasColumnName("montant_tva");
            entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TauxTva).HasColumnName("taux_tva");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DevisMetre).WithMany(p => p.DevisMetreTravauxAdditionnelsDetails)
                .HasForeignKey(d => d.DevisMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetreTravauxAdditionnelsDetail_devis_metre_id_fkey");
        });

        modelBuilder.Entity<DevisMetreTravauxDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DevisMetreTravauxDetail_pkey");

            entity.ToTable("DevisMetreTravauxDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeTravaux)
                .HasMaxLength(255)
                .HasColumnName("code_travaux");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DesignationTravaux)
                .HasMaxLength(255)
                .HasColumnName("designation_travaux");
            entity.Property(e => e.DevisMetreId).HasColumnName("devis_metre_id");
            entity.Property(e => e.MontantHt).HasColumnName("montant_ht");
            entity.Property(e => e.MontantTtc).HasColumnName("montant_ttc");
            entity.Property(e => e.MontantTva).HasColumnName("montant_tva");
            entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TauxTva).HasColumnName("taux_tva");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DevisMetre).WithMany(p => p.DevisMetreTravauxDetails)
                .HasForeignKey(d => d.DevisMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DevisMetreTravauxDetail_devis_metre_id_fkey");
        });

        modelBuilder.Entity<DiametreCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DiametreCompteur_pkey");

            entity.ToTable("DiametreCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Reglage).HasColumnName("reglage");
            entity.Property(e => e.ReglageMax).HasColumnName("reglage_max");
            entity.Property(e => e.ReglageMin).HasColumnName("reglage_min");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.DiametreCompteurs)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("DiametreCompteur_produit_id_fkey");
        });

        modelBuilder.Entity<DiametreConduite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DiametreConduite_pkey");

            entity.ToTable("DiametreConduite", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.DiametreConduites)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("DiametreConduite_produit_id_fkey");
        });

        modelBuilder.Entity<DiametreTuyau>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DiametreTuyau_pkey");

            entity.ToTable("DiametreTuyau", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.DiametreTuyaus)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("DiametreTuyau_produit_id_fkey");
        });

        modelBuilder.Entity<EcritureComptable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EcritureComptable_pkey");

            entity.ToTable("EcritureComptable", "sc_sge");

            entity.HasIndex(e => e.NumeroEvenement, "EcritureComptable_numero_evenement_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CentreInvitationId)
                .HasMaxLength(255)
                .HasColumnName("centre_invitation_id");
            entity.Property(e => e.CentreInvitationNom)
                .HasMaxLength(255)
                .HasColumnName("centre_invitation_nom");
            entity.Property(e => e.CodeBudget)
                .HasMaxLength(255)
                .HasColumnName("code_budget");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CodeOperationLibelle)
                .HasMaxLength(255)
                .HasColumnName("code_operation_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEvenement).HasColumnName("date_evenement");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.IsExport)
                .HasDefaultValue(false)
                .HasColumnName("is_export");
            entity.Property(e => e.LibelleTopId).HasColumnName("libelle_top_id");
            entity.Property(e => e.LibelleTopLibelle)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_libelle");
            entity.Property(e => e.MontantGlobal).HasColumnName("montant_global");
            entity.Property(e => e.NumeroEvenement)
                .HasMaxLength(255)
                .HasColumnName("numero_evenement");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.TypeDemandeLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_demande_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.EcritureComptables)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EcritureComptable_code_operation_id_fkey");

            entity.HasOne(d => d.LibelleTop).WithMany(p => p.EcritureComptables)
                .HasForeignKey(d => d.LibelleTopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EcritureComptable_libelle_top_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.EcritureComptables)
                .HasForeignKey(d => d.TypeDemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EcritureComptable_type_demande_id_fkey");
        });

        modelBuilder.Entity<EcritureComptableDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EcritureComptableDetail_pkey");

            entity.ToTable("EcritureComptableDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteId).HasColumnName("compte_id");
            entity.Property(e => e.CompteLibelle)
                .HasMaxLength(255)
                .HasColumnName("compte_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EcritureId).HasColumnName("ecriture_id");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Compte).WithMany(p => p.EcritureComptableDetails)
                .HasForeignKey(d => d.CompteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EcritureComptableDetail_compte_id_fkey");

            entity.HasOne(d => d.Ecriture).WithMany(p => p.EcritureComptableDetails)
                .HasForeignKey(d => d.EcritureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EcritureComptableDetail_ecriture_id_fkey");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Entity_pkey");

            entity.ToTable("Entity");

            entity.HasIndex(e => e.Code, "entity_code_idx");

            entity.HasIndex(e => e.Email, "entity_email_idx");

            entity.HasIndex(e => e.Name, "entity_name_idx");

            entity.HasIndex(e => e.Phone, "entity_phone_idx");

            entity.HasIndex(e => e.Status, "entity_status_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EntityTypeId)
                .HasMaxLength(255)
                .HasColumnName("entity_type_id");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .HasColumnName("logo");
            entity.Property(e => e.MetaData)
                .HasMaxLength(255)
                .HasColumnName("meta_data");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.EntityType).WithMany(p => p.Entities)
                .HasForeignKey(d => d.EntityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Entity_entity_type_id_fkey");
        });

        modelBuilder.Entity<EntityParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EntityParameter_pkey");

            entity.ToTable("EntityParameter");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EntityId)
                .HasMaxLength(255)
                .HasColumnName("entity_id");
            entity.Property(e => e.Family)
                .HasMaxLength(255)
                .HasColumnName("family");
            entity.Property(e => e.IsLoadOnstart)
                .HasDefaultValue(true)
                .HasColumnName("is_load_onstart");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.ParentKey)
                .HasMaxLength(255)
                .HasColumnName("parent_key");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityParameters)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("EntityParameter_entity_id_fkey");
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EntityType_pkey");

            entity.ToTable("EntityType");

            entity.HasIndex(e => e.Code, "EntityType_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key13").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "EntityType_code_key9").IsUnique();

            entity.HasIndex(e => e.Code, "entitytype_code_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<EntityUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.EntityId }).HasName("EntityUser_pkey");

            entity.ToTable("EntityUser");

            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.EntityId)
                .HasMaxLength(255)
                .HasColumnName("entity_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.IsMain)
                .HasDefaultValue(false)
                .HasColumnName("is_main");
            entity.Property(e => e.MetaData)
                .HasMaxLength(255)
                .HasColumnName("meta_data");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityUsers)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("EntityUser_entity_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.EntityUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("EntityUser_user_id_fkey");
        });

        modelBuilder.Entity<EquipeIntervention>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EquipeIntervention_pkey");

            entity.ToTable("EquipeIntervention", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<EquipeInterventionAgent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EquipeInterventionAgent_pkey");

            entity.ToTable("EquipeInterventionAgent", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AgentId)
                .HasMaxLength(255)
                .HasColumnName("agent_id");
            entity.Property(e => e.AgentNomComplet)
                .HasMaxLength(255)
                .HasColumnName("agent_nom_complet");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutAffectation).HasColumnName("date_debut_affectation");
            entity.Property(e => e.DateFinAffectation).HasColumnName("date_fin_affectation");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EquipeId).HasColumnName("equipe_id");
            entity.Property(e => e.EquipeLibelle)
                .HasMaxLength(255)
                .HasColumnName("equipe_libelle");
            entity.Property(e => e.RoleAgentId).HasColumnName("role_agent_id");
            entity.Property(e => e.RoleAgentLibelle)
                .HasMaxLength(255)
                .HasColumnName("role_agent_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Agent).WithMany(p => p.EquipeInterventionAgents)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EquipeInterventionAgent_agent_id_fkey");

            entity.HasOne(d => d.Equipe).WithMany(p => p.EquipeInterventionAgents)
                .HasForeignKey(d => d.EquipeId)
                .HasConstraintName("EquipeInterventionAgent_equipe_id_fkey");

            entity.HasOne(d => d.RoleAgent).WithMany(p => p.EquipeInterventionAgents)
                .HasForeignKey(d => d.RoleAgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EquipeInterventionAgent_role_agent_id_fkey");
        });

        modelBuilder.Entity<EquipeInterventionVehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EquipeInterventionVehicule_pkey");

            entity.ToTable("EquipeInterventionVehicule", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutAffectation).HasColumnName("date_debut_affectation");
            entity.Property(e => e.DateFinAffectation).HasColumnName("date_fin_affectation");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EquipeId).HasColumnName("equipe_id");
            entity.Property(e => e.EquipeLibelle)
                .HasMaxLength(255)
                .HasColumnName("equipe_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VehiculeId)
                .HasMaxLength(255)
                .HasColumnName("vehicule_id");

            entity.HasOne(d => d.Equipe).WithMany(p => p.EquipeInterventionVehicules)
                .HasForeignKey(d => d.EquipeId)
                .HasConstraintName("EquipeInterventionVehicule_equipe_id_fkey");

            entity.HasOne(d => d.Vehicule).WithMany(p => p.EquipeInterventionVehicules)
                .HasForeignKey(d => d.VehiculeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EquipeInterventionVehicule_vehicule_id_fkey");
        });

        modelBuilder.Entity<Etalonnage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Etalonnage_pkey");

            entity.ToTable("Etalonnage", "sc_sge");

            entity.HasIndex(e => e.NumeroEtalonnage, "Etalonnage_numero_etalonnage_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Commentaire)
                .HasMaxLength(255)
                .HasColumnName("commentaire");
            entity.Property(e => e.CompteurId)
                .HasMaxLength(255)
                .HasColumnName("compteur_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEtalonnage).HasColumnName("date_etalonnage");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.EtatCompteurId).HasColumnName("etat_compteur_id");
            entity.Property(e => e.EtatCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("etat_compteur_libelle");
            entity.Property(e => e.NumeroAbonne)
                .HasMaxLength(255)
                .HasColumnName("numero_abonne");
            entity.Property(e => e.NumeroAbonnement)
                .HasMaxLength(255)
                .HasColumnName("numero_abonnement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroCompteur)
                .HasMaxLength(255)
                .HasColumnName("numero_compteur");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroEtalonnage)
                .HasMaxLength(255)
                .HasColumnName("numero_etalonnage");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.EtatCompteur).WithMany(p => p.Etalonnages)
                .HasForeignKey(d => d.EtatCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Etalonnage_etat_compteur_id_fkey");
        });

        modelBuilder.Entity<EtatAbonnement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EtatAbonnement_pkey");

            entity.ToTable("EtatAbonnement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<EtatBranchement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EtatBranchement_pkey");

            entity.ToTable("EtatBranchement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<EtatCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EtatCompteur_pkey");

            entity.ToTable("EtatCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Extension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Extensions_pkey");

            entity.ToTable("Extensions", "sc_sge");

            entity.HasIndex(e => e.NumeroExtension, "Extensions_numero_extension_key").IsUnique();

            entity.HasIndex(e => e.NumeroExtension, "Extensions_numero_extension_key1").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdresseComplementaire)
                .HasMaxLength(255)
                .HasColumnName("adresse_complementaire");
            entity.Property(e => e.Carre)
                .HasMaxLength(255)
                .HasColumnName("carre");
            entity.Property(e => e.CommuneId)
                .HasMaxLength(255)
                .HasColumnName("commune_id");
            entity.Property(e => e.CommuneLibelle)
                .HasMaxLength(255)
                .HasColumnName("commune_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.Etage)
                .HasMaxLength(255)
                .HasColumnName("etage");
            entity.Property(e => e.Ilot)
                .HasMaxLength(255)
                .HasColumnName("ilot");
            entity.Property(e => e.Lot)
                .HasMaxLength(255)
                .HasColumnName("lot");
            entity.Property(e => e.NumeroExtension)
                .HasMaxLength(255)
                .HasColumnName("numero_extension");
            entity.Property(e => e.Porte)
                .HasMaxLength(255)
                .HasColumnName("porte");
            entity.Property(e => e.QuartierId).HasColumnName("quartier_id");
            entity.Property(e => e.QuartierLibelle)
                .HasMaxLength(255)
                .HasColumnName("quartier_libelle");
            entity.Property(e => e.Rue)
                .HasMaxLength(255)
                .HasColumnName("rue");
            entity.Property(e => e.SecteurId).HasColumnName("secteur_id");
            entity.Property(e => e.SecteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("secteur_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VilleId)
                .HasMaxLength(255)
                .HasColumnName("ville_id");
            entity.Property(e => e.VilleLibelle)
                .HasMaxLength(255)
                .HasColumnName("ville_libelle");

            entity.HasOne(d => d.Commune).WithMany(p => p.Extensions)
                .HasForeignKey(d => d.CommuneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Extensions_commune_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.Extensions)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Extensions_demande_id_fkey");

            entity.HasOne(d => d.Quartier).WithMany(p => p.Extensions)
                .HasForeignKey(d => d.QuartierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Extensions_quartier_id_fkey");

            entity.HasOne(d => d.Secteur).WithMany(p => p.Extensions)
                .HasForeignKey(d => d.SecteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Extensions_secteur_id_fkey");

            entity.HasOne(d => d.Ville).WithMany(p => p.Extensions)
                .HasForeignKey(d => d.VilleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Extensions_ville_id_fkey");
        });

        modelBuilder.Entity<ExtensionsBeneficiaire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ExtensionsBeneficiaires_pkey");

            entity.ToTable("ExtensionsBeneficiaires", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BoitePostal)
                .HasMaxLength(255)
                .HasColumnName("boite_postal");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEtablissement).HasColumnName("date_etablissement");
            entity.Property(e => e.DateExpiration).HasColumnName("date_expiration");
            entity.Property(e => e.DateNaissance)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("date_naissance");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DenominationId).HasColumnName("denomination_id");
            entity.Property(e => e.DenominationLibelle)
                .HasMaxLength(255)
                .HasColumnName("denomination_libelle");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EntiteEtablissement)
                .HasMaxLength(255)
                .HasColumnName("entite_etablissement");
            entity.Property(e => e.ExtensionId)
                .HasMaxLength(255)
                .HasColumnName("extension_id");
            entity.Property(e => e.Fax)
                .HasMaxLength(255)
                .HasColumnName("fax");
            entity.Property(e => e.IsRepresentant)
                .HasDefaultValue(false)
                .HasColumnName("is_representant");
            entity.Property(e => e.LieuNaissance)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("lieu_naissance");
            entity.Property(e => e.NationnaliteId).HasColumnName("nationnalite_id");
            entity.Property(e => e.NationnaliteLibelle)
                .HasMaxLength(255)
                .HasColumnName("nationnalite_libelle");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("nom");
            entity.Property(e => e.NumeroAgrement)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_agrement");
            entity.Property(e => e.NumeroFiscal)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_fiscal");
            entity.Property(e => e.NumeroIdentificationUnique)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_identification_unique");
            entity.Property(e => e.NumeroPiece)
                .HasMaxLength(255)
                .HasColumnName("numero_piece");
            entity.Property(e => e.NumeroRccm)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("numero_rccm");
            entity.Property(e => e.Prenoms)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Physique")
                .HasColumnName("prenoms");
            entity.Property(e => e.RaisonSociale)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("raison_sociale");
            entity.Property(e => e.RefFichier)
                .HasMaxLength(255)
                .HasColumnName("ref_fichier");
            entity.Property(e => e.Sigle)
                .HasMaxLength(255)
                .HasComment("Obligatoire pour Personne Morale")
                .HasColumnName("sigle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Telephone)
                .HasMaxLength(255)
                .HasColumnName("telephone");
            entity.Property(e => e.TelephoneFixe)
                .HasMaxLength(255)
                .HasColumnName("telephone_fixe");
            entity.Property(e => e.TypeClientId).HasColumnName("type_client_id");
            entity.Property(e => e.TypeClientLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_client_libelle");
            entity.Property(e => e.TypePieceId).HasColumnName("type_piece_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlFichier)
                .HasMaxLength(255)
                .HasColumnName("url_fichier");
            entity.Property(e => e.Whatsapp)
                .HasMaxLength(255)
                .HasColumnName("whatsapp");

            entity.HasOne(d => d.Extension).WithMany(p => p.ExtensionsBeneficiaires)
                .HasForeignKey(d => d.ExtensionId)
                .HasConstraintName("ExtensionsBeneficiaires_extension_id_fkey");

            entity.HasOne(d => d.TypeClient).WithMany(p => p.ExtensionsBeneficiaires)
                .HasForeignKey(d => d.TypeClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExtensionsBeneficiaires_type_client_id_fkey");

            entity.HasOne(d => d.TypePiece).WithMany(p => p.ExtensionsBeneficiaires)
                .HasForeignKey(d => d.TypePieceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExtensionsBeneficiaires_type_piece_id_fkey");
        });

        modelBuilder.Entity<ExtensionsPointsPiquage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ExtensionsPointsPiquage_pkey");

            entity.ToTable("ExtensionsPointsPiquage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.ExtensionId)
                .HasMaxLength(255)
                .HasColumnName("extension_id");
            entity.Property(e => e.PointsPiquageId).HasColumnName("points_piquage_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Extension).WithMany(p => p.ExtensionsPointsPiquages)
                .HasForeignKey(d => d.ExtensionId)
                .HasConstraintName("ExtensionsPointsPiquage_extension_id_fkey");

            entity.HasOne(d => d.PointsPiquage).WithMany(p => p.ExtensionsPointsPiquages)
                .HasForeignKey(d => d.PointsPiquageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExtensionsPointsPiquage_points_piquage_id_fkey");
        });

        modelBuilder.Entity<FactureClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FactureClient_pkey");

            entity.ToTable("FactureClient", "sc_sge");

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key1").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key10").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key11").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key2").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key3").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key4").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key5").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key6").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key7").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key8").IsUnique();

            entity.HasIndex(e => e.NumeroFacture, "FactureClient_numero_facture_key9").IsUnique();

            entity.HasIndex(e => e.DateEdition, "facture_client_date_edition");

            entity.HasIndex(e => e.DateExigibilite, "facture_client_date_exigibilite");

            entity.HasIndex(e => e.DateLimite, "facture_client_date_limite");

            entity.HasIndex(e => e.DateReception, "facture_client_date_reception");

            entity.HasIndex(e => e.Exigible, "facture_client_exigible");

            entity.HasIndex(e => e.IsSolde, "facture_client_is_solde");

            entity.HasIndex(e => e.NumeroAbonnement, "facture_client_numero_abonnement");

            entity.HasIndex(e => e.NumeroClient, "facture_client_numero_client");

            entity.HasIndex(e => e.NumeroCompteur, "facture_client_numero_compteur");

            entity.HasIndex(e => e.NumeroDemande, "facture_client_numero_demande");

            entity.HasIndex(e => e.NumeroElement, "facture_client_numero_element");

            entity.HasIndex(e => e.NumeroFacture, "facture_client_numero_facture");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminId)
                .HasMaxLength(255)
                .HasColumnName("admin_id");
            entity.Property(e => e.AncienIndex).HasColumnName("ancien_index");
            entity.Property(e => e.CampagneId)
                .HasMaxLength(255)
                .HasColumnName("campagne_id");
            entity.Property(e => e.CampagneLibelle)
                .HasMaxLength(255)
                .HasColumnName("campagne_libelle");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CodeCentre)
                .HasMaxLength(255)
                .HasColumnName("code_centre");
            entity.Property(e => e.CodeOperationCode)
                .HasMaxLength(255)
                .HasColumnName("code_operation_code");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CodeOperationLibelle)
                .HasMaxLength(255)
                .HasColumnName("code_operation_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEdition).HasColumnName("date_edition");
            entity.Property(e => e.DateElement).HasColumnName("date_element");
            entity.Property(e => e.DateExigibilite).HasColumnName("date_exigibilite");
            entity.Property(e => e.DateFlag).HasColumnName("date_flag");
            entity.Property(e => e.DateLimite).HasColumnName("date_limite");
            entity.Property(e => e.DateReception).HasColumnName("date_reception");
            entity.Property(e => e.DateTransaction).HasColumnName("date_transaction");
            entity.Property(e => e.DateValeur).HasColumnName("date_valeur");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.DiametreCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("diametre_compteur_libelle");
            entity.Property(e => e.Exigible)
                .HasDefaultValue(false)
                .HasColumnName("exigible");
            entity.Property(e => e.FraisRetard)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("frais_retard");
            entity.Property(e => e.GroupementCode)
                .HasMaxLength(255)
                .HasColumnName("groupement_code");
            entity.Property(e => e.GroupementLibelle)
                .HasMaxLength(255)
                .HasColumnName("groupement_libelle");
            entity.Property(e => e.IsSolde)
                .HasDefaultValue(false)
                .HasColumnName("is_solde");
            entity.Property(e => e.IsSync)
                .HasDefaultValue(false)
                .HasColumnName("is_sync");
            entity.Property(e => e.LibelleTopCode)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_code");
            entity.Property(e => e.LibelleTopId).HasColumnName("libelle_top_id");
            entity.Property(e => e.LibelleTopLibelle)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_libelle");
            entity.Property(e => e.MatriculeAdmin)
                .HasMaxLength(255)
                .HasColumnName("matricule_admin");
            entity.Property(e => e.MoisComptable)
                .HasMaxLength(255)
                .HasColumnName("mois_comptable");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.MontantHt)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant_ht");
            entity.Property(e => e.MontantRegle)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant_regle");
            entity.Property(e => e.MontantTva)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant_tva");
            entity.Property(e => e.MotifAnnulationId).HasColumnName("motif_annulation_id");
            entity.Property(e => e.MotifAnnulationLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_annulation_libelle");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasColumnName("nom_centre");
            entity.Property(e => e.NonEncaissable)
                .HasDefaultValue(false)
                .HasColumnName("non_encaissable");
            entity.Property(e => e.NouveauIndex).HasColumnName("nouveau_index");
            entity.Property(e => e.NumeroAbonnement)
                .HasMaxLength(255)
                .HasColumnName("numero_abonnement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroCompteur)
                .HasMaxLength(255)
                .HasColumnName("numero_compteur");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroElement)
                .HasMaxLength(255)
                .HasColumnName("numero_element");
            entity.Property(e => e.NumeroFacture)
                .HasMaxLength(255)
                .HasColumnName("numero_facture");
            entity.Property(e => e.OrdreMarche).HasColumnName("ordre_marche");
            entity.Property(e => e.PeriodeId).HasColumnName("periode_id");
            entity.Property(e => e.PeriodeLibelle)
                .HasMaxLength(255)
                .HasColumnName("periode_libelle");
            entity.Property(e => e.ReferenceContrat)
                .HasMaxLength(255)
                .HasColumnName("reference_contrat");
            entity.Property(e => e.ReferencePdl)
                .HasMaxLength(255)
                .HasColumnName("reference_pdl");
            entity.Property(e => e.Solde)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("solde");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutFactureCode)
                .HasMaxLength(255)
                .HasColumnName("statut_facture_code");
            entity.Property(e => e.StatutFactureId).HasColumnName("statut_facture_id");
            entity.Property(e => e.StatutFactureLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_facture_libelle");
            entity.Property(e => e.TaxeAdeduire)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("taxe_adeduire");
            entity.Property(e => e.TypeFactureCode)
                .HasMaxLength(255)
                .HasColumnName("type_facture_code");
            entity.Property(e => e.TypeFactureId).HasColumnName("type_facture_id");
            entity.Property(e => e.TypeFactureLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_facture_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VolumeConsommation).HasColumnName("volume_consommation");

            entity.HasOne(d => d.Campagne).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.CampagneId)
                .HasConstraintName("FactureClient_campagne_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FactureClient_client_id_fkey");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FactureClient_code_operation_id_fkey");

            entity.HasOne(d => d.LibelleTop).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.LibelleTopId)
                .HasConstraintName("FactureClient_libelle_top_id_fkey");

            entity.HasOne(d => d.MotifAnnulation).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.MotifAnnulationId)
                .HasConstraintName("FactureClient_motif_annulation_id_fkey");

            entity.HasOne(d => d.Periode).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.PeriodeId)
                .HasConstraintName("FactureClient_periode_id_fkey");

            entity.HasOne(d => d.StatutFacture).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.StatutFactureId)
                .HasConstraintName("FactureClient_statut_facture_id_fkey");

            entity.HasOne(d => d.TypeFacture).WithMany(p => p.FactureClients)
                .HasForeignKey(d => d.TypeFactureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FactureClient_type_facture_id_fkey");
        });

        modelBuilder.Entity<FactureClientDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FactureClientDetail_pkey");

            entity.ToTable("FactureClientDetail", "sc_sge");

            entity.HasIndex(e => e.FactureClientId, "facture_client_detail_facture_client_id");

            entity.HasIndex(e => e.NumeroFacture, "facture_client_detail_numero_facture");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Consommation).HasColumnName("consommation");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FactureClientId)
                .HasMaxLength(255)
                .HasColumnName("facture_client_id");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.NumeroFacture)
                .HasMaxLength(255)
                .HasColumnName("numero_facture");
            entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TotalHt).HasColumnName("total_ht");
            entity.Property(e => e.TotalTtc).HasColumnName("total_ttc");
            entity.Property(e => e.TotalTva).HasColumnName("total_tva");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Fonction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Fonction_pkey");

            entity.ToTable("Fonction", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Form>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Form_pkey");

            entity.ToTable("Form");

            entity.HasIndex(e => e.Code, "Form_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "Form_code_key9").IsUnique();

            entity.HasIndex(e => e.Family, "form_family_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Family)
                .HasMaxLength(255)
                .HasColumnName("family");
            entity.Property(e => e.IsLocked)
                .HasDefaultValue(false)
                .HasColumnName("is_locked");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.LockedBy)
                .HasMaxLength(255)
                .HasColumnName("locked_by");
            entity.Property(e => e.Schema)
                .HasColumnType("json")
                .HasColumnName("schema");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<FraisDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FraisDemande_pkey");

            entity.ToTable("FraisDemande", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeOperationCode)
                .HasMaxLength(255)
                .HasColumnName("code_operation_code");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CodeOperationLibelle)
                .HasMaxLength(255)
                .HasColumnName("code_operation_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateReglement).HasColumnName("date_reglement");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.LibelleTopCode)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_code");
            entity.Property(e => e.LibelleTopId).HasColumnName("libelle_top_id");
            entity.Property(e => e.LibelleTopLibelle)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_libelle");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeFraisId).HasColumnName("type_frais_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.FraisDemandes)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FraisDemande_code_operation_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.FraisDemandes)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FraisDemande_demande_id_fkey");

            entity.HasOne(d => d.LibelleTop).WithMany(p => p.FraisDemandes)
                .HasForeignKey(d => d.LibelleTopId)
                .HasConstraintName("FraisDemande_libelle_top_id_fkey");

            entity.HasOne(d => d.TypeFrais).WithMany(p => p.FraisDemandes)
                .HasForeignKey(d => d.TypeFraisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FraisDemande_type_frais_id_fkey");
        });

        modelBuilder.Entity<Fraude>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Fraude_pkey");

            entity.ToTable("Fraude", "sc_sge");

            entity.HasIndex(e => e.NumeroFraude, "Fraude_numero_fraude_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateConstat).HasColumnName("date_constat");
            entity.Property(e => e.DateConvocation).HasColumnName("date_convocation");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.HeureConvocation).HasColumnName("heure_convocation");
            entity.Property(e => e.NumeroAbonne)
                .HasMaxLength(255)
                .HasColumnName("numero_abonne");
            entity.Property(e => e.NumeroAbonnement)
                .HasMaxLength(255)
                .HasColumnName("numero_abonnement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroFraude)
                .HasMaxLength(255)
                .HasColumnName("numero_fraude");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<FraudeEnqueteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FraudeEnqueteur_pkey");

            entity.ToTable("FraudeEnqueteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FraudeId)
                .HasMaxLength(255)
                .HasColumnName("fraude_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserCentre)
                .HasMaxLength(255)
                .HasColumnName("user_centre");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.UserMatricule)
                .HasMaxLength(255)
                .HasColumnName("user_matricule");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
            entity.Property(e => e.UserSite)
                .HasMaxLength(255)
                .HasColumnName("user_site");

            entity.HasOne(d => d.Fraude).WithMany(p => p.FraudeEnqueteurs)
                .HasForeignKey(d => d.FraudeId)
                .HasConstraintName("FraudeEnqueteur_fraude_id_fkey");
        });

        modelBuilder.Entity<FraudeTypeUtilise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FraudeTypeUtilise_pkey");

            entity.ToTable("FraudeTypeUtilise", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FraudeId)
                .HasMaxLength(255)
                .HasColumnName("fraude_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeFraudeId).HasColumnName("type_fraude_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Fraude).WithMany(p => p.FraudeTypeUtilises)
                .HasForeignKey(d => d.FraudeId)
                .HasConstraintName("FraudeTypeUtilise_fraude_id_fkey");

            entity.HasOne(d => d.TypeFraude).WithMany(p => p.FraudeTypeUtilises)
                .HasForeignKey(d => d.TypeFraudeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FraudeTypeUtilise_type_fraude_id_fkey");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Group_pkey");

            entity.ToTable("Group");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EntityId)
                .HasMaxLength(255)
                .HasColumnName("entity_id");
            entity.Property(e => e.GroupTypeId)
                .HasMaxLength(255)
                .HasColumnName("group_type_id");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Entity).WithMany(p => p.Groups)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Group_entity_id_fkey");

            entity.HasOne(d => d.GroupType).WithMany(p => p.Groups)
                .HasForeignKey(d => d.GroupTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Group_group_type_id_fkey");
        });

        modelBuilder.Entity<GroupType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GroupType_pkey");

            entity.ToTable("GroupType");

            entity.HasIndex(e => e.Code, "GroupType_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key13").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key14").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key15").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "GroupType_code_key9").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<GroupeFacturation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GroupeFacturation_pkey");

            entity.ToTable("GroupeFacturation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Frequence)
                .HasMaxLength(255)
                .HasColumnName("frequence");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Mois)
                .HasMaxLength(255)
                .HasColumnName("mois");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Gxdlmsactionschedule>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsactionscheduleid).HasName("gxdlmsactionschedule_pkey");

            entity.ToTable("gxdlmsactionschedule", "sc_sge");

            entity.Property(e => e.Gxdlmsactionscheduleid).HasColumnName("gxdlmsactionscheduleid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Actiontype).HasColumnName("actiontype");
            entity.Property(e => e.ExecutedscriptLn)
                .HasMaxLength(50)
                .HasColumnName("executedscript_ln");
            entity.Property(e => e.Executedscriptselector).HasColumnName("executedscriptselector");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Objecttype).HasColumnName("objecttype");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsactionschedules)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsactionschedule_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsactionscheduleexecutiontime>(entity =>
        {
            entity.HasKey(e => e.Executiontimeid).HasName("gxdlmsactionscheduleexecutiontime_pkey");

            entity.ToTable("gxdlmsactionscheduleexecutiontime", "sc_sge");

            entity.Property(e => e.Executiontimeid).HasColumnName("executiontimeid");
            entity.Property(e => e.Gxdlmsactionscheduleid).HasColumnName("gxdlmsactionscheduleid");
            entity.Property(e => e.Time)
                .HasMaxLength(50)
                .HasColumnName("time");

            entity.HasOne(d => d.Gxdlmsactionschedule).WithMany(p => p.Gxdlmsactionscheduleexecutiontimes)
                .HasForeignKey(d => d.Gxdlmsactionscheduleid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmsactionscheduleexecutiontime_gxdlmsactionscheduleid_fkey");
        });

        modelBuilder.Entity<Gxdlmsactivitycalendar>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsactivitycalendarid).HasName("gxdlmsactivitycalendar_pkey");

            entity.ToTable("gxdlmsactivitycalendar", "sc_sge");

            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Calendarnameactive)
                .HasMaxLength(50)
                .HasColumnName("calendarnameactive");
            entity.Property(e => e.Calendarnamepassive)
                .HasMaxLength(50)
                .HasColumnName("calendarnamepassive");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Time)
                .HasMaxLength(50)
                .HasColumnName("time");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsactivitycalendars)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsactivitycalendar_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsassociationlogicalname>(entity =>
        {
            entity.HasKey(e => e.Associationid).HasName("gxdlmsassociationlogicalname_pkey");

            entity.ToTable("gxdlmsassociationlogicalname", "sc_sge");

            entity.Property(e => e.Associationid).HasColumnName("associationid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Associationstatus).HasColumnName("associationstatus");
            entity.Property(e => e.Clientsap).HasColumnName("clientsap");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Multipleassociationviews).HasColumnName("multipleassociationviews");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Secret).HasColumnName("secret");
            entity.Property(e => e.Securitysetupreference)
                .HasMaxLength(50)
                .HasColumnName("securitysetupreference");
            entity.Property(e => e.Serversap).HasColumnName("serversap");
            entity.Property(e => e.Users)
                .HasMaxLength(50)
                .HasColumnName("users");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsassociationlogicalnames)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("fk_object_assoc");
        });

        modelBuilder.Entity<Gxdlmsassociationobjectlist>(entity =>
        {
            entity.HasKey(e => e.Objectid).HasName("gxdlmsassociationobjectlist_pkey");

            entity.ToTable("gxdlmsassociationobjectlist", "sc_sge");

            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Associationid).HasColumnName("associationid");
            entity.Property(e => e.Gxdlmsdataid).HasColumnName("gxdlmsdataid");

            entity.HasOne(d => d.Association).WithMany(p => p.Gxdlmsassociationobjectlists)
                .HasForeignKey(d => d.Associationid)
                .HasConstraintName("fk_association_objlist");

            entity.HasOne(d => d.Gxdlmsdata).WithMany(p => p.Gxdlmsassociationobjectlists)
                .HasForeignKey(d => d.Gxdlmsdataid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_gxdlmsdata_assoc");
        });

        modelBuilder.Entity<Gxdlmsautoconnect>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsautoconnectid).HasName("gxdlmsautoconnect_pkey");

            entity.ToTable("gxdlmsautoconnect", "sc_sge");

            entity.Property(e => e.Gxdlmsautoconnectid).HasColumnName("gxdlmsautoconnectid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Callingwindow)
                .HasMaxLength(50)
                .HasColumnName("callingwindow");
            entity.Property(e => e.Destinations)
                .HasMaxLength(50)
                .HasColumnName("destinations");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Mode).HasColumnName("mode");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Repetitiondelay).HasColumnName("repetitiondelay");
            entity.Property(e => e.Repetitions).HasColumnName("repetitions");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsautoconnects)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsautoconnect_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsclock>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsclockid).HasName("gxdlmsclock_pkey");

            entity.ToTable("gxdlmsclock", "sc_sge");

            entity.Property(e => e.Gxdlmsclockid).HasColumnName("gxdlmsclockid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Begin).HasMaxLength(50);
            entity.Property(e => e.Clockbase).HasColumnName("clockbase");
            entity.Property(e => e.Deviation).HasColumnName("deviation");
            entity.Property(e => e.Enabled).HasColumnName("enabled");
            entity.Property(e => e.End).HasMaxLength(50);
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Time).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Timezone).HasColumnName("timezone");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsclocks)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsclock_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsdatum>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsdataid).HasName("gxdlmsdata_pkey");

            entity.ToTable("gxdlmsdata", "sc_sge");

            entity.Property(e => e.Gxdlmsdataid).HasColumnName("gxdlmsdataid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.NumeroCompteur)
                .HasColumnType("character varying")
                .HasColumnName("numero_compteur");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.ValueContent).HasColumnName("value_content");
            entity.Property(e => e.ValueType).HasColumnName("value_type");
            entity.Property(e => e.ValueUitype).HasColumnName("value_uitype");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsdata)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("fk_object_gxdlmsdata");
        });

        modelBuilder.Entity<Gxdlmsdemandregister>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsdemandregisterid).HasName("gxdlmsdemandregister_pkey");

            entity.ToTable("gxdlmsdemandregister", "sc_sge");

            entity.Property(e => e.Gxdlmsdemandregisterid).HasColumnName("gxdlmsdemandregisterid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Currentaveragevaluecontent).HasColumnName("currentaveragevaluecontent");
            entity.Property(e => e.Currentaveragevaluetype).HasColumnName("currentaveragevaluetype");
            entity.Property(e => e.Currentaveragevalueuitype).HasColumnName("currentaveragevalueuitype");
            entity.Property(e => e.LastaveragevalueContent).HasColumnName("lastaveragevalue_content");
            entity.Property(e => e.Lastaveragevaluetype).HasColumnName("lastaveragevaluetype");
            entity.Property(e => e.Lastaveragevalueuitype).HasColumnName("lastaveragevalueuitype");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Numberofperiods).HasColumnName("numberofperiods");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Scaler).HasColumnName("scaler");
            entity.Property(e => e.Starttimecurrent)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttimecurrent");
            entity.Property(e => e.Statustype).HasColumnName("statustype");
            entity.Property(e => e.Unit).HasColumnName("unit");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsdemandregisters)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsdemandregister_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsdisconnectcontrol>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsdisconnectcontrolid).HasName("gxdlmsdisconnectcontrol_pkey");

            entity.ToTable("gxdlmsdisconnectcontrol", "sc_sge");

            entity.Property(e => e.Gxdlmsdisconnectcontrolid).HasColumnName("gxdlmsdisconnectcontrolid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Controlmode).HasColumnName("controlmode");
            entity.Property(e => e.Controlstate).HasColumnName("controlstate");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Outputstate).HasColumnName("outputstate");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsdisconnectcontrols)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsdisconnectcontrol_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsextendedregister>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsextendregisterid).HasName("gxdlmsextendedregister_pkey");

            entity.ToTable("gxdlmsextendedregister", "sc_sge");

            entity.Property(e => e.Gxdlmsextendregisterid).HasColumnName("gxdlmsextendregisterid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Scaler).HasColumnName("scaler");
            entity.Property(e => e.Statustype).HasColumnName("statustype");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.ValueContent).HasColumnName("value_content");
            entity.Property(e => e.ValueType).HasColumnName("value_type");
            entity.Property(e => e.ValueUitype).HasColumnName("value_uitype");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsextendedregisters)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsextendedregister_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsgprssetup>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsgprssetupid).HasName("gxdlmsgprssetup_pkey");

            entity.ToTable("gxdlmsgprssetup", "sc_sge");

            entity.Property(e => e.Gxdlmsgprssetupid).HasColumnName("gxdlmsgprssetupid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Apn)
                .HasMaxLength(100)
                .HasColumnName("apn");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Pincode)
                .HasMaxLength(50)
                .HasColumnName("pincode");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsgprssetups)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsgprssetup_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsgprssetupdefaultqo>(entity =>
        {
            entity.HasKey(e => e.Qosid).HasName("gxdlmsgprssetupdefaultqos_pkey");

            entity.ToTable("gxdlmsgprssetupdefaultqos", "sc_sge");

            entity.Property(e => e.Qosid).HasColumnName("qosid");
            entity.Property(e => e.Delay).HasColumnName("delay");
            entity.Property(e => e.Gxdlmsgprssetupid).HasColumnName("gxdlmsgprssetupid");
            entity.Property(e => e.Meanthroughput).HasColumnName("meanthroughput");
            entity.Property(e => e.Peakthroughput).HasColumnName("peakthroughput");
            entity.Property(e => e.Precedence).HasColumnName("precedence");
            entity.Property(e => e.Reliability).HasColumnName("reliability");

            entity.HasOne(d => d.Gxdlmsgprssetup).WithMany(p => p.Gxdlmsgprssetupdefaultqos)
                .HasForeignKey(d => d.Gxdlmsgprssetupid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmsgprssetupdefaultqos_gxdlmsgprssetupid_fkey");
        });

        modelBuilder.Entity<Gxdlmsgprssetuprequestedqo>(entity =>
        {
            entity.HasKey(e => e.Qosid).HasName("gxdlmsgprssetuprequestedqos_pkey");

            entity.ToTable("gxdlmsgprssetuprequestedqos", "sc_sge");

            entity.Property(e => e.Qosid).HasColumnName("qosid");
            entity.Property(e => e.Delay).HasColumnName("delay");
            entity.Property(e => e.Gxdlmsgprssetupid).HasColumnName("gxdlmsgprssetupid");
            entity.Property(e => e.Meanthroughput).HasColumnName("meanthroughput");
            entity.Property(e => e.Peakthroughput).HasColumnName("peakthroughput");
            entity.Property(e => e.Precedence).HasColumnName("precedence");
            entity.Property(e => e.Reliability).HasColumnName("reliability");

            entity.HasOne(d => d.Gxdlmsgprssetup).WithMany(p => p.Gxdlmsgprssetuprequestedqos)
                .HasForeignKey(d => d.Gxdlmsgprssetupid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmsgprssetuprequestedqos_gxdlmsgprssetupid_fkey");
        });

        modelBuilder.Entity<Gxdlmsgsmdiagnostic>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsgsmdiagnosticid).HasName("gxdlmsgsmdiagnostic_pkey");

            entity.ToTable("gxdlmsgsmdiagnostic", "sc_sge");

            entity.Property(e => e.Gxdlmsgsmdiagnosticid).HasColumnName("gxdlmsgsmdiagnosticid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Circuitswitchstatus).HasColumnName("circuitswitchstatus");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Operator)
                .HasMaxLength(50)
                .HasColumnName("operator");
            entity.Property(e => e.Packetswitchstatus).HasColumnName("packetswitchstatus");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsgsmdiagnostics)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsgsmdiagnostic_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsgsmdiagnosticadjacentcell>(entity =>
        {
            entity.HasKey(e => e.Adjacentcellid).HasName("gxdlmsgsmdiagnosticadjacentcells_pkey");

            entity.ToTable("gxdlmsgsmdiagnosticadjacentcells", "sc_sge");

            entity.Property(e => e.Adjacentcellid).HasColumnName("adjacentcellid");
            entity.Property(e => e.Ber).HasColumnName("ber");
            entity.Property(e => e.Cellid).HasColumnName("cellid");
            entity.Property(e => e.Gxdlmsgsmdiagnosticid).HasColumnName("gxdlmsgsmdiagnosticid");
            entity.Property(e => e.Signalquality).HasColumnName("signalquality");

            entity.HasOne(d => d.Gxdlmsgsmdiagnostic).WithMany(p => p.Gxdlmsgsmdiagnosticadjacentcells)
                .HasForeignKey(d => d.Gxdlmsgsmdiagnosticid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmsgsmdiagnosticadjacentcells_gxdlmsgsmdiagnosticid_fkey");
        });

        modelBuilder.Entity<Gxdlmsgsmdiagnosticcellinfo>(entity =>
        {
            entity.HasKey(e => e.Cellinfoid).HasName("gxdlmsgsmdiagnosticcellinfo_pkey");

            entity.ToTable("gxdlmsgsmdiagnosticcellinfo", "sc_sge");

            entity.Property(e => e.Cellinfoid).HasColumnName("cellinfoid");
            entity.Property(e => e.Ber).HasColumnName("ber");
            entity.Property(e => e.Cellid).HasColumnName("cellid");
            entity.Property(e => e.Gxdlmsgsmdiagnosticid).HasColumnName("gxdlmsgsmdiagnosticid");
            entity.Property(e => e.Locationid).HasColumnName("locationid");
            entity.Property(e => e.Signalquality).HasColumnName("signalquality");

            entity.HasOne(d => d.Gxdlmsgsmdiagnostic).WithMany(p => p.Gxdlmsgsmdiagnosticcellinfos)
                .HasForeignKey(d => d.Gxdlmsgsmdiagnosticid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmsgsmdiagnosticcellinfo_gxdlmsgsmdiagnosticid_fkey");
        });

        modelBuilder.Entity<Gxdlmsiechdlcsetup>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsiechdlcsetupid).HasName("gxdlmsiechdlcsetup_pkey");

            entity.ToTable("gxdlmsiechdlcsetup", "sc_sge");

            entity.Property(e => e.Gxdlmsiechdlcsetupid).HasColumnName("gxdlmsiechdlcsetupid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Deviceaddress)
                .HasMaxLength(50)
                .HasColumnName("deviceaddress");
            entity.Property(e => e.Inactivitytimeout).HasColumnName("inactivitytimeout");
            entity.Property(e => e.Intercharactertimeout).HasColumnName("intercharactertimeout");
            entity.Property(e => e.Maxinfolengthrx).HasColumnName("maxinfolengthrx");
            entity.Property(e => e.Maxinfolengthtx).HasColumnName("maxinfolengthtx");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.Version).HasColumnName("version");
            entity.Property(e => e.Windowsizerx).HasColumnName("windowsizerx");
            entity.Property(e => e.Windowsizetx).HasColumnName("windowsizetx");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsiechdlcsetups)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsiechdlcsetup_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsieclocalportsetup>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsieclocalportsetupid).HasName("gxdlmsieclocalportsetup_pkey");

            entity.ToTable("gxdlmsieclocalportsetup", "sc_sge");

            entity.Property(e => e.Gxdlmsieclocalportsetupid).HasColumnName("gxdlmsieclocalportsetupid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Defaultbaudrate).HasColumnName("defaultbaudrate");
            entity.Property(e => e.Defaultmode).HasColumnName("defaultmode");
            entity.Property(e => e.Deviceaddress)
                .HasMaxLength(50)
                .HasColumnName("deviceaddress");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Password1)
                .HasMaxLength(50)
                .HasColumnName("password1");
            entity.Property(e => e.Password2)
                .HasMaxLength(50)
                .HasColumnName("password2");
            entity.Property(e => e.Password5)
                .HasMaxLength(50)
                .HasColumnName("password5");
            entity.Property(e => e.Proposedbaudrate).HasColumnName("proposedbaudrate");
            entity.Property(e => e.Responsetime).HasColumnName("responsetime");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsieclocalportsetups)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsieclocalportsetup_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmslimiter>(entity =>
        {
            entity.HasKey(e => e.Gxdlmslimiterid).HasName("gxdlmslimiter_pkey");

            entity.ToTable("gxdlmslimiter", "sc_sge");

            entity.Property(e => e.Gxdlmslimiterid).HasColumnName("gxdlmslimiterid");
            entity.Property(e => e.Access)
                .HasMaxLength(8)
                .HasColumnName("access");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(8)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Minoverthresholdduration).HasColumnName("minoverthresholdduration");
            entity.Property(e => e.Minunderthresholdduration).HasColumnName("minunderthresholdduration");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Thresholdactive).HasColumnName("thresholdactive");
            entity.Property(e => e.Thresholdemergency).HasColumnName("thresholdemergency");
            entity.Property(e => e.Thresholdnormal).HasColumnName("thresholdnormal");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmslimiters)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmslimiter_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmslimiteremergencyprofile>(entity =>
        {
            entity.HasKey(e => e.Emergencyprofileid).HasName("gxdlmslimiteremergencyprofile_pkey");

            entity.ToTable("gxdlmslimiteremergencyprofile", "sc_sge");

            entity.Property(e => e.Emergencyprofileid).HasColumnName("emergencyprofileid");
            entity.Property(e => e.Capturetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("capturetime");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Gxdlmslimiterid).HasColumnName("gxdlmslimiterid");
            entity.Property(e => e.Profileid).HasColumnName("profileid");
            entity.Property(e => e.Starttime)
                .HasMaxLength(50)
                .HasColumnName("starttime");

            entity.HasOne(d => d.Gxdlmslimiter).WithMany(p => p.Gxdlmslimiteremergencyprofiles)
                .HasForeignKey(d => d.Gxdlmslimiterid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmslimiteremergencyprofile_gxdlmslimiterid_fkey");
        });

        modelBuilder.Entity<Gxdlmslimitermonitoredvalue>(entity =>
        {
            entity.HasKey(e => e.Monitoredvalueid).HasName("gxdlmslimitermonitoredvalue_pkey");

            entity.ToTable("gxdlmslimitermonitoredvalue", "sc_sge");

            entity.Property(e => e.Monitoredvalueid).HasColumnName("monitoredvalueid");
            entity.Property(e => e.Gxdlmslimiterid).HasColumnName("gxdlmslimiterid");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Logicalname)
                .HasMaxLength(50)
                .HasColumnName("logicalname");
            entity.Property(e => e.Objecttype).HasColumnName("objecttype");

            entity.HasOne(d => d.Gxdlmslimiter).WithMany(p => p.Gxdlmslimitermonitoredvalues)
                .HasForeignKey(d => d.Gxdlmslimiterid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gxdlmslimitermonitoredvalue_gxdlmslimiterid_fkey");
        });

        modelBuilder.Entity<Gxdlmsprofilgeneric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gxdlmsprofilgeneric_pkey");

            entity.ToTable("gxdlmsprofilgeneric", "sc_sge");

            entity.HasIndex(e => e.Origine, "gxdlmsprofilgeneric__origine");

            entity.HasIndex(e => e.Codeobisid, "gxdlmsprofilgeneric_codeobisid");

            entity.HasIndex(e => e.Compteurid, "gxdlmsprofilgeneric_compteurid");

            entity.HasIndex(e => e.DateCreated, "gxdlmsprofilgeneric_date_created");

            entity.HasIndex(e => e.Type, "gxdlmsprofilgeneric_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codeobisid).HasColumnName("codeobisid");
            entity.Property(e => e.Compteurid)
                .HasMaxLength(255)
                .HasColumnName("compteurid");
            entity.Property(e => e.DateCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_created");
            entity.Property(e => e.Origine).HasMaxLength(255);
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.Codeobis).WithMany(p => p.Gxdlmsprofilgenerics)
                .HasForeignKey(d => d.Codeobisid)
                .HasConstraintName("gxdlmsprofilgeneric_codeobisid_fkey");

            entity.HasOne(d => d.Compteur).WithMany(p => p.Gxdlmsprofilgenerics)
                .HasForeignKey(d => d.Compteurid)
                .HasConstraintName("gxdlmsprofilgeneric_compteurid_fkey");
        });

        modelBuilder.Entity<Gxdlmsprofilgenericdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gxdlmsprofilgenericdetails_pkey");

            entity.ToTable("gxdlmsprofilgenericdetails", "sc_sge");

            entity.HasIndex(e => e.Codeobisid, "gxdlmsprofilgenericdetails_codeobisid");

            entity.HasIndex(e => e.Profilgenericid, "gxdlmsprofilgenericdetails_profilgenericid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codeobisid).HasColumnName("codeobisid");
            entity.Property(e => e.Profilgenericid).HasColumnName("profilgenericid");
            entity.Property(e => e.Unit)
                .HasMaxLength(255)
                .HasColumnName("unit");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");

            entity.HasOne(d => d.Codeobis).WithMany(p => p.Gxdlmsprofilgenericdetails)
                .HasForeignKey(d => d.Codeobisid)
                .HasConstraintName("gxdlmsprofilgenericdetails_codeobisid_fkey");
        });

        modelBuilder.Entity<Gxdlmsprofilgenericdetailsevent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gxdlmsprofilgenericdetailsevent_pkey");

            entity.ToTable("gxdlmsprofilgenericdetailsevent", "sc_sge");

            entity.HasIndex(e => e.Category, "gxdlmsprofilgenericdetailsevent_category");

            entity.HasIndex(e => e.Codeobisid, "gxdlmsprofilgenericdetailsevent_codeobisid");

            entity.HasIndex(e => e.Profilgenericid, "gxdlmsprofilgenericdetailsevent_profilgenericid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Codeobisid).HasColumnName("codeobisid");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Profilgenericid).HasColumnName("profilgenericid");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");

            entity.HasOne(d => d.Codeobis).WithMany(p => p.Gxdlmsprofilgenericdetailsevents)
                .HasForeignKey(d => d.Codeobisid)
                .HasConstraintName("gxdlmsprofilgenericdetailsevent_codeobisid_fkey");
        });

        modelBuilder.Entity<Gxdlmsregister>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsregisterid).HasName("gxdlmsregister_pkey");

            entity.ToTable("gxdlmsregister", "sc_sge");

            entity.Property(e => e.Gxdlmsregisterid).HasColumnName("gxdlmsregisterid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Scaler).HasColumnName("scaler");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.ValueContent).HasColumnName("value_content");
            entity.Property(e => e.ValueType).HasColumnName("value_type");
            entity.Property(e => e.ValueUitype).HasColumnName("value_uitype");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsregisters)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsregister_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmssapassignment>(entity =>
        {
            entity.HasKey(e => e.Gxdlmssapassignmentid).HasName("gxdlmssapassignment_pkey");

            entity.ToTable("gxdlmssapassignment", "sc_sge");

            entity.Property(e => e.Gxdlmssapassignmentid).HasColumnName("gxdlmssapassignmentid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Sapassignmentlist).HasColumnName("sapassignmentlist");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmssapassignments)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmssapassignment_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsscripttable>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsscripttableid).HasName("gxdlmsscripttable_pkey");

            entity.ToTable("gxdlmsscripttable", "sc_sge");

            entity.Property(e => e.Gxdlmsscripttableid).HasColumnName("gxdlmsscripttableid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsscripttables)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsscripttable_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmssecuritysetup>(entity =>
        {
            entity.HasKey(e => e.Gxdlmssecuritysetupid).HasName("gxdlmssecuritysetup_pkey");

            entity.ToTable("gxdlmssecuritysetup", "sc_sge");

            entity.Property(e => e.Gxdlmssecuritysetupid).HasColumnName("gxdlmssecuritysetupid");
            entity.Property(e => e.Certificatedata).HasColumnName("certificatedata");
            entity.Property(e => e.Clientsystemtitle)
                .HasMaxLength(255)
                .HasColumnName("clientsystemtitle");
            entity.Property(e => e.Gak).HasColumnName("gak");
            entity.Property(e => e.Gbek).HasColumnName("gbek");
            entity.Property(e => e.Guek).HasColumnName("guek");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Securitypolicy).HasColumnName("securitypolicy");
            entity.Property(e => e.Securitysuite).HasColumnName("securitysuite");
            entity.Property(e => e.Serversystemtitle)
                .HasMaxLength(255)
                .HasColumnName("serversystemtitle");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmssecuritysetups)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmssecuritysetup_objectid_fkey");
        });

        modelBuilder.Entity<Gxdlmsspecialdaystable>(entity =>
        {
            entity.HasKey(e => e.Gxdlmsspecialdaystableid).HasName("gxdlmsspecialdaystable_pkey");

            entity.ToTable("gxdlmsspecialdaystable", "sc_sge");

            entity.Property(e => e.Gxdlmsspecialdaystableid).HasColumnName("gxdlmsspecialdaystableid");
            entity.Property(e => e.Access)
                .HasMaxLength(50)
                .HasColumnName("access");
            entity.Property(e => e.Methodaccess)
                .HasMaxLength(50)
                .HasColumnName("methodaccess");
            entity.Property(e => e.Objectid).HasColumnName("objectid");

            entity.HasOne(d => d.Object).WithMany(p => p.Gxdlmsspecialdaystables)
                .HasForeignKey(d => d.Objectid)
                .HasConstraintName("gxdlmsspecialdaystable_objectid_fkey");
        });

        modelBuilder.Entity<LiaisonCompteurTransfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LiaisonCompteurTransfo_pkey");

            entity.ToTable("LiaisonCompteurTransfo", "sc_sge");

            entity.HasIndex(e => e.AgentId, "liaison_compteur_transfo_agent_id");

            entity.HasIndex(e => e.AgentMatricule, "liaison_compteur_transfo_agent_matricule");

            entity.HasIndex(e => e.CompteurId, "liaison_compteur_transfo_compteur_id");

            entity.HasIndex(e => e.NumeroCompteur, "liaison_compteur_transfo_numero_compteur");

            entity.HasIndex(e => e.PosteTransfoCode, "liaison_compteur_transfo_poste_transfo_code");

            entity.HasIndex(e => e.PosteTransfoId, "liaison_compteur_transfo_poste_transfo_id");

            entity.HasIndex(e => e.Status, "liaison_compteur_transfo_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AgentId)
                .HasMaxLength(255)
                .HasColumnName("agent_id");
            entity.Property(e => e.AgentMatricule)
                .HasMaxLength(255)
                .HasColumnName("agent_matricule");
            entity.Property(e => e.CompteurId)
                .HasMaxLength(255)
                .HasColumnName("compteur_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.NumeroCompteur)
                .HasMaxLength(255)
                .HasColumnName("numero_compteur");
            entity.Property(e => e.PosteTransfoCode)
                .HasMaxLength(255)
                .HasColumnName("poste_transfo_code");
            entity.Property(e => e.PosteTransfoId).HasColumnName("poste_transfo_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Agent).WithMany(p => p.LiaisonCompteurTransfos)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LiaisonCompteurTransfo_agent_id_fkey");

            entity.HasOne(d => d.Compteur).WithMany(p => p.LiaisonCompteurTransfos)
                .HasForeignKey(d => d.CompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LiaisonCompteurTransfo_compteur_id_fkey");

            entity.HasOne(d => d.PosteTransfo).WithMany(p => p.LiaisonCompteurTransfos)
                .HasForeignKey(d => d.PosteTransfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LiaisonCompteurTransfo_poste_transfo_id_fkey");
        });

        modelBuilder.Entity<LibelleTop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LibelleTop_pkey");

            entity.ToTable("LibelleTop", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<LotRi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LotRi_pkey");

            entity.ToTable("LotRi", "sc_sge");

            entity.HasIndex(e => e.NumeroLotri, "LotRi_numero_lotri_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminId)
                .HasMaxLength(255)
                .HasColumnName("admin_id");
            entity.Property(e => e.AdminMatricule)
                .HasMaxLength(255)
                .HasColumnName("admin_matricule");
            entity.Property(e => e.AdminNom)
                .HasMaxLength(255)
                .HasColumnName("admin_nom");
            entity.Property(e => e.Base)
                .HasMaxLength(255)
                .HasColumnName("base");
            entity.Property(e => e.CampagneCode)
                .HasMaxLength(255)
                .HasColumnName("campagne_code");
            entity.Property(e => e.CampagneId)
                .HasMaxLength(255)
                .HasColumnName("campagne_id");
            entity.Property(e => e.CampagneLibelle)
                .HasMaxLength(255)
                .HasColumnName("campagne_libelle");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CentreNom)
                .HasMaxLength(255)
                .HasColumnName("centre_nom");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateFacturation).HasColumnName("date_facturation");
            entity.Property(e => e.DateSaisie).HasColumnName("date_saisie");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Exigibilite).HasColumnName("exigibilite");
            entity.Property(e => e.Jet)
                .HasMaxLength(255)
                .HasColumnName("jet");
            entity.Property(e => e.NombreAbonne).HasColumnName("nombre_abonne");
            entity.Property(e => e.NumeroLotri)
                .HasMaxLength(255)
                .HasColumnName("numero_lotri");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TourneeId).HasColumnName("tournee_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Campagne).WithMany(p => p.LotRis)
                .HasForeignKey(d => d.CampagneId)
                .HasConstraintName("LotRi_campagne_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.LotRis)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LotRi_produit_id_fkey");

            entity.HasOne(d => d.Tournee).WithMany(p => p.LotRis)
                .HasForeignKey(d => d.TourneeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LotRi_tournee_id_fkey");
        });

        modelBuilder.Entity<Magasin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Magasin_pkey");

            entity.ToTable("Magasin", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.NomCentre)
                .HasMaxLength(255)
                .HasColumnName("nom_centre");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeMagasinId).HasColumnName("type_magasin_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeMagasin).WithMany(p => p.Magasins)
                .HasForeignKey(d => d.TypeMagasinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Magasin_type_magasin_id_fkey");
        });

        modelBuilder.Entity<MagasinStock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MagasinStock_pkey");

            entity.ToTable("MagasinStock", "sc_sge");

            entity.HasIndex(e => e.MagasinId, "magasin_stock_magasin_id");

            entity.HasIndex(e => e.MaterielDevisId, "magasin_stock_materiel_devis_id");

            entity.HasIndex(e => new { e.MaterielDevisId, e.MagasinId }, "magasin_stock_materiel_devis_id_magasin_id").IsUnique();

            entity.HasIndex(e => e.Status, "magasin_stock_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateValue).HasColumnName("date_value");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MagasinId)
                .HasMaxLength(255)
                .HasColumnName("magasin_id");
            entity.Property(e => e.MagasinNom)
                .HasMaxLength(255)
                .HasColumnName("magasin_nom");
            entity.Property(e => e.MaterielCoutUnitaire).HasColumnName("materiel_cout_unitaire");
            entity.Property(e => e.MaterielDevisId).HasColumnName("materiel_devis_id");
            entity.Property(e => e.MaterielLibelle)
                .HasMaxLength(255)
                .HasColumnName("materiel_libelle");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Magasin).WithMany(p => p.MagasinStocks)
                .HasForeignKey(d => d.MagasinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MagasinStock_magasin_id_fkey");

            entity.HasOne(d => d.MaterielDevis).WithMany(p => p.MagasinStocks)
                .HasForeignKey(d => d.MaterielDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MagasinStock_materiel_devis_id_fkey");
        });

        modelBuilder.Entity<MarqueCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MarqueCompteur_pkey");

            entity.ToTable("MarqueCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MarqueVehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MarqueVehicule_pkey");

            entity.ToTable("MarqueVehicule", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MaterielDevi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MaterielDevis_pkey");

            entity.ToTable("MaterielDevis", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CoutUnitaire).HasColumnName("cout_unitaire");
            entity.Property(e => e.CoutUnitaireMainDoeuvre)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("cout_unitaire_main_doeuvre");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.IsAdditional)
                .HasDefaultValue(false)
                .HasColumnName("is_additional");
            entity.Property(e => e.IsBrtcomplet)
                .HasDefaultValue(false)
                .HasColumnName("is_brtcomplet");
            entity.Property(e => e.IsCompteur)
                .HasDefaultValue(false)
                .HasColumnName("is_compteur");
            entity.Property(e => e.IsDetail)
                .HasDefaultValue(false)
                .HasColumnName("is_detail");
            entity.Property(e => e.IsDeviscomplementaire)
                .HasDefaultValue(false)
                .HasColumnName("is_deviscomplementaire");
            entity.Property(e => e.IsExtension)
                .HasDefaultValue(false)
                .HasColumnName("is_extension");
            entity.Property(e => e.IsMaterielPose)
                .HasDefaultValue(false)
                .HasColumnName("is_materiel_pose");
            entity.Property(e => e.IsSeparation)
                .HasDefaultValue(false)
                .HasColumnName("is_separation");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.MaterielDevis)
                .HasForeignKey(d => d.DiametreCompteurId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("MaterielDevis_diametre_compteur_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.MaterielDevis)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MaterielDevis_produit_id_fkey");
        });

        modelBuilder.Entity<MaterielsTypeDemandePardiametre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MaterielsTypeDemandePardiametre_pkey");

            entity.ToTable("MaterielsTypeDemandePardiametre", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Facturable)
                .HasDefaultValue(true)
                .HasColumnName("facturable");
            entity.Property(e => e.MaterielDevisCode)
                .HasMaxLength(255)
                .HasColumnName("materiel_devis_code");
            entity.Property(e => e.MaterielDevisId).HasColumnName("materiel_devis_id");
            entity.Property(e => e.MaterielDevisLibelle)
                .HasMaxLength(255)
                .HasColumnName("materiel_devis_libelle");
            entity.Property(e => e.Obligatoire)
                .HasDefaultValue(true)
                .HasColumnName("obligatoire");
            entity.Property(e => e.QuantiteDefaut)
                .HasDefaultValue(0)
                .HasColumnName("quantite_defaut");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.TypeDemandeLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_demande_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.MaterielDevis).WithMany(p => p.MaterielsTypeDemandePardiametres)
                .HasForeignKey(d => d.MaterielDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MaterielsTypeDemandePardiametre_materiel_devis_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.MaterielsTypeDemandePardiametres)
                .HasForeignKey(d => d.TypeDemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MaterielsTypeDemandePardiametre_type_demande_id_fkey");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Menu_pkey");

            entity.ToTable("Menu");

            entity.HasIndex(e => e.Code, "menu_code_idx");

            entity.HasIndex(e => e.ParentCode, "menu_parent_code_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(false)
                .HasColumnName("active");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Disabled)
                .HasDefaultValue(false)
                .HasColumnName("disabled");
            entity.Property(e => e.Icon)
                .HasMaxLength(255)
                .HasColumnName("icon");
            entity.Property(e => e.IsGroupTitle)
                .HasDefaultValue(false)
                .HasColumnName("is_group_title");
            entity.Property(e => e.IsLoadMenu)
                .HasDefaultValue(false)
                .HasColumnName("is_load_menu");
            entity.Property(e => e.IsModule)
                .HasDefaultValue(false)
                .HasColumnName("is_module");
            entity.Property(e => e.IsSide)
                .HasDefaultValue(true)
                .HasColumnName("is_side");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Namespace)
                .HasMaxLength(255)
                .HasColumnName("namespace");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(255)
                .HasColumnName("parent_code");
            entity.Property(e => e.Permission)
                .HasMaxLength(255)
                .HasColumnName("permission");
            entity.Property(e => e.Routing)
                .HasMaxLength(255)
                .HasColumnName("routing");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MethodePoseCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MethodePoseCompteur_pkey");

            entity.ToTable("MethodePoseCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MetreRefection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MetreRefection_pkey");

            entity.ToTable("MetreRefection", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduMetreId).HasColumnName("compte_rendu_metre_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeRefectionId).HasColumnName("type_refection_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CompteRenduMetre).WithMany(p => p.MetreRefections)
                .HasForeignKey(d => d.CompteRenduMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MetreRefection_compte_rendu_metre_id_fkey");

            entity.HasOne(d => d.TypeRefection).WithMany(p => p.MetreRefections)
                .HasForeignKey(d => d.TypeRefectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MetreRefection_type_refection_id_fkey");
        });

        modelBuilder.Entity<MetreTravauxAdditionnel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MetreTravauxAdditionnel_pkey");

            entity.ToTable("MetreTravauxAdditionnel", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduMetreId).HasColumnName("compte_rendu_metre_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeTravauxAdditionnelId).HasColumnName("type_travaux_additionnel_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CompteRenduMetre).WithMany(p => p.MetreTravauxAdditionnels)
                .HasForeignKey(d => d.CompteRenduMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MetreTravauxAdditionnel_compte_rendu_metre_id_fkey");

            entity.HasOne(d => d.TypeTravauxAdditionnel).WithMany(p => p.MetreTravauxAdditionnels)
                .HasForeignKey(d => d.TypeTravauxAdditionnelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MetreTravauxAdditionnel_type_travaux_additionnel_id_fkey");
        });

        modelBuilder.Entity<MetreTravauxAfaire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MetreTravauxAfaire_pkey");

            entity.ToTable("MetreTravauxAfaire", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CompteRenduMetreId).HasColumnName("compte_rendu_metre_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeTravauxId).HasColumnName("type_travaux_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CompteRenduMetre).WithMany(p => p.MetreTravauxAfaires)
                .HasForeignKey(d => d.CompteRenduMetreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MetreTravauxAfaire_compte_rendu_metre_id_fkey");

            entity.HasOne(d => d.TypeTravaux).WithMany(p => p.MetreTravauxAfaires)
                .HasForeignKey(d => d.TypeTravauxId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MetreTravauxAfaire_type_travaux_id_fkey");
        });

        modelBuilder.Entity<ModeApplicationTarif>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ModeApplicationTarif_pkey");

            entity.ToTable("ModeApplicationTarif", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ModeCalcul>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ModeCalcul_pkey");

            entity.ToTable("ModeCalcul", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ModeFacturation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ModeFacturation_pkey");

            entity.ToTable("ModeFacturation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ModePaiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ModePaiement_pkey");

            entity.ToTable("ModePaiement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ModeReglement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ModeReglement_pkey");

            entity.ToTable("ModeReglement", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ModeleVehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ModeleVehicule_pkey");

            entity.ToTable("ModeleVehicule", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Moratoire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Moratoire_pkey");

            entity.ToTable("Moratoire", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CampagneId)
                .HasMaxLength(255)
                .HasColumnName("campagne_id");
            entity.Property(e => e.CampagneLibelle)
                .HasMaxLength(255)
                .HasColumnName("campagne_libelle");
            entity.Property(e => e.CodeCentre)
                .HasMaxLength(255)
                .HasColumnName("code_centre");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FactureClientId)
                .HasMaxLength(255)
                .HasColumnName("facture_client_id");
            entity.Property(e => e.IsPrecontentieux).HasColumnName("is_precontentieux");
            entity.Property(e => e.LibelleTopCode)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_code");
            entity.Property(e => e.LibelleTopId).HasColumnName("libelle_top_id");
            entity.Property(e => e.LibelleTopLibelle)
                .HasMaxLength(255)
                .HasColumnName("libelle_top_libelle");
            entity.Property(e => e.MontantMinApayer).HasColumnName("montant_min_apayer");
            entity.Property(e => e.MontantRestantApayer).HasColumnName("montant_restant_apayer");
            entity.Property(e => e.NombreEcheance).HasColumnName("nombre_echeance");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroFacture)
                .HasMaxLength(255)
                .HasColumnName("numero_facture");
            entity.Property(e => e.OrdreMarche).HasColumnName("ordre_marche");
            entity.Property(e => e.PeriodiciteMoratoireId).HasColumnName("periodicite_moratoire_id");
            entity.Property(e => e.PeriodiciteMoratoireLibelle)
                .HasMaxLength(255)
                .HasColumnName("periodicite_moratoire_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutMoratoireId).HasColumnName("statut_moratoire_id");
            entity.Property(e => e.StatutMoratoireLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_moratoire_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.FactureClient).WithMany(p => p.Moratoires)
                .HasForeignKey(d => d.FactureClientId)
                .HasConstraintName("Moratoire_facture_client_id_fkey");

            entity.HasOne(d => d.PeriodiciteMoratoire).WithMany(p => p.Moratoires)
                .HasForeignKey(d => d.PeriodiciteMoratoireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Moratoire_periodicite_moratoire_id_fkey");
        });

        modelBuilder.Entity<MoratoireDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MoratoireDetail_pkey");

            entity.ToTable("MoratoireDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEcheance).HasColumnName("date_echeance");
            entity.Property(e => e.DateExigibilite).HasColumnName("date_exigibilite");
            entity.Property(e => e.DateReglement).HasColumnName("date_reglement");
            entity.Property(e => e.DateValeur).HasColumnName("date_valeur");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FraisRetard)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("frais_retard");
            entity.Property(e => e.Montant)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant");
            entity.Property(e => e.MontantRegle)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant_regle");
            entity.Property(e => e.MontantRestant)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant_restant");
            entity.Property(e => e.MoratoireId).HasColumnName("moratoire_id");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.NumeroFacture)
                .HasMaxLength(255)
                .HasColumnName("numero_facture");
            entity.Property(e => e.Refem)
                .HasMaxLength(255)
                .HasColumnName("refem");
            entity.Property(e => e.Sens)
                .HasMaxLength(255)
                .HasColumnName("sens");
            entity.Property(e => e.Solde)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("solde");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutMoratoireId).HasColumnName("statut_moratoire_id");
            entity.Property(e => e.StatutMoratoireLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_moratoire_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Moratoire).WithMany(p => p.MoratoireDetails)
                .HasForeignKey(d => d.MoratoireId)
                .HasConstraintName("MoratoireDetail_moratoire_id_fkey");

            entity.HasOne(d => d.StatutMoratoire).WithMany(p => p.MoratoireDetails)
                .HasForeignKey(d => d.StatutMoratoireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MoratoireDetail_statut_moratoire_id_fkey");
        });

        modelBuilder.Entity<MotifAnnulation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MotifAnnulation_pkey");

            entity.ToTable("MotifAnnulation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MotifDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MotifDemande_pkey");

            entity.ToTable("MotifDemande", "sc_sge");

            entity.HasIndex(e => e.Code, "motifdemande_code_idx");

            entity.HasIndex(e => e.Famille, "motifdemande_famille_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MotifImpossibilite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MotifImpossibilite_pkey");

            entity.ToTable("MotifImpossibilite", "sc_sge");

            entity.HasIndex(e => e.Code, "motifimpossibilite_code_idx");

            entity.HasIndex(e => e.Famille, "motifimpossibilite_famille_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MotifPoseCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MotifPoseCompteur_pkey");

            entity.ToTable("MotifPoseCompteur", "sc_sge");

            entity.HasIndex(e => e.Code, "motifposecompteur_code_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MotifRejet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MotifRejet_pkey");

            entity.ToTable("MotifRejet", "sc_sge");

            entity.HasIndex(e => e.Code, "motifrejet_code_idx");

            entity.HasIndex(e => e.Famille, "motifrejet_famille_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MotifReleve>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MotifReleve_pkey");

            entity.ToTable("MotifReleve", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<MoyenPaiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MoyenPaiement_pkey");

            entity.ToTable("MoyenPaiement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Nationnalite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Nationnalite_pkey");

            entity.ToTable("Nationnalite", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<NatureBranchement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NatureBranchement_pkey");

            entity.ToTable("NatureBranchement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<NatureConduite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NatureConduite_pkey");

            entity.ToTable("NatureConduite", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<NaturePiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NaturePiece_pkey");

            entity.ToTable("NaturePiece", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<NatureTuyau>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NatureTuyau_pkey");

            entity.ToTable("NatureTuyau", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Object>(entity =>
        {
            entity.HasKey(e => e.Objectid).HasName("objects_pkey");

            entity.ToTable("objects", "sc_sge");

            entity.HasIndex(e => e.Ln, "objects_ln_key").IsUnique();

            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Description2).HasColumnName("description2");
            entity.Property(e => e.Ln)
                .HasMaxLength(50)
                .HasColumnName("ln");
            entity.Property(e => e.Objecttype)
                .HasMaxLength(50)
                .HasColumnName("objecttype");
        });

        modelBuilder.Entity<Objectrelation>(entity =>
        {
            entity.HasKey(e => e.Relationid).HasName("objectrelations_pkey");

            entity.ToTable("objectrelations", "sc_sge");

            entity.Property(e => e.Relationid).HasColumnName("relationid");
            entity.Property(e => e.Childobjectid).HasColumnName("childobjectid");
            entity.Property(e => e.Parentobjectid).HasColumnName("parentobjectid");
            entity.Property(e => e.Relationtype)
                .HasMaxLength(50)
                .HasColumnName("relationtype");

            entity.HasOne(d => d.Childobject).WithMany(p => p.ObjectrelationChildobjects)
                .HasForeignKey(d => d.Childobjectid)
                .HasConstraintName("fk_child_object");

            entity.HasOne(d => d.Parentobject).WithMany(p => p.ObjectrelationParentobjects)
                .HasForeignKey(d => d.Parentobjectid)
                .HasConstraintName("fk_parent_object");
        });

        modelBuilder.Entity<OutputsModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OutputsModel_pkey");

            entity.ToTable("OutputsModel");

            entity.HasIndex(e => e.Code, "outputs_model_code");

            entity.HasIndex(e => e.Family, "outputs_model_family");

            entity.HasIndex(e => e.Label, "outputs_model_label");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Args)
                .HasColumnType("json")
                .HasColumnName("args");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Family)
                .HasMaxLength(255)
                .HasColumnName("family");
            entity.Property(e => e.IsLocked)
                .HasDefaultValue(false)
                .HasColumnName("is_locked");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.LockedBy)
                .HasMaxLength(255)
                .HasColumnName("locked_by");
            entity.Property(e => e.Schema).HasColumnName("schema");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlModel)
                .HasMaxLength(255)
                .HasColumnName("url_model");
        });

        modelBuilder.Entity<Paiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Paiement_pkey");

            entity.ToTable("Paiement", "sc_sge");

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key1").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key2").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key3").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key4").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key5").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key6").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key7").IsUnique();

            entity.HasIndex(e => e.RefPaiement, "Paiement_ref_paiement_key8").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvisId).HasColumnName("avis_id");
            entity.Property(e => e.AvisLibelle)
                .HasMaxLength(255)
                .HasColumnName("avis_libelle");
            entity.Property(e => e.CaisseId)
                .HasMaxLength(255)
                .HasColumnName("caisse_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Commentaire)
                .HasMaxLength(255)
                .HasColumnName("commentaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateAnnulation).HasColumnName("date_annulation");
            entity.Property(e => e.DatePaiement).HasColumnName("date_paiement");
            entity.Property(e => e.DateReaffectation).HasColumnName("date_reaffectation");
            entity.Property(e => e.DateRejet).HasColumnName("date_rejet");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeurMatricule)
                .HasMaxLength(255)
                .HasColumnName("demandeur_matricule");
            entity.Property(e => e.DemandeurNom)
                .HasMaxLength(255)
                .HasColumnName("demandeur_nom");
            entity.Property(e => e.IsEncaissement)
                .HasDefaultValue(true)
                .HasColumnName("is_encaissement");
            entity.Property(e => e.MatriculeCaissier)
                .HasMaxLength(255)
                .HasColumnName("matricule_caissier");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.MotifAnnulationId).HasColumnName("motif_annulation_id");
            entity.Property(e => e.MotifAnnulationLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_annulation_libelle");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.MotifRejetLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_rejet_libelle");
            entity.Property(e => e.NumeroCaisse)
                .HasMaxLength(255)
                .HasColumnName("numero_caisse");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroRecu)
                .HasMaxLength(255)
                .HasColumnName("numero_recu");
            entity.Property(e => e.PosteId)
                .HasMaxLength(255)
                .HasColumnName("poste_id");
            entity.Property(e => e.RefPaiement)
                .HasMaxLength(255)
                .HasColumnName("ref_paiement");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutPaiementId).HasColumnName("statut_paiement_id");
            entity.Property(e => e.StatutPaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_paiement_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlRecuPaiement)
                .HasMaxLength(255)
                .HasColumnName("url_recu_paiement");
            entity.Property(e => e.ValidateurMatricule)
                .HasMaxLength(255)
                .HasColumnName("validateur_matricule");
            entity.Property(e => e.ValidateurNom)
                .HasMaxLength(255)
                .HasColumnName("validateur_nom");

            entity.HasOne(d => d.Avis).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.AvisId)
                .HasConstraintName("Paiement_avis_id_fkey");

            entity.HasOne(d => d.Caisse).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.CaisseId)
                .HasConstraintName("Paiement_caisse_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("Paiement_client_id_fkey");

            entity.HasOne(d => d.MotifAnnulation).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.MotifAnnulationId)
                .HasConstraintName("Paiement_motif_annulation_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.MotifRejetId)
                .HasConstraintName("Paiement_motif_rejet_id_fkey");

            entity.HasOne(d => d.Poste).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.PosteId)
                .HasConstraintName("Paiement_poste_id_fkey");

            entity.HasOne(d => d.StatutPaiement).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.StatutPaiementId)
                .HasConstraintName("Paiement_statut_paiement_id_fkey");
        });

        modelBuilder.Entity<PaiementMoyensUtilise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PaiementMoyensUtilise_pkey");

            entity.ToTable("PaiementMoyensUtilise", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CanceledUser)
                .HasMaxLength(255)
                .HasColumnName("canceled_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateAnnulation).HasColumnName("date_annulation");
            entity.Property(e => e.DatePaiement).HasColumnName("date_paiement");
            entity.Property(e => e.DateRejet).HasColumnName("date_rejet");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MonnaieArendre).HasColumnName("monnaie_arendre");
            entity.Property(e => e.MonnaieAvoir).HasColumnName("monnaie_avoir");
            entity.Property(e => e.MonnaieRendu).HasColumnName("monnaie_rendu");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.MotifAnnulationId).HasColumnName("motif_annulation_id");
            entity.Property(e => e.MotifAnnulationLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_annulation_libelle");
            entity.Property(e => e.MotifRejetId).HasColumnName("motif_rejet_id");
            entity.Property(e => e.MotifRejetLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_rejet_libelle");
            entity.Property(e => e.MoyenPaiementId).HasColumnName("moyen_paiement_id");
            entity.Property(e => e.MoyenPaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("moyen_paiement_libelle");
            entity.Property(e => e.NumeroCaisse)
                .HasMaxLength(255)
                .HasColumnName("numero_caisse");
            entity.Property(e => e.PaiementId)
                .HasMaxLength(255)
                .HasColumnName("paiement_id");
            entity.Property(e => e.PartenairePaiementCode)
                .HasMaxLength(255)
                .HasColumnName("partenaire_paiement_code");
            entity.Property(e => e.PartenairePaiementId).HasColumnName("partenaire_paiement_id");
            entity.Property(e => e.PartenairePaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("partenaire_paiement_libelle");
            entity.Property(e => e.ReferencePaiement)
                .HasMaxLength(255)
                .HasColumnName("reference_paiement");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutPaiementId).HasColumnName("statut_paiement_id");
            entity.Property(e => e.StatutPaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_paiement_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.MotifAnnulation).WithMany(p => p.PaiementMoyensUtilises)
                .HasForeignKey(d => d.MotifAnnulationId)
                .HasConstraintName("PaiementMoyensUtilise_motif_annulation_id_fkey");

            entity.HasOne(d => d.MotifRejet).WithMany(p => p.PaiementMoyensUtilises)
                .HasForeignKey(d => d.MotifRejetId)
                .HasConstraintName("PaiementMoyensUtilise_motif_rejet_id_fkey");

            entity.HasOne(d => d.MoyenPaiement).WithMany(p => p.PaiementMoyensUtilises)
                .HasForeignKey(d => d.MoyenPaiementId)
                .HasConstraintName("PaiementMoyensUtilise_moyen_paiement_id_fkey");

            entity.HasOne(d => d.Paiement).WithMany(p => p.PaiementMoyensUtilises)
                .HasForeignKey(d => d.PaiementId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PaiementMoyensUtilise_paiement_id_fkey");

            entity.HasOne(d => d.PartenairePaiement).WithMany(p => p.PaiementMoyensUtilises)
                .HasForeignKey(d => d.PartenairePaiementId)
                .HasConstraintName("PaiementMoyensUtilise_partenaire_paiement_id_fkey");

            entity.HasOne(d => d.StatutPaiement).WithMany(p => p.PaiementMoyensUtilises)
                .HasForeignKey(d => d.StatutPaiementId)
                .HasConstraintName("PaiementMoyensUtilise_statut_paiement_id_fkey");
        });

        modelBuilder.Entity<ParamCaution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ParamCaution_pkey");

            entity.ToTable("ParamCaution", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.Montant)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("montant");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.ParamCautions)
                .HasForeignKey(d => d.DiametreCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ParamCaution_diametre_compteur_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.ParamCautions)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ParamCaution_produit_id_fkey");
        });

        modelBuilder.Entity<PartenairePaiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PartenairePaiement_pkey");

            entity.ToTable("PartenairePaiement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypePartenaireId).HasColumnName("type_partenaire_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypePartenaire).WithMany(p => p.PartenairePaiements)
                .HasForeignKey(d => d.TypePartenaireId)
                .HasConstraintName("PartenairePaiement_type_partenaire_id_fkey");
        });

        modelBuilder.Entity<Pdl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pdl_pkey");

            entity.ToTable("Pdl", "sc_sge");

            entity.HasIndex(e => e.NumeroPdl, "Pdl_numero_pdl_key").IsUnique();

            entity.HasIndex(e => e.NumeroPdl, "Pdl_numero_pdl_key1").IsUnique();

            entity.HasIndex(e => e.NumeroPdl, "Pdl_numero_pdl_key2").IsUnique();

            entity.HasIndex(e => e.NumeroPdl, "Pdl_numero_pdl_key3").IsUnique();

            entity.HasIndex(e => e.NumeroPdl, "Pdl_numero_pdl_key4").IsUnique();

            entity.HasIndex(e => e.NumeroPdl, "Pdl_numero_pdl_key5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.NumeroPdl)
                .HasMaxLength(255)
                .HasColumnName("numero_pdl");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Periode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Periode_pkey");

            entity.ToTable("Periode", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<PeriodeFacturation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PeriodeFacturation_pkey");

            entity.ToTable("PeriodeFacturation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<PeriodiciteMoratoire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PeriodiciteMoratoire_pkey");

            entity.ToTable("PeriodiciteMoratoire", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Valeur)
                .HasDefaultValue(0)
                .HasColumnName("valeur");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Permission_pkey");

            entity.ToTable("Permission");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<PersonnesMoralesLiee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PersonnesMoralesLiee_pkey");

            entity.ToTable("PersonnesMoralesLiee", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BoitePostal)
                .HasMaxLength(255)
                .HasColumnName("boite_postal");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DenominationId).HasColumnName("denomination_id");
            entity.Property(e => e.DenominationLibelle)
                .HasMaxLength(255)
                .HasColumnName("denomination_libelle");
            entity.Property(e => e.Fax)
                .HasMaxLength(255)
                .HasColumnName("fax");
            entity.Property(e => e.LienFacebook)
                .HasMaxLength(255)
                .HasColumnName("lien_facebook");
            entity.Property(e => e.LienLinkedin)
                .HasMaxLength(255)
                .HasColumnName("lien_linkedin");
            entity.Property(e => e.NationnaliteId).HasColumnName("nationnalite_id");
            entity.Property(e => e.NationnaliteLibelle)
                .HasMaxLength(255)
                .HasColumnName("nationnalite_libelle");
            entity.Property(e => e.NumeroAgrement)
                .HasMaxLength(255)
                .HasColumnName("numero_agrement");
            entity.Property(e => e.NumeroFiscal)
                .HasMaxLength(255)
                .HasColumnName("numero_fiscal");
            entity.Property(e => e.NumeroRccm)
                .HasMaxLength(255)
                .HasColumnName("numero_rccm");
            entity.Property(e => e.RaisonSociale)
                .HasMaxLength(255)
                .HasColumnName("raison_sociale");
            entity.Property(e => e.Sigle)
                .HasMaxLength(255)
                .HasColumnName("sigle");
            entity.Property(e => e.SiteWeb)
                .HasMaxLength(255)
                .HasColumnName("site_web");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TelephoneFixe)
                .HasMaxLength(255)
                .HasColumnName("telephone_fixe");
            entity.Property(e => e.Uid)
                .HasMaxLength(255)
                .HasColumnName("uid");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Whatsapp)
                .HasMaxLength(255)
                .HasColumnName("whatsapp");
        });

        modelBuilder.Entity<PersonnesMoralesPiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PersonnesMoralesPiece_pkey");

            entity.ToTable("PersonnesMoralesPiece", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<PersonnesPhysiquePiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PersonnesPhysiquePiece_pkey");

            entity.ToTable("PersonnesPhysiquePiece", "sc_sge");

            entity.HasIndex(e => new { e.TypePieceId, e.PersonnePhysiqueId }, "PersonnesPhysiquePiece_type_piece_id_personne_physique_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEtablissement).HasColumnName("date_etablissement");
            entity.Property(e => e.DateExpiration).HasColumnName("date_expiration");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.NumeroPiece)
                .HasMaxLength(255)
                .HasColumnName("numero_piece");
            entity.Property(e => e.PersonnePhysiqueId)
                .HasMaxLength(255)
                .HasColumnName("personne_physique_id");
            entity.Property(e => e.RefFichier)
                .HasMaxLength(255)
                .HasColumnName("ref_fichier");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypePieceId).HasColumnName("type_piece_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlFichier)
                .HasMaxLength(255)
                .HasColumnName("url_fichier");

            entity.HasOne(d => d.PersonnePhysique).WithMany(p => p.PersonnesPhysiquePieces)
                .HasForeignKey(d => d.PersonnePhysiqueId)
                .HasConstraintName("PersonnesPhysiquePiece_personne_physique_id_fkey");

            entity.HasOne(d => d.TypePiece).WithMany(p => p.PersonnesPhysiquePieces)
                .HasForeignKey(d => d.TypePieceId)
                .HasConstraintName("PersonnesPhysiquePiece_type_piece_id_fkey");
        });

        modelBuilder.Entity<PersonnesPhysiquesLiee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PersonnesPhysiquesLiee_pkey");

            entity.ToTable("PersonnesPhysiquesLiee", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.BoitePostal)
                .HasMaxLength(255)
                .HasColumnName("boite_postal");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateNaissance).HasColumnName("date_naissance");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DenominationId).HasColumnName("denomination_id");
            entity.Property(e => e.DenominationLibelle)
                .HasMaxLength(255)
                .HasColumnName("denomination_libelle");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fax)
                .HasMaxLength(255)
                .HasColumnName("fax");
            entity.Property(e => e.LieuNaissance)
                .HasMaxLength(255)
                .HasColumnName("lieu_naissance");
            entity.Property(e => e.NationnaliteId).HasColumnName("nationnalite_id");
            entity.Property(e => e.NationnaliteLibelle)
                .HasMaxLength(255)
                .HasColumnName("nationnalite_libelle");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.Prenoms)
                .HasMaxLength(255)
                .HasColumnName("prenoms");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Telephone)
                .HasMaxLength(255)
                .HasColumnName("telephone");
            entity.Property(e => e.TelephoneFixe)
                .HasMaxLength(255)
                .HasColumnName("telephone_fixe");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Whatsapp)
                .HasMaxLength(255)
                .HasColumnName("whatsapp");
        });

        modelBuilder.Entity<PhaseCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PhaseCompteur_pkey");

            entity.ToTable("PhaseCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<PointsPiquage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PointsPiquage_pkey");

            entity.ToTable("PointsPiquage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CommuneId)
                .HasMaxLength(255)
                .HasColumnName("commune_id");
            entity.Property(e => e.CommuneLibelle)
                .HasMaxLength(255)
                .HasColumnName("commune_libelle");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Numero)
                .HasMaxLength(255)
                .HasColumnName("numero");
            entity.Property(e => e.PressionMatin).HasColumnName("pression_matin");
            entity.Property(e => e.PressionSoir).HasColumnName("pression_soir");
            entity.Property(e => e.QuartierId).HasColumnName("quartier_id");
            entity.Property(e => e.QuartierLibelle)
                .HasMaxLength(255)
                .HasColumnName("quartier_libelle");
            entity.Property(e => e.SecteurId).HasColumnName("secteur_id");
            entity.Property(e => e.SecteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("secteur_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VilleId)
                .HasMaxLength(255)
                .HasColumnName("ville_id");
            entity.Property(e => e.VilleLibelle)
                .HasMaxLength(255)
                .HasColumnName("ville_libelle");

            entity.HasOne(d => d.Commune).WithMany(p => p.PointsPiquages)
                .HasForeignKey(d => d.CommuneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PointsPiquage_commune_id_fkey");

            entity.HasOne(d => d.Quartier).WithMany(p => p.PointsPiquages)
                .HasForeignKey(d => d.QuartierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PointsPiquage_quartier_id_fkey");

            entity.HasOne(d => d.Secteur).WithMany(p => p.PointsPiquages)
                .HasForeignKey(d => d.SecteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PointsPiquage_secteur_id_fkey");

            entity.HasOne(d => d.Ville).WithMany(p => p.PointsPiquages)
                .HasForeignKey(d => d.VilleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PointsPiquage_ville_id_fkey");
        });

        modelBuilder.Entity<PoseCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PoseCompteur_pkey");

            entity.ToTable("PoseCompteur", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AgentId)
                .HasMaxLength(255)
                .HasColumnName("agent_id");
            entity.Property(e => e.BranchementEauPdlId).HasColumnName("branchement_eau_pdl_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CompteurId)
                .HasMaxLength(255)
                .HasColumnName("compteur_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDepose).HasColumnName("date_depose");
            entity.Property(e => e.DatePose).HasColumnName("date_pose");
            entity.Property(e => e.DateSaisie).HasColumnName("date_saisie");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.IndexDepose).HasColumnName("index_depose");
            entity.Property(e => e.MatriculeAgent)
                .HasMaxLength(255)
                .HasColumnName("matricule_agent");
            entity.Property(e => e.MethodeDeposeId).HasColumnName("methode_depose_id");
            entity.Property(e => e.MethodeDeposeLibelle)
                .HasMaxLength(255)
                .HasColumnName("methode_depose_libelle");
            entity.Property(e => e.MethodePoseId).HasColumnName("methode_pose_id");
            entity.Property(e => e.MethodePoseLibelle)
                .HasMaxLength(255)
                .HasColumnName("methode_pose_libelle");
            entity.Property(e => e.MotifDeposeId).HasColumnName("motif_depose_id");
            entity.Property(e => e.MotifDeposeLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_depose_libelle");
            entity.Property(e => e.MotifPoseId).HasColumnName("motif_pose_id");
            entity.Property(e => e.MotifPoseLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_pose_libelle");
            entity.Property(e => e.NomCompletAgent)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_agent");
            entity.Property(e => e.NomCompletClient)
                .HasMaxLength(255)
                .HasColumnName("nom_complet_client");
            entity.Property(e => e.NumeroAbonnement)
                .HasMaxLength(255)
                .HasColumnName("numero_abonnement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroCompteur)
                .HasMaxLength(255)
                .HasColumnName("numero_compteur");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Agent).WithMany(p => p.PoseCompteurs)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("PoseCompteur_agent_id_fkey");

            entity.HasOne(d => d.BranchementEauPdl).WithMany(p => p.PoseCompteurs)
                .HasForeignKey(d => d.BranchementEauPdlId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PoseCompteur_branchement_eau_pdl_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.PoseCompteurs)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("PoseCompteur_client_id_fkey");

            entity.HasOne(d => d.MethodePose).WithMany(p => p.PoseCompteurs)
                .HasForeignKey(d => d.MethodePoseId)
                .HasConstraintName("PoseCompteur_methode_pose_id_fkey");

            entity.HasOne(d => d.MotifPose).WithMany(p => p.PoseCompteurs)
                .HasForeignKey(d => d.MotifPoseId)
                .HasConstraintName("PoseCompteur_motif_pose_id_fkey");
        });

        modelBuilder.Entity<Poste>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Poste_pkey");

            entity.ToTable("Poste", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CaisseId)
                .HasMaxLength(255)
                .HasColumnName("caisse_id");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CheminEdition)
                .HasMaxLength(255)
                .HasColumnName("chemin_edition");
            entity.Property(e => e.CodeCentre)
                .HasMaxLength(255)
                .HasColumnName("code_centre");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Ip)
                .HasMaxLength(255)
                .HasColumnName("ip");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.NumeroCaisse)
                .HasMaxLength(255)
                .HasColumnName("numero_caisse");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Caisse).WithMany(p => p.Postes)
                .HasForeignKey(d => d.CaisseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Poste_caisse_id_fkey");

            // Configuration de la relation one-to-many avec Equipement
            entity.HasMany(p => p.Equipements)
                .WithOne(e => e.Poste)
                .HasForeignKey(e => e.PosteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Equipement_Poste");
        });

        modelBuilder.Entity<Postetransformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("postetransformation_pk");

            entity.ToTable("postetransformation", "sc_sge");

            entity.HasIndex(e => e.CentreId, "postetransformation_centre_id");

            entity.HasIndex(e => e.Code, "postetransformation_code");

            entity.HasIndex(e => e.DatePose, "postetransformation_date_pose");

            entity.HasIndex(e => e.NomCentre, "postetransformation_nom_centre");

            entity.HasIndex(e => e.Status, "postetransformation_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasColumnType("character varying")
                .HasColumnName("added_user");
            entity.Property(e => e.CentreId)
                .HasColumnType("character varying")
                .HasColumnName("centre_id");
            entity.Property(e => e.Code)
                .HasColumnType("character varying")
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DatePose).HasColumnName("date_pose");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasColumnType("character varying")
                .HasColumnName("libelle");
            entity.Property(e => e.NomCentre)
                .HasColumnType("character varying")
                .HasColumnName("nom_centre");
            entity.Property(e => e.NomFabriquant)
                .HasColumnType("character varying")
                .HasColumnName("nom_fabriquant");
            entity.Property(e => e.PissanceSortie)
                .HasColumnType("character varying")
                .HasColumnName("pissance_sortie");
            entity.Property(e => e.PuissanceEntree)
                .HasColumnType("character varying")
                .HasColumnName("puissance_entree");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasColumnType("character varying")
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Process_pkey");

            entity.ToTable("Process");

            entity.HasIndex(e => e.Code, "Process_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "Process_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "process_code_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(false)
                .HasColumnName("active");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AutoStart)
                .HasDefaultValue(false)
                .HasColumnName("auto_start");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Delai)
                .HasComment("delai d'exécution en min")
                .HasColumnName("delai");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsGroupTitle)
                .HasDefaultValue(false)
                .HasColumnName("is_group_title");
            entity.Property(e => e.ModelsToUse)
                .HasColumnType("json")
                .HasColumnName("models_to_use");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(255)
                .HasColumnName("parent_code");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Produit_pkey");

            entity.ToTable("Produit", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Profile_pkey");

            entity.ToTable("Profile");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<ProfileMenu>(entity =>
        {
            entity.HasKey(e => new { e.ProfileId, e.MenuId }).HasName("ProfileMenu_pkey");

            entity.ToTable("ProfileMenu");

            entity.HasIndex(e => e.MenuId, "profile_menu_menu_id");

            entity.HasIndex(e => e.ProfileId, "profile_menu_profile_id");

            entity.HasIndex(e => e.Status, "profile_menu_status");

            entity.Property(e => e.ProfileId)
                .HasMaxLength(255)
                .HasColumnName("profile_id");
            entity.Property(e => e.MenuId)
                .HasMaxLength(255)
                .HasColumnName("menu_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Permission)
                .HasMaxLength(255)
                .HasColumnName("permission");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Menu).WithMany(p => p.ProfileMenus)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("ProfileMenu_menu_id_fkey");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfileMenus)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("ProfileMenu_profile_id_fkey");
        });

        modelBuilder.Entity<ProfilePermission>(entity =>
        {
            entity.HasKey(e => new { e.ProfileId, e.PermissionId }).HasName("ProfilePermission_pkey");

            entity.ToTable("ProfilePermission");

            entity.HasIndex(e => e.PermissionId, "profile_permission_permission_id");

            entity.HasIndex(e => e.ProfileId, "profile_permission_profile_id");

            entity.HasIndex(e => e.Status, "profile_permission_status");

            entity.Property(e => e.ProfileId)
                .HasMaxLength(255)
                .HasColumnName("profile_id");
            entity.Property(e => e.PermissionId)
                .HasMaxLength(255)
                .HasColumnName("permission_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Permission).WithMany(p => p.ProfilePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("ProfilePermission_permission_id_fkey");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfilePermissions)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("ProfilePermission_profile_id_fkey");
        });

        modelBuilder.Entity<ProgrammationTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProgrammationTravaux_pkey");

            entity.ToTable("ProgrammationTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutPrevu).HasColumnName("date_debut_prevu");
            entity.Property(e => e.DateFinPrevu).HasColumnName("date_fin_prevu");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.EquipeId).HasColumnName("equipe_id");
            entity.Property(e => e.NumeroBi)
                .HasMaxLength(255)
                .HasColumnName("numero_bi");
            entity.Property(e => e.NumeroBs)
                .HasMaxLength(255)
                .HasColumnName("numero_bs");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VehiculeId)
                .HasMaxLength(255)
                .HasColumnName("vehicule_id");

            entity.HasOne(d => d.Demande).WithMany(p => p.ProgrammationTravauxes)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProgrammationTravaux_demande_id_fkey");

            entity.HasOne(d => d.Equipe).WithMany(p => p.ProgrammationTravauxes)
                .HasForeignKey(d => d.EquipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProgrammationTravaux_equipe_id_fkey");

            entity.HasOne(d => d.Vehicule).WithMany(p => p.ProgrammationTravauxes)
                .HasForeignKey(d => d.VehiculeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProgrammationTravaux_vehicule_id_fkey");
        });

        modelBuilder.Entity<PuissanceInstallee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PuissanceInstallee_pkey");

            entity.ToTable("PuissanceInstallee", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.KPerteActive1).HasColumnName("k_perte_active1");
            entity.Property(e => e.KPerteActive2).HasColumnName("k_perte_active2");
            entity.Property(e => e.KPerteReactive1).HasColumnName("k_perte_reactive1");
            entity.Property(e => e.KPerteReactive2).HasColumnName("k_perte_reactive2");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Valeur)
                .HasMaxLength(255)
                .HasColumnName("valeur");

            entity.HasOne(d => d.Produit).WithMany(p => p.PuissanceInstallees)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PuissanceInstallee_produit_id_fkey");
        });

        modelBuilder.Entity<PuissanceSouscrite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PuissanceSouscrite_pkey");

            entity.ToTable("PuissanceSouscrite", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Valeur)
                .HasMaxLength(255)
                .HasColumnName("valeur");
        });

        modelBuilder.Entity<Quartier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Quartier_pkey");

            entity.ToTable("Quartier", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CommuneId)
                .HasMaxLength(255)
                .HasColumnName("commune_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Commune).WithMany(p => p.Quartiers)
                .HasForeignKey(d => d.CommuneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Quartier_commune_id_fkey");
        });

        modelBuilder.Entity<RechercheTarif>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RechercheTarif_pkey");

            entity.ToTable("RechercheTarif", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Redevance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Redevance_pkey");

            entity.ToTable("Redevance", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.ProduitLibelle)
                .HasMaxLength(255)
                .HasColumnName("produit_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeLienRedevanceId).HasColumnName("type_lien_redevance_id");
            entity.Property(e => e.TypeLienRedevanceLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_lien_redevance_libelle");
            entity.Property(e => e.TypeRedevanceId).HasColumnName("type_redevance_id");
            entity.Property(e => e.TypeRedevanceLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_redevance_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.Redevances)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Redevance_produit_id_fkey");

            entity.HasOne(d => d.TypeLienRedevance).WithMany(p => p.Redevances)
                .HasForeignKey(d => d.TypeLienRedevanceId)
                .HasConstraintName("Redevance_type_lien_redevance_id_fkey");

            entity.HasOne(d => d.TypeRedevance).WithMany(p => p.Redevances)
                .HasForeignKey(d => d.TypeRedevanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Redevance_type_redevance_id_fkey");
        });

        modelBuilder.Entity<ReglageCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ReglageCompteur_pkey");

            entity.ToTable("ReglageCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CalibreCompteurId).HasColumnName("calibre_compteur_id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Reglage).HasColumnName("reglage");
            entity.Property(e => e.ReglageMax).HasColumnName("reglage_max");
            entity.Property(e => e.ReglageMin).HasColumnName("reglage_min");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CalibreCompteur).WithMany(p => p.ReglageCompteurs)
                .HasForeignKey(d => d.CalibreCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReglageCompteur_calibre_compteur_id_fkey");
        });

        modelBuilder.Entity<Reglement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reglement_pkey");

            entity.ToTable("Reglement", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateReglement).HasColumnName("date_reglement");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.IsEncaissement)
                .HasDefaultValue(true)
                .HasColumnName("is_encaissement");
            entity.Property(e => e.MontantARegler)
                .HasDefaultValue(0)
                .HasColumnName("montant_a_regler");
            entity.Property(e => e.MontantRegler)
                .HasDefaultValue(0)
                .HasColumnName("montant_regler");
            entity.Property(e => e.MontantRestant)
                .HasDefaultValue(0)
                .HasColumnName("montant_restant");
            entity.Property(e => e.NumeroRecu)
                .HasMaxLength(255)
                .HasColumnName("numero_recu");
            entity.Property(e => e.ObjetId)
                .HasMaxLength(255)
                .HasColumnName("objet_id");
            entity.Property(e => e.ObjetRef)
                .HasMaxLength(255)
                .HasColumnName("objet_ref");
            entity.Property(e => e.PaiementId)
                .HasMaxLength(255)
                .HasColumnName("paiement_id");
            entity.Property(e => e.RefPaiement)
                .HasMaxLength(255)
                .HasColumnName("ref_paiement");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutReglementId).HasColumnName("statut_reglement_id");
            entity.Property(e => e.StatutReglementLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_reglement_libelle");
            entity.Property(e => e.TypeObjetCode)
                .HasMaxLength(255)
                .HasColumnName("type_objet_code");
            entity.Property(e => e.TypeObjetId).HasColumnName("type_objet_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UrlRecuPaiement)
                .HasMaxLength(255)
                .HasColumnName("url_recu_paiement");

            entity.HasOne(d => d.Paiement).WithMany(p => p.Reglements)
                .HasForeignKey(d => d.PaiementId)
                .HasConstraintName("Reglement_paiement_id_fkey");

            entity.HasOne(d => d.StatutReglement).WithMany(p => p.Reglements)
                .HasForeignKey(d => d.StatutReglementId)
                .HasConstraintName("Reglement_statut_reglement_id_fkey");

            entity.HasOne(d => d.TypeObjet).WithMany(p => p.Reglements)
                .HasForeignKey(d => d.TypeObjetId)
                .HasConstraintName("Reglement_type_objet_id_fkey");
        });

        modelBuilder.Entity<Regroupement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Regroupement_pkey");

            entity.ToTable("Regroupement", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Adresse)
                .HasMaxLength(255)
                .HasColumnName("adresse");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.ContactRepresantant)
                .HasMaxLength(255)
                .HasColumnName("contact_represantant");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.NomRepresantant)
                .HasMaxLength(255)
                .HasColumnName("nom_represantant");
            entity.Property(e => e.PrenomRepresantant)
                .HasMaxLength(255)
                .HasColumnName("prenom_represantant");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Relance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Relance_pkey");

            entity.ToTable("Relance", "sc_sge");

            entity.HasIndex(e => e.NumeroRelance, "Relance_numero_relance_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminId)
                .HasMaxLength(255)
                .HasColumnName("admin_id");
            entity.Property(e => e.AdminMatricule)
                .HasMaxLength(255)
                .HasColumnName("admin_matricule");
            entity.Property(e => e.AdminNom)
                .HasMaxLength(255)
                .HasColumnName("admin_nom");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateRelance).HasColumnName("date_relance");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MotifId).HasColumnName("motif_id");
            entity.Property(e => e.MotifLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_libelle");
            entity.Property(e => e.NombreClient).HasColumnName("nombre_client");
            entity.Property(e => e.NombreFacture).HasColumnName("nombre_facture");
            entity.Property(e => e.NumeroRelance)
                .HasMaxLength(255)
                .HasColumnName("numero_relance");
            entity.Property(e => e.Parametres)
                .HasColumnType("json")
                .HasColumnName("parametres");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TotalFacture).HasColumnName("total_facture");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Motif).WithMany(p => p.Relances)
                .HasForeignKey(d => d.MotifId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relance_motif_id_fkey");
        });

        modelBuilder.Entity<RelanceClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RelanceClient_pkey");

            entity.ToTable("RelanceClient", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FacturesRelance)
                .HasColumnType("json")
                .HasColumnName("factures_relance");
            entity.Property(e => e.RelanceId)
                .HasMaxLength(255)
                .HasColumnName("relance_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Client).WithMany(p => p.RelanceClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RelanceClient_client_id_fkey");

            entity.HasOne(d => d.Relance).WithMany(p => p.RelanceClients)
                .HasForeignKey(d => d.RelanceId)
                .HasConstraintName("RelanceClient_relance_id_fkey");
        });

        modelBuilder.Entity<RelatedEntity>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.EntitySource, e.EntityDestination }).HasName("RelatedEntity_pkey");

            entity.ToTable("RelatedEntity");

            entity.HasIndex(e => new { e.EntitySource, e.EntityDestination }, "RelatedEntity_entity_source_entity_destination_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.EntitySource)
                .HasMaxLength(255)
                .HasColumnName("entity_source");
            entity.Property(e => e.EntityDestination)
                .HasMaxLength(255)
                .HasColumnName("entity_destination");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MetaData)
                .HasMaxLength(255)
                .HasColumnName("meta_data");
            entity.Property(e => e.RelationTypeId)
                .HasMaxLength(255)
                .HasColumnName("relation_type_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.EntityDestinationNavigation).WithMany(p => p.RelatedEntityEntityDestinationNavigations)
                .HasForeignKey(d => d.EntityDestination)
                .HasConstraintName("RelatedEntity_entity_destination_fkey");

            entity.HasOne(d => d.EntitySourceNavigation).WithMany(p => p.RelatedEntityEntitySourceNavigations)
                .HasForeignKey(d => d.EntitySource)
                .HasConstraintName("RelatedEntity_entity_source_fkey");

            entity.HasOne(d => d.RelationType).WithMany(p => p.RelatedEntities)
                .HasForeignKey(d => d.RelationTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RelatedEntity_relation_type_id_fkey");
        });

        modelBuilder.Entity<RelationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RelationType_pkey");

            entity.ToTable("RelationType");

            entity.HasIndex(e => e.Code, "RelationType_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key13").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key14").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key15").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key16").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key17").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key18").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key19").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key20").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "RelationType_code_key9").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Releve>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Releve_pkey");

            entity.ToTable("Releve", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbonnementId)
                .HasMaxLength(255)
                .HasColumnName("abonnement_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AgentId)
                .HasMaxLength(255)
                .HasColumnName("agent_id");
            entity.Property(e => e.AgentMatricule)
                .HasMaxLength(255)
                .HasColumnName("agent_matricule");
            entity.Property(e => e.AncienIndex).HasColumnName("ancien_index");
            entity.Property(e => e.CampagneCode)
                .HasMaxLength(255)
                .HasColumnName("campagne_code");
            entity.Property(e => e.CampagneLibelle)
                .HasMaxLength(255)
                .HasColumnName("campagne_libelle");
            entity.Property(e => e.CasGenere)
                .HasMaxLength(255)
                .HasColumnName("cas_genere");
            entity.Property(e => e.CasPrecedenteFacture)
                .HasMaxLength(255)
                .HasColumnName("cas_precedente_facture");
            entity.Property(e => e.CentreCode)
                .HasMaxLength(255)
                .HasColumnName("centre_code");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.CodeEvt)
                .HasMaxLength(255)
                .HasColumnName("code_evt");
            entity.Property(e => e.CoefComptage).HasColumnName("coef_comptage");
            entity.Property(e => e.CoefFacture).HasColumnName("coef_facture");
            entity.Property(e => e.CoefK1).HasColumnName("coef_k1");
            entity.Property(e => e.CoefK2).HasColumnName("coef_k2");
            entity.Property(e => e.CoefKr1).HasColumnName("coef_kr1");
            entity.Property(e => e.CoefKr2).HasColumnName("coef_kr2");
            entity.Property(e => e.CoefLecture).HasColumnName("coef_lecture");
            entity.Property(e => e.Commentaire)
                .HasMaxLength(255)
                .HasColumnName("commentaire");
            entity.Property(e => e.CompteurId)
                .HasMaxLength(255)
                .HasColumnName("compteur_id");
            entity.Property(e => e.ConsoFacture).HasColumnName("conso_facture");
            entity.Property(e => e.ConsoMoyennePrecedenteFacture).HasColumnName("conso_moyenne_precedente_facture");
            entity.Property(e => e.ConsoNonFacture).HasColumnName("conso_non_facture");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutExoneration).HasColumnName("date_debut_exoneration");
            entity.Property(e => e.DateFinExoneration).HasColumnName("date_fin_exoneration");
            entity.Property(e => e.DateReleve).HasColumnName("date_releve");
            entity.Property(e => e.DateRelevePrecedenteFacture).HasColumnName("date_releve_precedente_facture");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.DiametreCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("diametre_compteur_libelle");
            entity.Property(e => e.Enquete)
                .HasMaxLength(255)
                .HasColumnName("enquete");
            entity.Property(e => e.IndexPrecedenteFacture).HasColumnName("index_precedente_facture");
            entity.Property(e => e.IsConsoSeule).HasColumnName("is_conso_seule");
            entity.Property(e => e.IsExonereTva).HasColumnName("is_exonere_tva");
            entity.Property(e => e.IsFacturable).HasColumnName("is_facturable");
            entity.Property(e => e.IsFacture).HasColumnName("is_facture");
            entity.Property(e => e.IsVirtuel).HasColumnName("is_virtuel");
            entity.Property(e => e.LotRiId).HasColumnName("lot_ri_id");
            entity.Property(e => e.LotRiNumero)
                .HasMaxLength(255)
                .HasColumnName("lot_ri_numero");
            entity.Property(e => e.MotifReleveId).HasColumnName("motif_releve_id");
            entity.Property(e => e.MotifReleveLibelle)
                .HasMaxLength(255)
                .HasColumnName("motif_releve_libelle");
            entity.Property(e => e.NombreHabitant).HasColumnName("nombre_habitant");
            entity.Property(e => e.NombrePointDeau).HasColumnName("nombre_point_deau");
            entity.Property(e => e.NouveauCadran)
                .HasMaxLength(255)
                .HasColumnName("nouveau_cadran");
            entity.Property(e => e.NouveauCompteur)
                .HasMaxLength(255)
                .HasColumnName("nouveau_compteur");
            entity.Property(e => e.NouveauIndex).HasColumnName("nouveau_index");
            entity.Property(e => e.NumeroAbonnement)
                .HasMaxLength(255)
                .HasColumnName("numero_abonnement");
            entity.Property(e => e.NumeroBranchement)
                .HasMaxLength(255)
                .HasColumnName("numero_branchement");
            entity.Property(e => e.NumeroClient)
                .HasMaxLength(255)
                .HasColumnName("numero_client");
            entity.Property(e => e.NumeroCompteur)
                .HasMaxLength(255)
                .HasColumnName("numero_compteur");
            entity.Property(e => e.NumeroDemande)
                .HasMaxLength(255)
                .HasColumnName("numero_demande");
            entity.Property(e => e.NumeroPdl)
                .HasMaxLength(255)
                .HasColumnName("numero_pdl");
            entity.Property(e => e.NumeroReleve)
                .HasMaxLength(255)
                .HasColumnName("numero_releve");
            entity.Property(e => e.Ordre).HasColumnName("ordre");
            entity.Property(e => e.PdlId).HasColumnName("pdl_id");
            entity.Property(e => e.PeriodeCode)
                .HasMaxLength(255)
                .HasColumnName("periode_code");
            entity.Property(e => e.PeriodeId).HasColumnName("periode_id");
            entity.Property(e => e.PeriodeLibelle)
                .HasMaxLength(255)
                .HasColumnName("periode_libelle");
            entity.Property(e => e.PeriodePrecedenteFacture)
                .HasMaxLength(255)
                .HasColumnName("periode_precedente_facture");
            entity.Property(e => e.PhaseCompteur)
                .HasMaxLength(255)
                .HasColumnName("phase_compteur");
            entity.Property(e => e.PoseId)
                .HasMaxLength(255)
                .HasColumnName("pose_id");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Puissance).HasColumnName("puissance");
            entity.Property(e => e.PuissanceInstalle).HasColumnName("puissance_installe");
            entity.Property(e => e.ReglageCompteur)
                .HasMaxLength(255)
                .HasColumnName("reglage_compteur");
            entity.Property(e => e.RegroupementCode)
                .HasMaxLength(255)
                .HasColumnName("regroupement_code");
            entity.Property(e => e.RegroupementLibelle)
                .HasMaxLength(255)
                .HasColumnName("regroupement_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutReleveId).HasColumnName("statut_releve_id");
            entity.Property(e => e.StatutReleveLibelle)
                .HasMaxLength(255)
                .HasColumnName("statut_releve_libelle");
            entity.Property(e => e.TiersPayeurPhysiqueId)
                .HasMaxLength(255)
                .HasColumnName("tiers_payeur_physique_id");
            entity.Property(e => e.TiersPayeurPhysiqueNom)
                .HasMaxLength(255)
                .HasColumnName("tiers_payeur_physique_nom");
            entity.Property(e => e.TourneeId).HasColumnName("tournee_id");
            entity.Property(e => e.TourneeLibelle)
                .HasMaxLength(255)
                .HasColumnName("tournee_libelle");
            entity.Property(e => e.TypeComptageId).HasColumnName("type_comptage_id");
            entity.Property(e => e.TypeComptageLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_comptage_libelle");
            entity.Property(e => e.TypeCompteurId).HasColumnName("type_compteur_id");
            entity.Property(e => e.TypeCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_compteur_libelle");
            entity.Property(e => e.TypeConsommationId).HasColumnName("type_consommation_id");
            entity.Property(e => e.TypeConsommationLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_consommation_libelle");
            entity.Property(e => e.TypeTarifId).HasColumnName("type_tarif_id");
            entity.Property(e => e.TypeTarifLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_tarif_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UsageSecondaireId).HasColumnName("usage_secondaire_id");
            entity.Property(e => e.UsageSecondaireLibelle)
                .HasMaxLength(255)
                .HasColumnName("usage_secondaire_libelle");

            entity.HasOne(d => d.Abonnement).WithMany(p => p.Releves)
                .HasForeignKey(d => d.AbonnementId)
                .HasConstraintName("Releve_abonnement_id_fkey");

            entity.HasOne(d => d.Agent).WithMany(p => p.Releves)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Releve_agent_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Releves)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Releve_client_id_fkey");

            entity.HasOne(d => d.LotRi).WithMany(p => p.Releves)
                .HasForeignKey(d => d.LotRiId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Releve_lot_ri_id_fkey");

            entity.HasOne(d => d.MotifReleve).WithMany(p => p.Releves)
                .HasForeignKey(d => d.MotifReleveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Releve_motif_releve_id_fkey");

            entity.HasOne(d => d.Pdl).WithMany(p => p.Releves)
                .HasForeignKey(d => d.PdlId)
                .HasConstraintName("Releve_pdl_id_fkey");

            entity.HasOne(d => d.Periode).WithMany(p => p.Releves)
                .HasForeignKey(d => d.PeriodeId)
                .HasConstraintName("Releve_periode_id_fkey");

            entity.HasOne(d => d.Pose).WithMany(p => p.Releves)
                .HasForeignKey(d => d.PoseId)
                .HasConstraintName("Releve_pose_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.Releves)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Releve_produit_id_fkey");

            entity.HasOne(d => d.StatutReleve).WithMany(p => p.Releves)
                .HasForeignKey(d => d.StatutReleveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Releve_statut_releve_id_fkey");

            entity.HasOne(d => d.Tournee).WithMany(p => p.Releves)
                .HasForeignKey(d => d.TourneeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Releve_tournee_id_fkey");

            entity.HasOne(d => d.TypeComptage).WithMany(p => p.Releves)
                .HasForeignKey(d => d.TypeComptageId)
                .HasConstraintName("Releve_type_comptage_id_fkey");

            entity.HasOne(d => d.TypeCompteur).WithMany(p => p.Releves)
                .HasForeignKey(d => d.TypeCompteurId)
                .HasConstraintName("Releve_type_compteur_id_fkey");

            entity.HasOne(d => d.TypeConsommation).WithMany(p => p.Releves)
                .HasForeignKey(d => d.TypeConsommationId)
                .HasConstraintName("Releve_type_consommation_id_fkey");

            entity.HasOne(d => d.TypeTarif).WithMany(p => p.Releves)
                .HasForeignKey(d => d.TypeTarifId)
                .HasConstraintName("Releve_type_tarif_id_fkey");
        });

        modelBuilder.Entity<ReleveCa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ReleveCas_pkey");

            entity.ToTable("ReleveCas", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CasCode)
                .HasMaxLength(255)
                .HasColumnName("cas_code");
            entity.Property(e => e.CasId).HasColumnName("cas_id");
            entity.Property(e => e.CasLibelle)
                .HasMaxLength(255)
                .HasColumnName("cas_libelle");
            entity.Property(e => e.Confirme).HasColumnName("confirme");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEnquete).HasColumnName("date_enquete");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.ReleveId).HasColumnName("releve_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Cas).WithMany(p => p.ReleveCas)
                .HasForeignKey(d => d.CasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReleveCas_cas_id_fkey");

            entity.HasOne(d => d.Releve).WithMany(p => p.ReleveCas)
                .HasForeignKey(d => d.ReleveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReleveCas_releve_id_fkey");
        });

        modelBuilder.Entity<Remarque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Remarque_pkey");

            entity.ToTable("Remarque", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<RemiseBanqueCaisse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RemiseBanqueCaisse_pkey");

            entity.ToTable("RemiseBanqueCaisse", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AgentRecepteurFonction)
                .HasMaxLength(255)
                .HasColumnName("agent_recepteur_fonction");
            entity.Property(e => e.AgentRecepteurNom)
                .HasMaxLength(255)
                .HasColumnName("agent_recepteur_nom");
            entity.Property(e => e.AgentRecepteurNumeroPiece)
                .HasMaxLength(255)
                .HasColumnName("agent_recepteur_numero_piece");
            entity.Property(e => e.AgentRecepteurPrenoms)
                .HasMaxLength(255)
                .HasColumnName("agent_recepteur_prenoms");
            entity.Property(e => e.CaisseId)
                .HasMaxLength(255)
                .HasColumnName("caisse_id");
            entity.Property(e => e.Caissier)
                .HasMaxLength(255)
                .HasColumnName("caissier");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateRemise).HasColumnName("date_remise");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MontantCheque).HasColumnName("montant_cheque");
            entity.Property(e => e.MontantEspece).HasColumnName("montant_espece");
            entity.Property(e => e.NombreCheque).HasColumnName("nombre_cheque");
            entity.Property(e => e.NumeroCaisse)
                .HasMaxLength(255)
                .HasColumnName("numero_caisse");
            entity.Property(e => e.NumeroCaisseDestination)
                .HasMaxLength(255)
                .HasColumnName("numero_caisse_destination");
            entity.Property(e => e.PartenairePaiementId).HasColumnName("partenaire_paiement_id");
            entity.Property(e => e.PartenairePaiementLibelle)
                .HasMaxLength(255)
                .HasColumnName("partenaire_paiement_libelle");
            entity.Property(e => e.SoldeCaise).HasColumnName("solde_caise");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeRemiseId).HasColumnName("type_remise_id");
            entity.Property(e => e.TypeRemiseLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_remise_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Caisse).WithMany(p => p.RemiseBanqueCaisses)
                .HasForeignKey(d => d.CaisseId)
                .HasConstraintName("RemiseBanqueCaisse_caisse_id_fkey");

            entity.HasOne(d => d.PartenairePaiement).WithMany(p => p.RemiseBanqueCaisses)
                .HasForeignKey(d => d.PartenairePaiementId)
                .HasConstraintName("RemiseBanqueCaisse_partenaire_paiement_id_fkey");

            entity.HasOne(d => d.TypeRemise).WithMany(p => p.RemiseBanqueCaisses)
                .HasForeignKey(d => d.TypeRemiseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RemiseBanqueCaisse_type_remise_id_fkey");
        });

        modelBuilder.Entity<RemiseBanqueCaisseDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RemiseBanqueCaisseDetail_pkey");

            entity.ToTable("RemiseBanqueCaisseDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.PaiementMoyenUtiliseId).HasColumnName("paiement_moyen_utilise_id");
            entity.Property(e => e.ReferencePaiement)
                .HasMaxLength(255)
                .HasColumnName("reference_paiement");
            entity.Property(e => e.RemiseId).HasColumnName("remise_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.PaiementMoyenUtilise).WithMany(p => p.RemiseBanqueCaisseDetails)
                .HasForeignKey(d => d.PaiementMoyenUtiliseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RemiseBanqueCaisseDetail_paiement_moyen_utilise_id_fkey");

            entity.HasOne(d => d.Remise).WithMany(p => p.RemiseBanqueCaisseDetails)
                .HasForeignKey(d => d.RemiseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RemiseBanqueCaisseDetail_remise_id_fkey");
        });

        modelBuilder.Entity<RoleAgent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RoleAgent_pkey");

            entity.ToTable("RoleAgent", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<RolesPersonnePhysique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RolesPersonnePhysique_pkey");

            entity.ToTable("RolesPersonnePhysique", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<RubriqueFacture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RubriqueFacture_pkey");

            entity.ToTable("RubriqueFacture", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CoutUnitaire)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("cout_unitaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.IsAdditional)
                .HasDefaultValue(false)
                .HasColumnName("is_additional");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.RubriqueFactures)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RubriqueFacture_produit_id_fkey");
        });

        modelBuilder.Entity<RubriqueFraude>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RubriqueFraude_pkey");

            entity.ToTable("RubriqueFraude", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CoutUnitaire)
                .HasDefaultValueSql("'0'::double precision")
                .HasColumnName("cout_unitaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.IsAdditional)
                .HasDefaultValue(false)
                .HasColumnName("is_additional");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.RubriqueFraudes)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RubriqueFraude_produit_id_fkey");
        });

        modelBuilder.Entity<Sapassignment>(entity =>
        {
            entity.HasKey(e => e.Sapassignmentid).HasName("sapassignments_pkey");

            entity.ToTable("sapassignments", "sc_sge");

            entity.Property(e => e.Sapassignmentid).HasColumnName("sapassignmentid");
            entity.Property(e => e.Gxdlmssapassignmentid).HasColumnName("gxdlmssapassignmentid");
            entity.Property(e => e.Sapid).HasColumnName("sapid");
            entity.Property(e => e.Sapname)
                .HasMaxLength(100)
                .HasColumnName("sapname");

            entity.HasOne(d => d.Gxdlmssapassignment).WithMany(p => p.Sapassignments)
                .HasForeignKey(d => d.Gxdlmssapassignmentid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sapassignments_gxdlmssapassignmentid_fkey");
        });

        modelBuilder.Entity<SchemaComptable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SchemaComptable_pkey");

            entity.ToTable("SchemaComptable", "sc_sge");

            entity.HasIndex(e => e.CodeOperationCode, "schema_comptable_code_operation_code");

            entity.HasIndex(e => e.CodeOperationId, "schema_comptable_code_operation_id");

            entity.HasIndex(e => e.CompteCode, "schema_comptable_compte_code");

            entity.HasIndex(e => e.CompteId, "schema_comptable_compte_id");

            entity.HasIndex(e => e.CompteNumero, "schema_comptable_compte_numero");

            entity.HasIndex(e => e.Id, "schema_comptable_id").IsUnique();

            entity.HasIndex(e => e.Status, "schema_comptable_status");

            entity.HasIndex(e => e.TypeClientId, "schema_comptable_type_client_id");

            entity.HasIndex(e => e.TypeDemandeId, "schema_comptable_type_demande_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeOperationCode)
                .HasMaxLength(255)
                .HasColumnName("code_operation_code");
            entity.Property(e => e.CodeOperationId).HasColumnName("code_operation_id");
            entity.Property(e => e.CodeOperationLibelle)
                .HasMaxLength(255)
                .HasColumnName("code_operation_libelle");
            entity.Property(e => e.CompteCode)
                .HasMaxLength(255)
                .HasColumnName("compte_code");
            entity.Property(e => e.CompteId).HasColumnName("compte_id");
            entity.Property(e => e.CompteLibelle)
                .HasMaxLength(255)
                .HasColumnName("compte_libelle");
            entity.Property(e => e.CompteNumero)
                .HasMaxLength(255)
                .HasColumnName("compte_numero");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeClientId).HasColumnName("type_client_id");
            entity.Property(e => e.TypeClientLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_client_libelle");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.TypeDemandeLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_demande_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.CodeOperation).WithMany(p => p.SchemaComptables)
                .HasForeignKey(d => d.CodeOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SchemaComptable_code_operation_id_fkey");

            entity.HasOne(d => d.Compte).WithMany(p => p.SchemaComptables)
                .HasForeignKey(d => d.CompteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SchemaComptable_compte_id_fkey");

            entity.HasOne(d => d.TypeClient).WithMany(p => p.SchemaComptables)
                .HasForeignKey(d => d.TypeClientId)
                .HasConstraintName("SchemaComptable_type_client_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.SchemaComptables)
                .HasForeignKey(d => d.TypeDemandeId)
                .HasConstraintName("SchemaComptable_type_demande_id_fkey");
        });

        modelBuilder.Entity<Script>(entity =>
        {
            entity.HasKey(e => e.Scriptid).HasName("scripts_pkey");

            entity.ToTable("scripts", "sc_sge");

            entity.Property(e => e.Scriptid).HasColumnName("scriptid");
            entity.Property(e => e.Actionindex).HasColumnName("actionindex");
            entity.Property(e => e.Actionln)
                .HasMaxLength(50)
                .HasColumnName("actionln");
            entity.Property(e => e.Actiontype).HasColumnName("actiontype");
            entity.Property(e => e.Gxdlmsscripttableid).HasColumnName("gxdlmsscripttableid");
            entity.Property(e => e.Objecttype).HasColumnName("objecttype");
            entity.Property(e => e.Parameter).HasColumnName("parameter");
            entity.Property(e => e.Parameterdatatype).HasColumnName("parameterdatatype");

            entity.HasOne(d => d.Gxdlmsscripttable).WithMany(p => p.Scripts)
                .HasForeignKey(d => d.Gxdlmsscripttableid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("scripts_gxdlmsscripttableid_fkey");
        });

        modelBuilder.Entity<Seasonprofileactive>(entity =>
        {
            entity.HasKey(e => e.Seasonprofileactiveid).HasName("seasonprofileactive_pkey");

            entity.ToTable("seasonprofileactive", "sc_sge");

            entity.Property(e => e.Seasonprofileactiveid).HasColumnName("seasonprofileactiveid");
            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Start)
                .HasMaxLength(50)
                .HasColumnName("start");
            entity.Property(e => e.Weekname)
                .HasMaxLength(50)
                .HasColumnName("weekname");

            entity.HasOne(d => d.Gxdlmsactivitycalendar).WithMany(p => p.Seasonprofileactives)
                .HasForeignKey(d => d.Gxdlmsactivitycalendarid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("seasonprofileactive_gxdlmsactivitycalendarid_fkey");
        });

        modelBuilder.Entity<Seasonprofilepassive>(entity =>
        {
            entity.HasKey(e => e.Seasonprofilepassiveid).HasName("seasonprofilepassive_pkey");

            entity.ToTable("seasonprofilepassive", "sc_sge");

            entity.Property(e => e.Seasonprofilepassiveid).HasColumnName("seasonprofilepassiveid");
            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Start)
                .HasMaxLength(50)
                .HasColumnName("start");
            entity.Property(e => e.Weekname)
                .HasMaxLength(50)
                .HasColumnName("weekname");

            entity.HasOne(d => d.Gxdlmsactivitycalendar).WithMany(p => p.Seasonprofilepassives)
                .HasForeignKey(d => d.Gxdlmsactivitycalendarid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("seasonprofilepassive_gxdlmsactivitycalendarid_fkey");
        });

        modelBuilder.Entity<Secteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Secteur_pkey");

            entity.ToTable("Secteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.QuartierId).HasColumnName("quartier_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Quartier).WithMany(p => p.Secteurs)
                .HasForeignKey(d => d.QuartierId)
                .HasConstraintName("Secteur_quartier_id_fkey");
        });

        modelBuilder.Entity<SortieMaterielsTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SortieMaterielsTravaux_pkey");

            entity.ToTable("SortieMaterielsTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AdminMatricule)
                .HasMaxLength(255)
                .HasColumnName("admin_matricule");
            entity.Property(e => e.AdminNomComplet)
                .HasMaxLength(255)
                .HasColumnName("admin_nom_complet");
            entity.Property(e => e.AgentFonction)
                .HasMaxLength(255)
                .HasColumnName("agent_fonction");
            entity.Property(e => e.AgentId)
                .HasMaxLength(255)
                .HasColumnName("agent_id");
            entity.Property(e => e.AgentMatricule)
                .HasMaxLength(255)
                .HasColumnName("agent_matricule");
            entity.Property(e => e.AgentNomComplet)
                .HasMaxLength(255)
                .HasColumnName("agent_nom_complet");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateSortie).HasColumnName("date_sortie");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DemandeId)
                .HasMaxLength(255)
                .HasColumnName("demande_id");
            entity.Property(e => e.MagasinId)
                .HasMaxLength(255)
                .HasColumnName("magasin_id");
            entity.Property(e => e.NumeroBs)
                .HasMaxLength(255)
                .HasColumnName("numero_bs");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Agent).WithMany(p => p.SortieMaterielsTravauxes)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SortieMaterielsTravaux_agent_id_fkey");

            entity.HasOne(d => d.Demande).WithMany(p => p.SortieMaterielsTravauxes)
                .HasForeignKey(d => d.DemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SortieMaterielsTravaux_demande_id_fkey");

            entity.HasOne(d => d.Magasin).WithMany(p => p.SortieMaterielsTravauxes)
                .HasForeignKey(d => d.MagasinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SortieMaterielsTravaux_magasin_id_fkey");
        });

        modelBuilder.Entity<SortieMaterielsTravauxDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SortieMaterielsTravauxDetail_pkey");

            entity.ToTable("SortieMaterielsTravauxDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Ecart).HasColumnName("ecart");
            entity.Property(e => e.MaterielCoutUnitaire).HasColumnName("materiel_cout_unitaire");
            entity.Property(e => e.MaterielDevisId).HasColumnName("materiel_devis_id");
            entity.Property(e => e.MaterielLibelle)
                .HasMaxLength(255)
                .HasColumnName("materiel_libelle");
            entity.Property(e => e.QuantiteDemandee).HasColumnName("quantite_demandee");
            entity.Property(e => e.QuantiteSortie).HasColumnName("quantite_sortie");
            entity.Property(e => e.SortieMaterielsId).HasColumnName("sortie_materiels_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.MaterielDevis).WithMany(p => p.SortieMaterielsTravauxDetails)
                .HasForeignKey(d => d.MaterielDevisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SortieMaterielsTravauxDetail_materiel_devis_id_fkey");

            entity.HasOne(d => d.SortieMateriels).WithMany(p => p.SortieMaterielsTravauxDetails)
                .HasForeignKey(d => d.SortieMaterielsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SortieMaterielsTravauxDetail_sortie_materiels_id_fkey");
        });

        modelBuilder.Entity<Specialdayentry>(entity =>
        {
            entity.HasKey(e => e.Entryid).HasName("specialdayentries_pkey");

            entity.ToTable("specialdayentries", "sc_sge");

            entity.Property(e => e.Entryid).HasColumnName("entryid");
            entity.Property(e => e.Dayid).HasColumnName("dayid");
            entity.Property(e => e.Entrydate)
                .HasMaxLength(50)
                .HasColumnName("entrydate");
            entity.Property(e => e.Entryindex).HasColumnName("entryindex");
            entity.Property(e => e.Gxdlmsspecialdaystableid).HasColumnName("gxdlmsspecialdaystableid");

            entity.HasOne(d => d.Gxdlmsspecialdaystable).WithMany(p => p.Specialdayentries)
                .HasForeignKey(d => d.Gxdlmsspecialdaystableid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("specialdayentries_gxdlmsspecialdaystableid_fkey");
        });

        modelBuilder.Entity<StatutAbonnement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutAbonnement_pkey");

            entity.ToTable("StatutAbonnement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutAvoir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutAvoir_pkey");

            entity.ToTable("StatutAvoir", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutBranchement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutBranchement_pkey");

            entity.ToTable("StatutBranchement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutCampagne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutCampagne_pkey");

            entity.ToTable("StatutCampagne", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutCompteur_pkey");

            entity.ToTable("StatutCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutDemande_pkey");

            entity.ToTable("StatutDemande", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutFacture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutFacture_pkey");

            entity.ToTable("StatutFacture", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutFrai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutFrais_pkey");

            entity.ToTable("StatutFrais", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutJuridique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutJuridique_pkey");

            entity.ToTable("StatutJuridique", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutMoratoire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutMoratoire_pkey");

            entity.ToTable("StatutMoratoire", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutPaiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutPaiement_pkey");

            entity.ToTable("StatutPaiement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutReclamation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutReclamation_pkey");

            entity.ToTable("StatutReclamation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutReglement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutReglement_pkey");

            entity.ToTable("StatutReglement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutReleve>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutReleve_pkey");

            entity.ToTable("StatutReleve", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutScelle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutScelle_pkey");

            entity.ToTable("StatutScelle", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutTransfert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutTransfert_pkey");

            entity.ToTable("StatutTransfert", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<StatutTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StatutTravaux_pkey");

            entity.ToTable("StatutTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Step_pkey");

            entity.ToTable("Step");

            entity.HasIndex(e => e.Code, "Step_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "Step_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "step_code_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(false)
                .HasColumnName("active");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Args)
                .HasColumnType("json")
                .HasColumnName("args");
            entity.Property(e => e.AutoStart)
                .HasDefaultValue(false)
                .HasColumnName("auto_start");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Delai)
                .HasComment("delai d'exécution en min")
                .HasColumnName("delai");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FormId).HasColumnName("form_id");
            entity.Property(e => e.IsEnd)
                .HasDefaultValue(false)
                .HasColumnName("is_end");
            entity.Property(e => e.IsStart)
                .HasDefaultValue(false)
                .HasColumnName("is_start");
            entity.Property(e => e.MetaData)
                .HasColumnType("json")
                .HasColumnName("meta_data");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .HasColumnName("path");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeBtn)
                .HasMaxLength(255)
                .HasDefaultValueSql("'primary'::character varying")
                .HasColumnName("type_btn");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Form).WithMany(p => p.Steps)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("Step_form_id_fkey");

            entity.HasOne(d => d.Process).WithMany(p => p.Steps)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("Step_process_id_fkey");
        });

        modelBuilder.Entity<StepsProfile>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ProfileId, e.StepId }).HasName("StepsProfile_pkey");

            entity.ToTable("StepsProfile");

            entity.HasIndex(e => new { e.ProfileId, e.StepId }, "StepsProfile_profile_id_step_id_key").IsUnique();

            entity.HasIndex(e => e.ProfileId, "steps_profile_profile_id");

            entity.HasIndex(e => e.Status, "steps_profile_status");

            entity.HasIndex(e => e.StepId, "steps_profile_step_id");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.ProfileId)
                .HasMaxLength(255)
                .HasColumnName("profile_id");
            entity.Property(e => e.StepId)
                .HasMaxLength(255)
                .HasColumnName("step_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Profile).WithMany(p => p.StepsProfiles)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("StepsProfile_profile_id_fkey");

            entity.HasOne(d => d.Step).WithMany(p => p.StepsProfiles)
                .HasForeignKey(d => d.StepId)
                .HasConstraintName("StepsProfile_step_id_fkey");
        });

        modelBuilder.Entity<StepsTransition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StepsTransition_pkey");

            entity.ToTable("StepsTransition");

            entity.HasIndex(e => e.FromStep, "steps_transition_from_step");

            entity.HasIndex(e => e.Order, "steps_transition_order");

            entity.HasIndex(e => e.Status, "steps_transition_status");

            entity.HasIndex(e => e.ToStep, "steps_transition_to_step");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Conditions).HasColumnName("conditions");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FromStep)
                .HasMaxLength(255)
                .HasColumnName("from_step");
            entity.Property(e => e.Icon)
                .HasMaxLength(255)
                .HasDefaultValueSql("'forward'::character varying")
                .HasColumnName("icon");
            entity.Property(e => e.IsLocked)
                .HasDefaultValue(false)
                .HasColumnName("is_locked");
            entity.Property(e => e.LockedBy)
                .HasMaxLength(255)
                .HasColumnName("locked_by");
            entity.Property(e => e.MajRule).HasColumnName("maj_rule");
            entity.Property(e => e.Order)
                .HasDefaultValue(1)
                .HasColumnName("order");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.ToStep)
                .HasMaxLength(255)
                .HasColumnName("to_step");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.FromStepNavigation).WithMany(p => p.StepsTransitionFromStepNavigations)
                .HasForeignKey(d => d.FromStep)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("StepsTransition_from_step_fkey");

            entity.HasOne(d => d.ToStepNavigation).WithMany(p => p.StepsTransitionToStepNavigations)
                .HasForeignKey(d => d.ToStep)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("StepsTransition_to_step_fkey");
        });

        modelBuilder.Entity<TarifFacturation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TarifFacturation_pkey");

            entity.ToTable("TarifFacturation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CategorieClientId).HasColumnName("categorie_client_id");
            entity.Property(e => e.CategorieClientLibelle)
                .HasMaxLength(255)
                .HasColumnName("categorie_client_libelle");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CentreNom)
                .HasMaxLength(255)
                .HasColumnName("centre_nom");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Commune)
                .HasMaxLength(255)
                .HasColumnName("commune");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebutApplication).HasColumnName("date_debut_application");
            entity.Property(e => e.DateFinApplication).HasColumnName("date_fin_application");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DiametreCompteurId).HasColumnName("diametre_compteur_id");
            entity.Property(e => e.DiametreCompteurLibelle)
                .HasMaxLength(255)
                .HasColumnName("diametre_compteur_libelle");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Montant).HasColumnName("montant");
            entity.Property(e => e.PeriodeDebut)
                .HasMaxLength(255)
                .HasColumnName("periode_debut");
            entity.Property(e => e.PeriodeFin)
                .HasMaxLength(255)
                .HasColumnName("periode_fin");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.ProduitLibelle)
                .HasMaxLength(255)
                .HasColumnName("produit_libelle");
            entity.Property(e => e.RedevanceId).HasColumnName("redevance_id");
            entity.Property(e => e.RedevanceLibelle)
                .HasMaxLength(255)
                .HasColumnName("redevance_libelle");
            entity.Property(e => e.Region)
                .HasMaxLength(255)
                .HasColumnName("region");
            entity.Property(e => e.RegroupementId)
                .HasMaxLength(255)
                .HasColumnName("regroupement_id");
            entity.Property(e => e.RegroupementLibelle)
                .HasMaxLength(255)
                .HasColumnName("regroupement_libelle");
            entity.Property(e => e.Sregion)
                .HasMaxLength(255)
                .HasColumnName("sregion");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TaxeId).HasColumnName("taxe_id");
            entity.Property(e => e.TaxeLibelle)
                .HasMaxLength(255)
                .HasColumnName("taxe_libelle");
            entity.Property(e => e.TypeFacturationId).HasColumnName("type_facturation_id");
            entity.Property(e => e.TypeFacturationLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_facturation_libelle");
            entity.Property(e => e.UniteComptageId).HasColumnName("unite_comptage_id");
            entity.Property(e => e.UniteComptageLibelle)
                .HasMaxLength(255)
                .HasColumnName("unite_comptage_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UsageSecondaireId).HasColumnName("usage_secondaire_id");
            entity.Property(e => e.UsageSecondaireLibelle)
                .HasMaxLength(255)
                .HasColumnName("usage_secondaire_libelle");

            entity.HasOne(d => d.CategorieClient).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.CategorieClientId)
                .HasConstraintName("TarifFacturation_categorie_client_id_fkey");

            entity.HasOne(d => d.DiametreCompteur).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.DiametreCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TarifFacturation_diametre_compteur_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TarifFacturation_produit_id_fkey");

            entity.HasOne(d => d.Redevance).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.RedevanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TarifFacturation_redevance_id_fkey");

            entity.HasOne(d => d.Regroupement).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.RegroupementId)
                .HasConstraintName("TarifFacturation_regroupement_id_fkey");

            entity.HasOne(d => d.Taxe).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.TaxeId)
                .HasConstraintName("TarifFacturation_taxe_id_fkey");

            entity.HasOne(d => d.TypeFacturation).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.TypeFacturationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TarifFacturation_type_facturation_id_fkey");

            entity.HasOne(d => d.UniteComptage).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.UniteComptageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TarifFacturation_unite_comptage_id_fkey");

            entity.HasOne(d => d.UsageSecondaire).WithMany(p => p.TarifFacturations)
                .HasForeignKey(d => d.UsageSecondaireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TarifFacturation_usage_secondaire_id_fkey");
        });

        modelBuilder.Entity<TarifFacturationDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TarifFacturationDetail_pkey");

            entity.ToTable("TarifFacturationDetail", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CodeTranche)
                .HasMaxLength(255)
                .HasColumnName("code_tranche");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.LibelleTranche)
                .HasMaxLength(255)
                .HasColumnName("libelle_tranche");
            entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TarifFacturationId).HasColumnName("tarif_facturation_id");
            entity.Property(e => e.TarifFacturationLibelle)
                .HasMaxLength(255)
                .HasColumnName("tarif_facturation_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.VolumeMax).HasColumnName("volume_max");
            entity.Property(e => e.VolumeMin).HasColumnName("volume_min");

            entity.HasOne(d => d.TarifFacturation).WithMany(p => p.TarifFacturationDetails)
                .HasForeignKey(d => d.TarifFacturationId)
                .HasConstraintName("TarifFacturationDetail_tarif_facturation_id_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Task_pkey");

            entity.ToTable("Task");

            entity.HasIndex(e => e.FromTaskId, "task_from_task_id");

            entity.HasIndex(e => e.FromTaskId, "task_from_task_id_idx");

            entity.HasIndex(e => e.ProcessId, "task_process_id");

            entity.HasIndex(e => e.ProcessId, "task_process_id_idx");

            entity.HasIndex(e => e.ProcessInstance, "task_process_instance");

            entity.HasIndex(e => e.ProcessInstance, "task_process_instance_idx");

            entity.HasIndex(e => e.Status, "task_status");

            entity.HasIndex(e => e.Status, "task_status_idx");

            entity.HasIndex(e => e.StatutNotification, "task_statut_notification_idx");

            entity.HasIndex(e => e.StatutNotificationRelance, "task_statut_notification_relance_idx");

            entity.HasIndex(e => e.StatutNotificationValid, "task_statut_notification_valid_idx");

            entity.HasIndex(e => e.StepId, "task_step_id");

            entity.HasIndex(e => e.StepId, "task_step_id_idx");

            entity.HasIndex(e => e.TaskStatusId, "task_task_status_id");

            entity.HasIndex(e => e.TaskStatusId, "task_task_status_id_idx");

            entity.HasIndex(e => e.TraitementAffectation, "task_traitement_affectation_idx");

            entity.HasIndex(e => e.UserId, "task_user_id");

            entity.HasIndex(e => e.UserId, "task_user_id_idx");

            entity.HasIndex(e => e.UserName, "task_user_name_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AutoStart)
                .HasDefaultValue(false)
                .HasColumnName("auto_start");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DataObject)
                .HasColumnType("json")
                .HasColumnName("data_object");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EndAt).HasColumnName("end_at");
            entity.Property(e => e.FromTaskId)
                .HasMaxLength(255)
                .HasColumnName("from_task_id");
            entity.Property(e => e.FromTaskName)
                .HasMaxLength(255)
                .HasColumnName("from_task_name");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.ProcessInstance).HasColumnName("process_instance");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(255)
                .HasColumnName("process_name");
            entity.Property(e => e.StartAt).HasColumnName("start_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.StatutNotification)
                .HasDefaultValue(0)
                .HasColumnName("statut_notification");
            entity.Property(e => e.StatutNotificationRelance)
                .HasDefaultValue(0)
                .HasColumnName("statut_notification_relance");
            entity.Property(e => e.StatutNotificationValid)
                .HasDefaultValue(0)
                .HasColumnName("statut_notification_valid");
            entity.Property(e => e.StepId)
                .HasMaxLength(255)
                .HasColumnName("step_id");
            entity.Property(e => e.StepName)
                .HasMaxLength(255)
                .HasColumnName("step_name");
            entity.Property(e => e.TaskStatusId).HasColumnName("task_status_id");
            entity.Property(e => e.TraitementAffectation)
                .HasDefaultValue(0)
                .HasColumnName("traitement_affectation");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.FromTask).WithMany(p => p.InverseFromTask)
                .HasForeignKey(d => d.FromTaskId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Task_from_task_id_fkey");

            entity.HasOne(d => d.Step).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_step_id_fkey");

            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_task_status_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Task_user_id_fkey");
        });

        modelBuilder.Entity<TasksStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TasksStatus_pkey");

            entity.ToTable("TasksStatus");

            entity.HasIndex(e => e.Code, "TasksStatus_code_key").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key1").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key10").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key11").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key12").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key13").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key2").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key3").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key4").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key5").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key6").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key7").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key8").IsUnique();

            entity.HasIndex(e => e.Code, "TasksStatus_code_key9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Taxe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Taxe_pkey");

            entity.ToTable("Taxe", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.Valeur)
                .HasMaxLength(255)
                .HasColumnName("valeur");
        });

        modelBuilder.Entity<Tournee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tournee_pkey");

            entity.ToTable("Tournee", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Priorite).HasColumnName("priorite");
            entity.Property(e => e.SecteurId).HasColumnName("secteur_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Secteur).WithMany(p => p.Tournees)
                .HasForeignKey(d => d.SecteurId)
                .HasConstraintName("Tournee_secteur_id_fkey");
        });

        modelBuilder.Entity<TourneeAgent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TourneeAgent_pkey");

            entity.ToTable("TourneeAgent", "sc_sge");

            entity.HasIndex(e => new { e.AgentId, e.TourneeId }, "TourneeAgent_agent_id_tournee_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AgentId)
                .HasMaxLength(255)
                .HasColumnName("agent_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TourneeId).HasColumnName("tournee_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Agent).WithMany(p => p.TourneeAgents)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("TourneeAgent_agent_id_fkey");

            entity.HasOne(d => d.Tournee).WithMany(p => p.TourneeAgents)
                .HasForeignKey(d => d.TourneeId)
                .HasConstraintName("TourneeAgent_tournee_id_fkey");
        });

        modelBuilder.Entity<TypeAccord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeAccord_pkey");

            entity.ToTable("TypeAccord", "sc_sge");

            entity.HasIndex(e => e.Code, "TypeAccord_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeActionDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeActionDemande_pkey");

            entity.ToTable("TypeActionDemande", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeActionEnqueteSatisfaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeActionEnqueteSatisfaction_pkey");

            entity.ToTable("TypeActionEnqueteSatisfaction", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeActionInventaire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeActionInventaire_pkey");

            entity.ToTable("TypeActionInventaire", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeActionRecouvrement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeActionRecouvrement_pkey");

            entity.ToTable("TypeActionRecouvrement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValue(0)
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeActiviteCompteClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeActiviteCompteClient_pkey");

            entity.ToTable("TypeActiviteCompteClient", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValue(0)
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeAvisDevisMetre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeAvisDevisMetre_pkey");

            entity.ToTable("TypeAvisDevisMetre", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeBilletPiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeBilletPiece_pkey");

            entity.ToTable("TypeBilletPiece", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeBilletage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeBilletage_pkey");

            entity.ToTable("TypeBilletage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeBranchement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeBranchement_pkey");

            entity.ToTable("TypeBranchement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeBranchementParProduit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeBranchementParProduit_pkey");

            entity.ToTable("TypeBranchementParProduit", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCas_pkey");

            entity.ToTable("TypeCas", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCaisse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCaisse_pkey");

            entity.ToTable("TypeCaisse", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCampagne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCampagne_pkey");

            entity.ToTable("TypeCampagne", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCarburantVehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCarburantVehicule_pkey");

            entity.ToTable("TypeCarburantVehicule", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCentre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCentre_pkey");

            entity.ToTable("TypeCentre", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeClient_pkey");

            entity.ToTable("TypeClient", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeComptage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeComptage_pkey");

            entity.ToTable("TypeComptage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.AvecPerte)
                .HasDefaultValue(false)
                .HasColumnName("avec_perte");
            entity.Property(e => e.AvecPrimeFixe)
                .HasDefaultValue(false)
                .HasColumnName("avec_prime_fixe");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.PuissanceInstalleeMax).HasColumnName("puissance_installee_max");
            entity.Property(e => e.PuissanceInstalleeMin).HasColumnName("puissance_installee_min");
            entity.Property(e => e.PuissanceSouscriteMax).HasColumnName("puissance_souscrite_max");
            entity.Property(e => e.PuissanceSouscriteMin).HasColumnName("puissance_souscrite_min");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TransfoUnique)
                .HasDefaultValue(false)
                .HasColumnName("transfo_unique");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCompte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCompte_pkey");

            entity.ToTable("TypeCompte", "sc_sge");

            entity.HasIndex(e => e.Code, "type_compte_code");

            entity.HasIndex(e => e.CodeParent, "type_compte_code_parent");

            entity.HasIndex(e => e.Status, "type_compte_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CodeParent)
                .HasMaxLength(255)
                .HasColumnName("code_parent");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCompteRendu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCompteRendu_pkey");

            entity.ToTable("TypeCompteRendu", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCompteur_pkey");

            entity.ToTable("TypeCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.OrdreAffichage)
                .HasMaxLength(255)
                .HasColumnName("ordre_affichage");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCompteurComptage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCompteurComptage_pkey");

            entity.ToTable("TypeCompteurComptage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeComptageId).HasColumnName("type_comptage_id");
            entity.Property(e => e.TypeCompteurId).HasColumnName("type_compteur_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeComptage).WithMany(p => p.TypeCompteurComptages)
                .HasForeignKey(d => d.TypeComptageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeCompteurComptage_type_comptage_id_fkey");

            entity.HasOne(d => d.TypeCompteur).WithMany(p => p.TypeCompteurComptages)
                .HasForeignKey(d => d.TypeCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeCompteurComptage_type_compteur_id_fkey");
        });

        modelBuilder.Entity<TypeConsommation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeConsommation_pkey");

            entity.ToTable("TypeConsommation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeContrat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeContrat_pkey");

            entity.ToTable("TypeContrat", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeControle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeControle_pkey");

            entity.ToTable("TypeControle", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeCoupure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeCoupure_pkey");

            entity.ToTable("TypeCoupure", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeDemande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDemande_pkey");

            entity.ToTable("TypeDemande", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeDemandePiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDemandePiece_pkey");

            entity.ToTable("TypeDemandePiece", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Obligatoire)
                .HasDefaultValue(true)
                .HasColumnName("obligatoire");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeClientId).HasColumnName("type_client_id");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.TypePieceId).HasColumnName("type_piece_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeClient).WithMany(p => p.TypeDemandePieces)
                .HasForeignKey(d => d.TypeClientId)
                .HasConstraintName("TypeDemandePiece_type_client_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.TypeDemandePieces)
                .HasForeignKey(d => d.TypeDemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeDemandePiece_type_demande_id_fkey");

            entity.HasOne(d => d.TypePiece).WithMany(p => p.TypeDemandePieces)
                .HasForeignKey(d => d.TypePieceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeDemandePiece_type_piece_id_fkey");
        });

        modelBuilder.Entity<TypeDemandeProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDemandeProcess_pkey");

            entity.ToTable("TypeDemandeProcess", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(255)
                .HasColumnName("process_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.TypeDemandeProcesses)
                .HasForeignKey(d => d.TypeDemandeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeDemandeProcess_type_demande_id_fkey");
        });

        modelBuilder.Entity<TypeDemandeProduit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDemandeProduit_pkey");

            entity.ToTable("TypeDemandeProduit", "sc_sge");

            entity.HasIndex(e => new { e.ProduitId, e.TypeDemandeId }, "TypeDemandeProduit_produit_id_type_demande_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.TypeDemandeProduits)
                .HasForeignKey(d => d.ProduitId)
                .HasConstraintName("TypeDemandeProduit_produit_id_fkey");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.TypeDemandeProduits)
                .HasForeignKey(d => d.TypeDemandeId)
                .HasConstraintName("TypeDemandeProduit_type_demande_id_fkey");
        });

        modelBuilder.Entity<TypeDemandeSpecification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDemandeSpecification_pkey");

            entity.ToTable("TypeDemandeSpecification", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeDemandeId).HasColumnName("type_demande_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.TypeDemande).WithMany(p => p.TypeDemandeSpecifications)
                .HasForeignKey(d => d.TypeDemandeId)
                .HasConstraintName("TypeDemandeSpecification_type_demande_id_fkey");
        });

        modelBuilder.Entity<TypeDepannage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDepannage_pkey");

            entity.ToTable("TypeDepannage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeDevise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDevise_pkey");

            entity.ToTable("TypeDevise", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TauxConversion).HasColumnName("taux_conversion");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeDisjoncteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDisjoncteur_pkey");

            entity.ToTable("TypeDisjoncteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.PhaseCompteurId).HasColumnName("phase_compteur_id");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.ReglageMax).HasColumnName("reglage_max");
            entity.Property(e => e.ReglageMin).HasColumnName("reglage_min");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.PhaseCompteur).WithMany(p => p.TypeDisjoncteurs)
                .HasForeignKey(d => d.PhaseCompteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeDisjoncteur_phase_compteur_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.TypeDisjoncteurs)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeDisjoncteur_produit_id_fkey");
        });

        modelBuilder.Entity<TypeDisjoncteurParCalibreCompteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDisjoncteurParCalibreCompteur_pkey");

            entity.ToTable("TypeDisjoncteurParCalibreCompteur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeDocument_pkey");

            entity.ToTable("TypeDocument", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Source)
                .HasMaxLength(255)
                .HasColumnName("source");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeFacturation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeFacturation_pkey");

            entity.ToTable("TypeFacturation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeFacturationAbonne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeFacturationAbonne_pkey");

            entity.ToTable("TypeFacturationAbonne", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValue(0)
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeFacture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeFacture_pkey");

            entity.ToTable("TypeFacture", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValue(0)
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeFrai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeFrais_pkey");

            entity.ToTable("TypeFrais", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeFraude>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeFraude_pkey");

            entity.ToTable("TypeFraude", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeInstallation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeInstallation_pkey");

            entity.ToTable("TypeInstallation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValue(0)
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeLienproduit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeLienproduit_pkey");

            entity.ToTable("TypeLienproduit", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeLienredevance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeLienredevance_pkey");

            entity.ToTable("TypeLienredevance", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeLot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeLot_pkey");

            entity.ToTable("TypeLot", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeMagasin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeMagasin_pkey");

            entity.ToTable("TypeMagasin", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeMateriel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeMateriel_pkey");

            entity.ToTable("TypeMateriel", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.TypeMateriels)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TypeMateriel_produit_id_fkey");
        });

        modelBuilder.Entity<TypeMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeMessage_pkey");

            entity.ToTable("TypeMessage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeNotification_pkey");

            entity.ToTable("TypeNotification", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeObjetReglement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeObjetReglement_pkey");

            entity.ToTable("TypeObjetReglement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypePanne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypePanne_pkey");

            entity.ToTable("TypePanne", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypePartenairePaiement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypePartenairePaiement_pkey");

            entity.ToTable("TypePartenairePaiement", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypePiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypePiece_pkey");

            entity.ToTable("TypePiece", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.NaturePieceId).HasColumnName("nature_piece_id");
            entity.Property(e => e.NaturePieceLibelle)
                .HasMaxLength(255)
                .HasColumnName("nature_piece_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.NaturePiece).WithMany(p => p.TypePieces)
                .HasForeignKey(d => d.NaturePieceId)
                .HasConstraintName("TypePiece_nature_piece_id_fkey");
        });

        modelBuilder.Entity<TypeProbleme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeProbleme_pkey");

            entity.ToTable("TypeProbleme", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FraisRetour)
                .HasDefaultValue(0)
                .HasColumnName("frais_retour");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeProvision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeProvision_pkey");

            entity.ToTable("TypeProvision", "sc_sge");

            entity.HasIndex(e => e.Code, "TypeProvision_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeReclamation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeReclamation_pkey");

            entity.ToTable("TypeReclamation", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeRecour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeRecours_pkey");

            entity.ToTable("TypeRecours", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Famille)
                .HasMaxLength(255)
                .HasColumnName("famille");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeRedevance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeRedevance_pkey");

            entity.ToTable("TypeRedevance", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeRefection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeRefection_pkey");

            entity.ToTable("TypeRefection", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CoutUnitaire)
                .HasDefaultValue(0)
                .HasColumnName("cout_unitaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UniteMesure)
                .HasMaxLength(255)
                .HasColumnName("unite_mesure");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.TypeRefections)
                .HasForeignKey(d => d.ProduitId)
                .HasConstraintName("TypeRefection_produit_id_fkey");
        });

        modelBuilder.Entity<TypeRemise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeRemise_pkey");

            entity.ToTable("TypeRemise", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeRemiseBanque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeRemiseBanque_pkey");

            entity.ToTable("TypeRemiseBanque", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeTarif>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTarif_pkey");

            entity.ToTable("TypeTarif", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeTaxe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTaxe_pkey");

            entity.ToTable("TypeTaxe", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeTimbre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTimbre_pkey");

            entity.ToTable("TypeTimbre", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Montant)
                .HasDefaultValue(0)
                .HasColumnName("montant");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeTransformateur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTransformateur_pkey");

            entity.ToTable("TypeTransformateur", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<TypeTravaux>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTravaux_pkey");

            entity.ToTable("TypeTravaux", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CategorieId).HasColumnName("categorie_id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CoutUnitaire)
                .HasDefaultValue(0)
                .HasColumnName("cout_unitaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UniteMesure)
                .HasMaxLength(255)
                .HasColumnName("unite_mesure");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Categorie).WithMany(p => p.TypeTravauxes)
                .HasForeignKey(d => d.CategorieId)
                .HasConstraintName("TypeTravaux_categorie_id_fkey");

            entity.HasOne(d => d.Produit).WithMany(p => p.TypeTravauxes)
                .HasForeignKey(d => d.ProduitId)
                .HasConstraintName("TypeTravaux_produit_id_fkey");
        });

        modelBuilder.Entity<TypeTravauxAdditionnel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTravauxAdditionnel_pkey");

            entity.ToTable("TypeTravauxAdditionnel", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CoutUnitaire)
                .HasDefaultValue(0)
                .HasColumnName("cout_unitaire");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.ProduitId)
                .HasMaxLength(255)
                .HasColumnName("produit_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UniteMesure)
                .HasMaxLength(255)
                .HasColumnName("unite_mesure");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Produit).WithMany(p => p.TypeTravauxAdditionnels)
                .HasForeignKey(d => d.ProduitId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("TypeTravauxAdditionnel_produit_id_fkey");
        });

        modelBuilder.Entity<TypeTuyau>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeTuyau_pkey");

            entity.ToTable("TypeTuyau", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<Typecommande>(entity =>
        {
            entity.HasKey(e => e.Idtype).HasName("typecommande_pkey");

            entity.ToTable("typecommande", "sc_sge");

            entity.Property(e => e.Idtype).HasColumnName("idtype");
            entity.Property(e => e.Libelletype)
                .HasMaxLength(255)
                .HasColumnName("libelletype");
        });

        modelBuilder.Entity<UniteComptage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UniteComptage_pkey");

            entity.ToTable("UniteComptage", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<UsagePrincipal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UsagePrincipal_pkey");

            entity.ToTable("UsagePrincipal", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<UsageSecondaire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UsageSecondaire_pkey");

            entity.ToTable("UsageSecondaire", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CorrespondanceAncienTarif)
                .HasMaxLength(255)
                .HasColumnName("correspondance_ancien_tarif");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.FrequenceFacturationAutorisee).HasColumnName("frequence_facturation_autorisee");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UsagePrincipalId).HasColumnName("usage_principal_id");

            entity.HasOne(d => d.UsagePrincipal).WithMany(p => p.UsageSecondaires)
                .HasForeignKey(d => d.UsagePrincipalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UsageSecondaire_usage_principal_id_fkey");
        });

        modelBuilder.Entity<UsageSecondaireParTypeClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UsageSecondaireParTypeClient_pkey");

            entity.ToTable("UsageSecondaireParTypeClient", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DelaiPaiement)
                .HasMaxLength(255)
                .HasColumnName("delai_paiement");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.LibelleTypeClient)
                .HasMaxLength(255)
                .HasColumnName("libelle_type_client");
            entity.Property(e => e.LibelleUsageSecondaire)
                .HasMaxLength(255)
                .HasColumnName("libelle_usage_secondaire");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeClientId).HasColumnName("type_client_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UsageSecondaireId).HasColumnName("usage_secondaire_id");

            entity.HasOne(d => d.TypeClient).WithMany(p => p.UsageSecondaireParTypeClients)
                .HasForeignKey(d => d.TypeClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UsageSecondaireParTypeClient_type_client_id_fkey");

            entity.HasOne(d => d.UsageSecondaire).WithMany(p => p.UsageSecondaireParTypeClients)
                .HasForeignKey(d => d.UsageSecondaireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UsageSecondaireParTypeClient_usage_secondaire_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "user_email_idx");

            entity.HasIndex(e => e.FirstName, "user_first_name_idx");

            entity.HasIndex(e => e.IsAdmin, "user_is_admin_idx");

            entity.HasIndex(e => e.IsAgent, "user_is_agent_idx");

            entity.HasIndex(e => e.IsRoot, "user_is_root_idx");

            entity.HasIndex(e => e.LastName, "user_last_name_idx");

            entity.HasIndex(e => e.Login, "user_login_idx");

            entity.HasIndex(e => e.Matricule, "user_matricule_idx");

            entity.HasIndex(e => e.NotificationDisabledStatus, "user_notification_disabled_status_idx");

            entity.HasIndex(e => e.NotificationRegisterStatus, "user_notification_register_status_idx");

            entity.HasIndex(e => e.NotificationResetPasswordStatus, "user_notification_reset_password_status_idx");

            entity.HasIndex(e => e.Phone, "user_phone_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CodeOTP)
                .HasMaxLength(255)
                .HasColumnName("code_o_t_p");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.DisabledCounter)
                .HasDefaultValue(0)
                .HasColumnName("disabled_counter");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("is_admin");
            entity.Property(e => e.IsAgent)
                .HasDefaultValue(false)
                .HasColumnName("is_agent");
            entity.Property(e => e.IsRoot)
                .HasDefaultValue(false)
                .HasColumnName("is_root");
            entity.Property(e => e.Lang)
                .HasMaxLength(255)
                .HasDefaultValueSql("'fr'::character varying")
                .HasColumnName("lang");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.Matricule)
                .HasColumnType("character varying")
                .HasColumnName("matricule");
            entity.Property(e => e.NewSession)
                .HasDefaultValue(true)
                .HasColumnName("new_session");
            entity.Property(e => e.NotificationDisabledStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_disabled_status");
            entity.Property(e => e.NotificationRegisterStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_register_status");
            entity.Property(e => e.NotificationResetPasswordStatus)
                .HasDefaultValue(0)
                .HasColumnName("notification_reset_password_status");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.GroupId, e.UserId }).HasName("UserGroup_pkey");

            entity.ToTable("UserGroup");

            entity.HasIndex(e => new { e.GroupId, e.UserId }, "UserGroup_group_id_user_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.GroupId)
                .HasMaxLength(255)
                .HasColumnName("group_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.MetaData)
                .HasMaxLength(255)
                .HasColumnName("meta_data");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Group).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("UserGroup_group_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserGroup_user_id_fkey");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PermissionId }).HasName("UserPermission_pkey");

            entity.ToTable("UserPermission");

            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.PermissionId)
                .HasMaxLength(255)
                .HasColumnName("permission_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("UserPermission_permission_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserPermission_user_id_fkey");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => new { e.ProfileId, e.UserId }).HasName("UserProfile_pkey");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.ProfileId, "user_profile_profile_id");

            entity.HasIndex(e => e.Status, "user_profile_status");

            entity.HasIndex(e => e.UserId, "user_profile_user_id");

            entity.Property(e => e.ProfileId)
                .HasMaxLength(255)
                .HasColumnName("profile_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Profile).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("UserProfile_profile_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserProfile_user_id_fkey");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserSession_pkey");

            entity.ToTable("UserSession");

            entity.HasIndex(e => e.Status, "user_session_status");

            entity.HasIndex(e => e.UserId, "user_session_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Ip)
                .HasMaxLength(255)
                .HasColumnName("ip");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<VariableTarif>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VariableTarif_pkey");

            entity.ToTable("VariableTarif", "sc_sge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CentreId)
                .HasMaxLength(255)
                .HasColumnName("centre_id");
            entity.Property(e => e.CentreNom)
                .HasMaxLength(255)
                .HasColumnName("centre_nom");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CompteComptable)
                .HasMaxLength(255)
                .HasColumnName("compte_comptable");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateApplication).HasColumnName("date_application");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.EstAnalytique)
                .HasDefaultValue(true)
                .HasColumnName("est_analytique");
            entity.Property(e => e.Formule)
                .HasMaxLength(255)
                .HasColumnName("formule");
            entity.Property(e => e.GenerationAnomalie)
                .HasDefaultValue(true)
                .HasColumnName("generation_anomalie");
            entity.Property(e => e.Libelle)
                .HasMaxLength(255)
                .HasColumnName("libelle");
            entity.Property(e => e.LibelleComptable)
                .HasMaxLength(255)
                .HasColumnName("libelle_comptable");
            entity.Property(e => e.ModeApplicationId).HasColumnName("mode_application_id");
            entity.Property(e => e.ModeApplicationLibelle)
                .HasMaxLength(255)
                .HasColumnName("mode_application_libelle");
            entity.Property(e => e.ModeCalculId).HasColumnName("mode_calcul_id");
            entity.Property(e => e.ModeCalculLibelle)
                .HasMaxLength(255)
                .HasColumnName("mode_calcul_libelle");
            entity.Property(e => e.OrdreEdition).HasColumnName("ordre_edition");
            entity.Property(e => e.RechercheTarifId).HasColumnName("recherche_tarif_id");
            entity.Property(e => e.RechercheTarifLibelle)
                .HasMaxLength(255)
                .HasColumnName("recherche_tarif_libelle");
            entity.Property(e => e.RedevanceId).HasColumnName("redevance_id");
            entity.Property(e => e.RedevanceLibelle)
                .HasMaxLength(255)
                .HasColumnName("redevance_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.ModeApplication).WithMany(p => p.VariableTarifs)
                .HasForeignKey(d => d.ModeApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VariableTarif_mode_application_id_fkey");

            entity.HasOne(d => d.ModeCalcul).WithMany(p => p.VariableTarifs)
                .HasForeignKey(d => d.ModeCalculId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VariableTarif_mode_calcul_id_fkey");

            entity.HasOne(d => d.RechercheTarif).WithMany(p => p.VariableTarifs)
                .HasForeignKey(d => d.RechercheTarifId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VariableTarif_recherche_tarif_id_fkey");

            entity.HasOne(d => d.Redevance).WithMany(p => p.VariableTarifs)
                .HasForeignKey(d => d.RedevanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VariableTarif_redevance_id_fkey");
        });

        modelBuilder.Entity<Vehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vehicule_pkey");

            entity.ToTable("Vehicule", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Annee)
                .HasMaxLength(255)
                .HasColumnName("annee");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateMisEncirculation)
                .HasMaxLength(255)
                .HasColumnName("date_mis_encirculation");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Immatriculation)
                .HasMaxLength(255)
                .HasColumnName("immatriculation");
            entity.Property(e => e.MarqueId).HasColumnName("marque_id");
            entity.Property(e => e.MarqueLibelle)
                .HasMaxLength(255)
                .HasColumnName("marque_libelle");
            entity.Property(e => e.ModeleId).HasColumnName("modele_id");
            entity.Property(e => e.ModeleLibelle)
                .HasMaxLength(255)
                .HasColumnName("modele_libelle");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.TypeCarburantId).HasColumnName("type_carburant_id");
            entity.Property(e => e.TypeCarburantLibelle)
                .HasMaxLength(255)
                .HasColumnName("type_carburant_libelle");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.Marque).WithMany(p => p.Vehicules)
                .HasForeignKey(d => d.MarqueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicule_marque_id_fkey");

            entity.HasOne(d => d.Modele).WithMany(p => p.Vehicules)
                .HasForeignKey(d => d.ModeleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicule_modele_id_fkey");

            entity.HasOne(d => d.TypeCarburant).WithMany(p => p.Vehicules)
                .HasForeignKey(d => d.TypeCarburantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicule_type_carburant_id_fkey");
        });

        modelBuilder.Entity<Weekprofiletableactive>(entity =>
        {
            entity.HasKey(e => e.Weekprofileactiveid).HasName("weekprofiletableactive_pkey");

            entity.ToTable("weekprofiletableactive", "sc_sge");

            entity.Property(e => e.Weekprofileactiveid).HasColumnName("weekprofileactiveid");
            entity.Property(e => e.Friday).HasColumnName("friday");
            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");
            entity.Property(e => e.Monday).HasColumnName("monday");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Saturday).HasColumnName("saturday");
            entity.Property(e => e.Sunday).HasColumnName("sunday");
            entity.Property(e => e.Thursday).HasColumnName("thursday");
            entity.Property(e => e.Tuesday).HasColumnName("tuesday");
            entity.Property(e => e.Wednesday).HasColumnName("wednesday");

            entity.HasOne(d => d.Gxdlmsactivitycalendar).WithMany(p => p.Weekprofiletableactives)
                .HasForeignKey(d => d.Gxdlmsactivitycalendarid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("weekprofiletableactive_gxdlmsactivitycalendarid_fkey");
        });

        modelBuilder.Entity<Weekprofiletablepassive>(entity =>
        {
            entity.HasKey(e => e.Weekprofilepassiveid).HasName("weekprofiletablepassive_pkey");

            entity.ToTable("weekprofiletablepassive", "sc_sge");

            entity.Property(e => e.Weekprofilepassiveid).HasColumnName("weekprofilepassiveid");
            entity.Property(e => e.Friday).HasColumnName("friday");
            entity.Property(e => e.Gxdlmsactivitycalendarid).HasColumnName("gxdlmsactivitycalendarid");
            entity.Property(e => e.Monday).HasColumnName("monday");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Saturday).HasColumnName("saturday");
            entity.Property(e => e.Sunday).HasColumnName("sunday");
            entity.Property(e => e.Thursday).HasColumnName("thursday");
            entity.Property(e => e.Tuesday).HasColumnName("tuesday");
            entity.Property(e => e.Wednesday).HasColumnName("wednesday");

            entity.HasOne(d => d.Gxdlmsactivitycalendar).WithMany(p => p.Weekprofiletablepassives)
                .HasForeignKey(d => d.Gxdlmsactivitycalendarid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("weekprofiletablepassive_gxdlmsactivitycalendarid_fkey");
        });

        modelBuilder.Entity<WsCoupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WsCoupon_pkey");

            entity.ToTable("WsCoupon");

            entity.HasIndex(e => e.Name, "WsCoupon_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
        });

        modelBuilder.Entity<WsLocality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WsLocality_pkey");

            entity.ToTable("WsLocality", "sc_sge");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CityId)
                .HasMaxLength(255)
                .HasColumnName("city_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.City).WithMany(p => p.WsLocalities)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("WsLocality_city_id_fkey");
        });

        modelBuilder.Entity<WsLocality1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WsLocality_pkey");

            entity.ToTable("WsLocality");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.CityId)
                .HasMaxLength(255)
                .HasColumnName("city_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");

            entity.HasOne(d => d.City).WithMany(p => p.WsLocality1s)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("WsLocality_city_id_fkey");
        });

        modelBuilder.Entity<WsUserAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WsUserAddress_pkey");

            entity.ToTable("WsUserAddress");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.AddedUser)
                .HasMaxLength(255)
                .HasColumnName("added_user");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address_1");
            entity.Property(e => e.Address2)
                .HasMaxLength(255)
                .HasColumnName("address_2");
            entity.Property(e => e.CityId)
                .HasMaxLength(255)
                .HasColumnName("city_id");
            entity.Property(e => e.Compagny)
                .HasMaxLength(255)
                .HasColumnName("compagny");
            entity.Property(e => e.CountryId)
                .HasMaxLength(255)
                .HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeleteAt).HasColumnName("delete_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.IsMain)
                .HasDefaultValue(false)
                .HasColumnName("is_main");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.LocalityId)
                .HasMaxLength(255)
                .HasColumnName("locality_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(255)
                .HasColumnName("postal_code");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser)
                .HasMaxLength(255)
                .HasColumnName("updated_user");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");

            entity.HasOne(d => d.City).WithMany(p => p.WsUserAddresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WsUserAddress_city_id_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.WsUserAddresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WsUserAddress_country_id_fkey");

            entity.HasOne(d => d.Locality).WithMany(p => p.WsUserAddresses)
                .HasForeignKey(d => d.LocalityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("WsUserAddress_locality_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.WsUserAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("WsUserAddress_user_id_fkey");
        });

        modelBuilder.Entity<Xdlmscontextinfo>(entity =>
        {
            entity.HasKey(e => e.Contextinfoid).HasName("xdlmscontextinfo_pkey");

            entity.ToTable("xdlmscontextinfo", "sc_sge");

            entity.Property(e => e.Contextinfoid).HasColumnName("contextinfoid");
            entity.Property(e => e.Associationid).HasColumnName("associationid");
            entity.Property(e => e.Conformance).HasColumnName("conformance");
            entity.Property(e => e.Cypheringinfo).HasColumnName("cypheringinfo");
            entity.Property(e => e.Dlmsversionnumber).HasColumnName("dlmsversionnumber");
            entity.Property(e => e.Maxreceivepdusize).HasColumnName("maxreceivepdusize");
            entity.Property(e => e.Maxsendpdusize).HasColumnName("maxsendpdusize");
            entity.Property(e => e.Qualityofservice).HasColumnName("qualityofservice");

            entity.HasOne(d => d.Association).WithMany(p => p.Xdlmscontextinfos)
                .HasForeignKey(d => d.Associationid)
                .HasConstraintName("fk_context_info");
        });
        modelBuilder.HasSequence("Form_id_seq");
        modelBuilder.HasSequence("MotifImpossibilite_id_seq", "sc_sge").StartsAt(5L);
        modelBuilder.HasSequence("MoyenPaiement_id_seq", "sc_sge");
        modelBuilder.HasSequence("NatureBranchement_id_seq", "sc_sge");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
