using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnitApp.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructionsPdfPathToPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructionsPdf",
                table: "Patterns",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructionsPdf",
                table: "Patterns");
        }
    }
}
