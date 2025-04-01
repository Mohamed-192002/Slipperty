using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTable7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariableValue_Products_ProductId",
                table: "VariableValue");

            migrationBuilder.DropIndex(
                name: "IX_VariableValue_ProductId",
                table: "VariableValue");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "VariableValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "VariableValue",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VariableValue_ProductId",
                table: "VariableValue",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableValue_Products_ProductId",
                table: "VariableValue",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
