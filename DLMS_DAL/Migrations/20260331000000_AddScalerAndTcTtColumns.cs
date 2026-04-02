using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLMS_DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddScalerAndTcTtColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // === Colonnes scaler sur Gxdlmsprofilgenericdetails ===
            migrationBuilder.AddColumn<string>(
                name: "Unite",
                table: "Gxdlmsprofilgenericdetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Exposant",
                table: "Gxdlmsprofilgenericdetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ScalerGurux",
                table: "Gxdlmsprofilgenericdetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Gxdlmsprofilgenericdetails",
                type: "datetime2",
                nullable: true);

            // === Colonnes RawValue si absente (ajoutée par script SQL) ===
            // RawValue est défini dans l'entité mais absent du snapshot EF.
            // On l'ajoute conditionnellement pour régulariser.
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Gxdlmsprofilgenericdetails' AND COLUMN_NAME = 'RawValue'
                )
                BEGIN
                    ALTER TABLE Gxdlmsprofilgenericdetails ADD RawValue nvarchar(max) NULL;
                END
            ");

            // === Colonnes TC/TT sur Compteur ===
            migrationBuilder.AddColumn<double>(
                name: "RapportTC",
                table: "Compteur",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RapportTT",
                table: "Compteur",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TCNumerateur",
                table: "Compteur",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TCDenominateur",
                table: "Compteur",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TTNumerateur",
                table: "Compteur",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TTDenominateur",
                table: "Compteur",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Gxdlmsprofilgenericdetails
            migrationBuilder.DropColumn(name: "Unite", table: "Gxdlmsprofilgenericdetails");
            migrationBuilder.DropColumn(name: "Exposant", table: "Gxdlmsprofilgenericdetails");
            migrationBuilder.DropColumn(name: "ScalerGurux", table: "Gxdlmsprofilgenericdetails");

            // Compteur
            migrationBuilder.DropColumn(name: "RapportTC", table: "Compteur");
            migrationBuilder.DropColumn(name: "RapportTT", table: "Compteur");
            migrationBuilder.DropColumn(name: "TCNumerateur", table: "Compteur");
            migrationBuilder.DropColumn(name: "TCDenominateur", table: "Compteur");
            migrationBuilder.DropColumn(name: "TTNumerateur", table: "Compteur");
            migrationBuilder.DropColumn(name: "TTDenominateur", table: "Compteur");
        }
    }
}
