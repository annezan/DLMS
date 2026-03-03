using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLMS_DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompteurLinkToCellule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompteurCellule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompteurId = table.Column<int>(type: "int", nullable: false),
                    CelluleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompteurCellule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompteurCellule_Cellule_CelluleId",
                        column: x => x.CelluleId,
                        principalTable: "Cellule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompteurCellule_Compteur_CompteurId",
                        column: x => x.CompteurId,
                        principalTable: "Compteur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompteurCellule_CompteurId_CelluleId",
                table: "CompteurCellule",
                columns: new[] { "CompteurId", "CelluleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompteurCellule");
        }
    }
}
