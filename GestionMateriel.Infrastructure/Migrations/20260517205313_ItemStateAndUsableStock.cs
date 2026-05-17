using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionMateriel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemStateAndUsableStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "items",
                type: "character varying(3)",
                unicode: false,
                maxLength: 3,
                nullable: false,
                defaultValue: "OK");

            migrationBuilder.AddColumn<int>(
                name: "usable_stock",
                table: "items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "items");

            migrationBuilder.DropColumn(
                name: "usable_stock",
                table: "items");
        }
    }
}
