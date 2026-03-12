using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLMS_DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiPassReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // === ReadingCycle ===
            migrationBuilder.CreateTable(
                name: "ReadingCycle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    TotalCompteurs = table.Column<int>(type: "int", nullable: false),
                    CompteursLus = table.Column<int>(type: "int", nullable: false),
                    MaxSessions = table.Column<int>(type: "int", nullable: false),
                    SessionActuelle = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingCycle", x => x.Id);
                });

            // === ReadingSession ===
            migrationBuilder.CreateTable(
                name: "ReadingSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingCycleId = table.Column<int>(type: "int", nullable: false),
                    NumeroSession = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    CompteursEnScope = table.Column<int>(type: "int", nullable: false),
                    CompteursLus = table.Column<int>(type: "int", nullable: false),
                    CompteursEchoues = table.Column<int>(type: "int", nullable: false),
                    DureeMs = table.Column<long>(type: "bigint", nullable: false),
                    DureeLectureMs = table.Column<long>(type: "bigint", nullable: false),
                    DureePauseMs = table.Column<long>(type: "bigint", nullable: false),
                    NombreIps = table.Column<int>(type: "int", nullable: false),
                    IpsAccessibles = table.Column<int>(type: "int", nullable: false),
                    TauxReussite = table.Column<double>(type: "float", nullable: false),
                    DebitCompteursParMinute = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingSession_ReadingCycle_ReadingCycleId",
                        column: x => x.ReadingCycleId,
                        principalTable: "ReadingCycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // === SessionPassResult ===
            migrationBuilder.CreateTable(
                name: "SessionPassResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingSessionId = table.Column<int>(type: "int", nullable: false),
                    NumeroPasse = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BudgetSeconds = table.Column<int>(type: "int", nullable: false),
                    CompteursEnScope = table.Column<int>(type: "int", nullable: false),
                    CompteursLus = table.Column<int>(type: "int", nullable: false),
                    CompteursEchoues = table.Column<int>(type: "int", nullable: false),
                    CompteursDifferes = table.Column<int>(type: "int", nullable: false),
                    IpsDifferees = table.Column<int>(type: "int", nullable: false),
                    DureeMs = table.Column<long>(type: "bigint", nullable: false),
                    CanaryTimeoutSeconds = table.Column<int>(type: "int", nullable: false),
                    CachedTimeoutSeconds = table.Column<int>(type: "int", nullable: false),
                    UncachedTimeoutSeconds = table.Column<int>(type: "int", nullable: false),
                    MaxConsecutiveFailures = table.Column<int>(type: "int", nullable: false),
                    CooldownCount = table.Column<int>(type: "int", nullable: false),
                    CooldownSeconds = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionPassResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionPassResult_ReadingSession_ReadingSessionId",
                        column: x => x.ReadingSessionId,
                        principalTable: "ReadingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // === MeterReadingStatus ===
            migrationBuilder.CreateTable(
                name: "MeterReadingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingSessionId = table.Column<int>(type: "int", nullable: false),
                    CompteurEquipementId = table.Column<int>(type: "int", nullable: false),
                    NumeroPasse = table.Column<int>(type: "int", nullable: false),
                    Resultat = table.Column<int>(type: "int", nullable: false),
                    MessageErreur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresseIp = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroCompteur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempsHdlcMs = table.Column<long>(type: "bigint", nullable: true),
                    TempsLectureMs = table.Column<long>(type: "bigint", nullable: true),
                    TempsClesMs = table.Column<long>(type: "bigint", nullable: true),
                    TempsTotalMs = table.Column<long>(type: "bigint", nullable: true),
                    TimeoutApplique = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadingStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReadingStatus_ReadingSession_ReadingSessionId",
                        column: x => x.ReadingSessionId,
                        principalTable: "ReadingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeterReadingStatus_CompteurEquipement_CompteurEquipementId",
                        column: x => x.CompteurEquipementId,
                        principalTable: "CompteurEquipement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // === IpSessionStats ===
            migrationBuilder.CreateTable(
                name: "IpSessionStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingSessionId = table.Column<int>(type: "int", nullable: false),
                    AdresseIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalTentatives = table.Column<int>(type: "int", nullable: false),
                    Reussites = table.Column<int>(type: "int", nullable: false),
                    Echecs = table.Column<int>(type: "int", nullable: false),
                    TauxReussite = table.Column<double>(type: "float", nullable: false),
                    LatenceMoyenneMs = table.Column<double>(type: "float", nullable: false),
                    TcpAccessible = table.Column<bool>(type: "bit", nullable: false),
                    TcpLatenceMs = table.Column<long>(type: "bigint", nullable: false),
                    CanaryReussi = table.Column<bool>(type: "bit", nullable: false),
                    EchecsConsecutifsMax = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpSessionStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IpSessionStats_ReadingSession_ReadingSessionId",
                        column: x => x.ReadingSessionId,
                        principalTable: "ReadingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // === ReadingConfiguration ===
            migrationBuilder.CreateTable(
                name: "ReadingConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cle = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Valeur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupeConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingConfiguration", x => x.Id);
                });

            // === Indexes ===
            migrationBuilder.CreateIndex(
                name: "IX_ReadingSession_ReadingCycleId",
                table: "ReadingSession",
                column: "ReadingCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionPassResult_ReadingSessionId",
                table: "SessionPassResult",
                column: "ReadingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadingStatus_ReadingSessionId_Resultat",
                table: "MeterReadingStatus",
                columns: new[] { "ReadingSessionId", "Resultat" });

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadingStatus_ReadingSessionId_CompteurEquipementId",
                table: "MeterReadingStatus",
                columns: new[] { "ReadingSessionId", "CompteurEquipementId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadingStatus_CompteurEquipementId",
                table: "MeterReadingStatus",
                column: "CompteurEquipementId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadingStatus_AdresseIp",
                table: "MeterReadingStatus",
                column: "AdresseIp");

            migrationBuilder.CreateIndex(
                name: "IX_IpSessionStats_ReadingSessionId",
                table: "IpSessionStats",
                column: "ReadingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingConfiguration_Cle",
                table: "ReadingConfiguration",
                column: "Cle",
                unique: true);

            // === Seed ReadingConfiguration ===
            migrationBuilder.InsertData(
                table: "ReadingConfiguration",
                columns: new[] { "Cle", "Valeur", "Description", "GroupeConfig", "CreatedBy", "UpdatedBy", "DeletedBy", "IsArchive" },
                values: new object[,]
                {
                    { "cycle.max_sessions", "10", "Nombre max de sessions par cycle", "cycle", "system", "", "", false },
                    { "session.global_ceiling_seconds", "3600", "Duree max globale d'une session (secondes)", "session", "system", "", "", false },
                    { "session.max_concurrent_ips", "8", "Nombre max d'IPs traitees en parallele", "session", "system", "", "", false },
                    { "pass1.budget_seconds", "1200", "Budget temps passe 1", "pass1", "system", "", "", false },
                    { "pass1.canary_timeout_seconds", "180", "Timeout canary passe 1", "pass1", "system", "", "", false },
                    { "pass1.cached_timeout_seconds", "180", "Timeout compteur cache passe 1", "pass1", "system", "", "", false },
                    { "pass1.uncached_timeout_seconds", "300", "Timeout compteur non-cache passe 1", "pass1", "system", "", "", false },
                    { "pass1.max_consecutive_failures", "2", "Echecs consecutifs max passe 1", "pass1", "system", "", "", false },
                    { "pass1.cooldown_count", "0", "Nombre de cooldowns passe 1", "pass1", "system", "", "", false },
                    { "pass1.cooldown_seconds", "0", "Duree cooldown passe 1", "pass1", "system", "", "", false },
                    { "pass1.pause_after_seconds", "300", "Pause apres passe 1", "pass1", "system", "", "", false },
                    { "pass2.budget_seconds", "900", "Budget temps passe 2", "pass2", "system", "", "", false },
                    { "pass2.canary_timeout_seconds", "300", "Timeout canary passe 2", "pass2", "system", "", "", false },
                    { "pass2.cached_timeout_seconds", "240", "Timeout compteur cache passe 2", "pass2", "system", "", "", false },
                    { "pass2.uncached_timeout_seconds", "360", "Timeout compteur non-cache passe 2", "pass2", "system", "", "", false },
                    { "pass2.max_consecutive_failures", "3", "Echecs consecutifs max passe 2", "pass2", "system", "", "", false },
                    { "pass2.cooldown_count", "1", "Nombre de cooldowns passe 2", "pass2", "system", "", "", false },
                    { "pass2.cooldown_seconds", "15", "Duree cooldown passe 2", "pass2", "system", "", "", false },
                    { "pass2.pause_after_seconds", "300", "Pause apres passe 2", "pass2", "system", "", "", false },
                    { "pass3.budget_seconds", "600", "Budget temps passe 3", "pass3", "system", "", "", false },
                    { "pass3.canary_timeout_seconds", "420", "Timeout canary passe 3", "pass3", "system", "", "", false },
                    { "pass3.cached_timeout_seconds", "300", "Timeout compteur cache passe 3", "pass3", "system", "", "", false },
                    { "pass3.uncached_timeout_seconds", "420", "Timeout compteur non-cache passe 3", "pass3", "system", "", "", false },
                    { "pass3.max_consecutive_failures", "5", "Echecs consecutifs max passe 3", "pass3", "system", "", "", false },
                    { "pass3.cooldown_count", "1", "Nombre de cooldowns passe 3", "pass3", "system", "", "", false },
                    { "pass3.cooldown_seconds", "30", "Duree cooldown passe 3", "pass3", "system", "", "", false },
                    { "pass3.pause_after_seconds", "0", "Pause apres passe 3", "pass3", "system", "", "", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ReadingConfiguration");
            migrationBuilder.DropTable(name: "IpSessionStats");
            migrationBuilder.DropTable(name: "MeterReadingStatus");
            migrationBuilder.DropTable(name: "SessionPassResult");
            migrationBuilder.DropTable(name: "ReadingSession");
            migrationBuilder.DropTable(name: "ReadingCycle");
        }
    }
}
