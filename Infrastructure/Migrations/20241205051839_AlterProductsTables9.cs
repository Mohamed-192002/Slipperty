using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTables9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariableCombination_Products_ProductId",
                table: "VariableCombination");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariableCombination",
                table: "VariableCombination");

            migrationBuilder.RenameTable(
                name: "VariableCombination",
                newName: "VariableCombinations");

            migrationBuilder.RenameIndex(
                name: "IX_VariableCombination_ProductId",
                table: "VariableCombinations",
                newName: "IX_VariableCombinations_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariableCombinations",
                table: "VariableCombinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableCombinations_Products_ProductId",
                table: "VariableCombinations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariableCombinations_Products_ProductId",
                table: "VariableCombinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariableCombinations",
                table: "VariableCombinations");

            migrationBuilder.RenameTable(
                name: "VariableCombinations",
                newName: "VariableCombination");

            migrationBuilder.RenameIndex(
                name: "IX_VariableCombinations_ProductId",
                table: "VariableCombination",
                newName: "IX_VariableCombination_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariableCombination",
                table: "VariableCombination",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableCombination_Products_ProductId",
                table: "VariableCombination",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
