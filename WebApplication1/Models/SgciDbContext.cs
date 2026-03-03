using System;
using System.Collections.Generic;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public partial class SgciDbContext : DbContext
{
    public SgciDbContext()
    {
    }

    public SgciDbContext(DbContextOptions<SgciDbContext> options)
        : base(options)
    {
    }

   

    public virtual DbSet<Actionoverthreshold> Actionoverthresholds { get; set; }

    public virtual DbSet<Actionunderthreshold> Actionunderthresholds { get; set; }

    public virtual DbSet<Applicationcontext> Applicationcontexts { get; set; }

    public virtual DbSet<Authenticationmechanism> Authenticationmechanisms { get; set; }

    public virtual DbSet<CodeObi> CodeObis { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<Commandecompteur> Commandecompteurs { get; set; }

    public virtual DbSet<Compteur> Compteurs { get; set; }

    public virtual DbSet<Dayactionspassive> Dayactionspassives { get; set; }

    public virtual DbSet<Dayprofiletableactive> Dayprofiletableactives { get; set; }

    public virtual DbSet<Dayprofiletablepassive> Dayprofiletablepassives { get; set; }

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

    public virtual DbSet<Objects> Objects { get; set; }

    public virtual DbSet<Objectrelation> Objectrelations { get; set; }

    public virtual DbSet<Typecommande> Typecommandes { get; set; }

    public virtual DbSet<Weekprofiletableactive> Weekprofiletableactives { get; set; }

    public virtual DbSet<Weekprofiletablepassive> Weekprofiletablepassives { get; set; }

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

        modelBuilder.Entity<Objects>(entity =>
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

        modelBuilder.Entity<Typecommande>(entity =>
        {
            entity.HasKey(e => e.Idtype).HasName("typecommande_pkey");

            entity.ToTable("typecommande", "sc_sge");

            entity.Property(e => e.Idtype).HasColumnName("idtype");
            entity.Property(e => e.Libelletype)
                .HasMaxLength(255)
                .HasColumnName("libelletype");
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
