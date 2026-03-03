using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLMS_DAL.Migrations
{
    /// <inheritdoc />
    public partial class EquipementLinkToCellule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipement_Poste_PosteId",
                table: "Equipement");

            migrationBuilder.RenameColumn(
                name: "PosteId",
                table: "Equipement",
                newName: "CelluleId");

            migrationBuilder.RenameIndex(
                name: "IX_Equipement_PosteId",
                table: "Equipement",
                newName: "IX_Equipement_CelluleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipement_Cellule_CelluleId",
                table: "Equipement",
                column: "CelluleId",
                principalTable: "Cellule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipement_Cellule_CelluleId",
                table: "Equipement");

            migrationBuilder.RenameColumn(
                name: "CelluleId",
                table: "Equipement",
                newName: "PosteId");

            migrationBuilder.RenameIndex(
                name: "IX_Equipement_CelluleId",
                table: "Equipement",
                newName: "IX_Equipement_PosteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipement_Poste_PosteId",
                table: "Equipement",
                column: "PosteId",
                principalTable: "Poste",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
