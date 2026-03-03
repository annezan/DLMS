using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLMS_DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCelluleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Cellule");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Cellule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValeurTension",
                table: "Cellule",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cellule");

            migrationBuilder.DropColumn(
                name: "ValeurTension",
                table: "Cellule");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Cellule",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
