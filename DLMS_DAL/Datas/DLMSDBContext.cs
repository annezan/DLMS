using DLMS_MODELS;
using DLMS_MODELS.AlarmsDomain.Entities;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using DLMS_MODELS.EquipementDomain.Entities;
using DLMS_MODELS.ErrorDomain.Entities;
using DLMS_MODELS.EventsDomain.Entities;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.PosteDomain.Entities;
using DLMS_MODELS.ReadingDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;
using DLMS_MODELS.TypecommandeDomain.Entities;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.Datas;

public partial class DLMSDBContext : DbContext
{
    public DLMSDBContext()
    {
    }
    public string serveur;
    public string Port;
    public string DB;
    public DLMSDBContext(DbContextOptions<DLMSDBContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Alarms> Alarms { get; set; }

    public virtual DbSet<AssociationKey> AssociationKeys { get; set; }

    public virtual DbSet<CodeObis> CodeObis { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<CommandeCompteur> CommandeCompteur { get; set; }

    public virtual DbSet<ResultatCommandeCompteur> ResultatCommandeCompteurs { get; set; }

    public virtual DbSet<Compteur> Compteur { get; set; }
    public virtual DbSet<Equipement> Equipement { get; set; }
    public virtual DbSet<Events> Events { get; set; }
    public virtual DbSet<Error> Error { get; set; }

    public virtual DbSet<CompteurEquipement> CompteurEquipement { get; set; }
    public virtual DbSet<CompteurCellule> CompteurCellule { get; set; }
    public virtual DbSet<Fabricant> Fabricants { get; set; }

    public virtual DbSet<Gxdlmsprofilgeneric> Gxdlmsprofilgenerics { get; set; }

    public virtual DbSet<Gxdlmsprofilgenericdetail> Gxdlmsprofilgenericdetails { get; set; }

    public virtual DbSet<Gxdlmsprofilgenericdetailsevent> Gxdlmsprofilgenericdetailsevents { get; set; }

    public virtual DbSet<Typecommande> Typecommandes { get; set; }
    public virtual DbSet<Poste> Poste { get; set; }
    public virtual DbSet<Cellule> Cellule { get; set; }

    #region Gestion des utilisateurs, rôles et permissions
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<TokenUser> TokenUsers { get; set; }
    #endregion

    #region Multi-Pass Reading
    public virtual DbSet<ReadingCycle> ReadingCycles { get; set; }
    public virtual DbSet<ReadingSession> ReadingSessions { get; set; }
    public virtual DbSet<SessionPassResult> SessionPassResults { get; set; }
    public virtual DbSet<MeterReadingStatus> MeterReadingStatuses { get; set; }
    public virtual DbSet<IpSessionStats> IpSessionStats { get; set; }
    public virtual DbSet<ReadingConfiguration> ReadingConfigurations { get; set; }
    public virtual DbSet<MeterProfileReadHistory> MeterProfileReadHistories { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compteur>(entity =>
        {
            entity.HasOne(pt => pt.Fabriquant)
                    .WithMany(p => p.Compteurs)
                    .HasForeignKey(pt => pt.FabriquantId);
        });

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasOne(pt => pt.Typecommande)
                    .WithMany(p => p.Commandes)
                    .HasForeignKey(pt => pt.TypecommandeId);
        });

        // Configuration de la relation one-to-many entre Poste et Cellule
        modelBuilder.Entity<Cellule>(entity =>
        {
            entity.HasOne(pt => pt.Poste)
                    .WithMany(p => p.Cellules)
                    .HasForeignKey(pt => pt.PosteId);
        });

        modelBuilder.Entity<CompteurEquipement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(t => new { t.CompteurId, t.EquipementId });
            entity.ToTable("CompteurEquipement");

            entity.HasOne(pt => pt.Compteur)
                    .WithMany(p => p.CompteurEquipement)
                    .HasForeignKey(pt => pt.CompteurId);

            entity.HasOne(pt => pt.Equipement)
                    .WithMany(p => p.EquipementCompteur)
                    .HasForeignKey(pt => pt.EquipementId);
        });

        // Configuration de la relation many-to-many entre Compteur et Cellule via CompteurCellule
        modelBuilder.Entity<CompteurCellule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(t => new { t.CompteurId, t.CelluleId });
            entity.ToTable("CompteurCellule");

            entity.HasOne(pt => pt.Compteur)
                    .WithMany(p => p.CompteurCellules)
                    .HasForeignKey(pt => pt.CompteurId);

            entity.HasOne(pt => pt.Cellule)
                    .WithMany(p => p.CelluleCompteurs)
                    .HasForeignKey(pt => pt.CelluleId);
        });

        modelBuilder.Entity<CommandeCompteur>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(t => new { t.CompteurId, t.CommandeId });
            entity.ToTable("CommandeCompteur");

            entity.HasOne(pt => pt.Compteur)
                    .WithMany(p => p.CommandeCompteur)
                    .HasForeignKey(pt => pt.CompteurId);

            entity.HasOne(pt => pt.Commande)
                    .WithMany(p => p.CommandeCompteur)
                    .HasForeignKey(pt => pt.CommandeId);
        });

        modelBuilder.Entity<ResultatCommandeCompteur>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(t => new { t.CommandeCompteurId, t.CodeObisId });
            entity.ToTable("ResultatCommandeCompteur");

            entity.HasOne(pt => pt.CommandeCompteur)
                    .WithMany(p => p.ResultatCommandeCompteurs)
                    .HasForeignKey(pt => pt.CommandeCompteurId);

            entity.HasOne(pt => pt.CodeObis)
                    .WithMany()
                    .HasForeignKey(pt => pt.CodeObisId);

            entity.HasOne(pt => pt.Gxdlmsprofilgeneric)
                    .WithMany()
                    .HasForeignKey(pt => pt.GxdlmsprofilgenericId);
        });

        modelBuilder.Entity<Gxdlmsprofilgeneric>(entity =>
        {
            entity.HasOne(pt => pt.Codeobis)
                    .WithOne(p => p.Gxdlmsprofilgenerics)
                    .HasForeignKey<Gxdlmsprofilgeneric>(pt => pt.CodeObisId);
        });

        modelBuilder.Entity<Gxdlmsprofilgenericdetail>(entity =>
        {
            entity.HasOne(pt => pt.Codeobis)
                    .WithMany(p => p.Gxdlmsprofilgenericdetails)
                    .HasForeignKey(pt => pt.CodeObisId);
        });

        modelBuilder.Entity<Gxdlmsprofilgenericdetailsevent>(entity =>
        {
            entity.HasOne(pt => pt.Codeobis)
                    .WithMany(p => p.Gxdlmsprofilgenericdetailsevents)
                    .HasForeignKey(pt => pt.CodeObisId);

            entity.HasOne(pt => pt.Event)
                    .WithMany(p => p.Gxdlmsprofilgenericdetailsevents)
                    .HasForeignKey(pt => pt.EventId);
        });

        #region Configuration Multi-Pass Reading

        modelBuilder.Entity<ReadingCycle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ReadingCycle");
        });

        modelBuilder.Entity<ReadingSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ReadingSession");

            entity.HasOne(e => e.ReadingCycle)
                  .WithMany(c => c.Sessions)
                  .HasForeignKey(e => e.ReadingCycleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SessionPassResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("SessionPassResult");

            entity.HasOne(e => e.ReadingSession)
                  .WithMany(s => s.Passes)
                  .HasForeignKey(e => e.ReadingSessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MeterReadingStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("MeterReadingStatus");

            entity.HasOne(e => e.ReadingSession)
                  .WithMany(s => s.MeterReadings)
                  .HasForeignKey(e => e.ReadingSessionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CompteurEquipement)
                  .WithMany()
                  .HasForeignKey(e => e.CompteurEquipementId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ReadingSessionId, e.Resultat });
            entity.HasIndex(e => new { e.ReadingSessionId, e.CompteurEquipementId });
            entity.HasIndex(e => e.CompteurEquipementId);
            entity.HasIndex(e => e.AdresseIp);
        });

        modelBuilder.Entity<IpSessionStats>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("IpSessionStats");

            entity.HasOne(e => e.ReadingSession)
                  .WithMany(s => s.IpStats)
                  .HasForeignKey(e => e.ReadingSessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReadingConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ReadingConfiguration");
            entity.HasIndex(e => e.Cle).IsUnique();
        });

        modelBuilder.Entity<MeterProfileReadHistory>(entity =>
        {
            entity.ToTable("MeterProfileReadHistory");
            entity.HasIndex(e => new { e.CompteurSerial, e.ProfileObis }).IsUnique();
            entity.HasIndex(e => e.CompteurSerial);
        });

        #endregion

        #region Configuration des relations Users - Roles - Permissions
        
        // Configuration de la relation User - Role (un utilisateur a un seul rôle)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(u => u.Role)
                  .WithMany()
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Configuration de la relation User - Poste (pour les utilisateurs avec poste assigné)
            entity.HasOne(u => u.Poste)
                  .WithMany(p => p.Users)
                  .HasForeignKey(u => u.PosteId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuration de la relation Role - Permission via RolePermission
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(rp => rp.Id);
            
            entity.HasOne(rp => rp.Roles)
                  .WithMany(r => r.RolePermissions)
                  .HasForeignKey(rp => rp.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permissions)
                  .WithMany(p => p.RolePermissions)
                  .HasForeignKey(rp => rp.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Index pour éviter les doublons
            entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                  .IsUnique();
        });

        #endregion
    }
}
