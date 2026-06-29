using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionMateriel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemIssue_Has_IsItemUsable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_item_usable",
                table: "item_issues",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_item_usable",
                table: "item_issues");
        }
    }
}
