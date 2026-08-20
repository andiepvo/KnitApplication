using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnitApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCraftTypeToPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CraftType",
                table: "Patterns",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CraftType",
                table: "Patterns");
        }
    }
}
