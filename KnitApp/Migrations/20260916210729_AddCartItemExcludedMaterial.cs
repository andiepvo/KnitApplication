using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnitApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItemExcludedMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItemExcludedMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CartItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemExcludedMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItemExcludedMaterials_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemExcludedMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemExcludedMaterials_CartItemId",
                table: "CartItemExcludedMaterials",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemExcludedMaterials_MaterialId",
                table: "CartItemExcludedMaterials",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemExcludedMaterials");
        }
    }
}
