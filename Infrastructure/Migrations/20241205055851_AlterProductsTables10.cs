using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTables10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariable_Products_ProductId",
                table: "ProductVariable");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableValue_ProductVariable_ProductVariableId",
                table: "VariableValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariableValue",
                table: "VariableValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariable",
                table: "ProductVariable");

            migrationBuilder.RenameTable(
                name: "VariableValue",
                newName: "VariableValues");

            migrationBuilder.RenameTable(
                name: "ProductVariable",
                newName: "ProductVariables");

            migrationBuilder.RenameIndex(
                name: "IX_VariableValue_ProductVariableId",
                table: "VariableValues",
                newName: "IX_VariableValues_ProductVariableId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariable_ProductId",
                table: "ProductVariables",
                newName: "IX_ProductVariables_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariableValues",
                table: "VariableValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariables",
                table: "ProductVariables",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariables_Products_ProductId",
                table: "ProductVariables",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableValues_ProductVariables_ProductVariableId",
                table: "VariableValues",
                column: "ProductVariableId",
                principalTable: "ProductVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariables_Products_ProductId",
                table: "ProductVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableValues_ProductVariables_ProductVariableId",
                table: "VariableValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariableValues",
                table: "VariableValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariables",
                table: "ProductVariables");

            migrationBuilder.RenameTable(
                name: "VariableValues",
                newName: "VariableValue");

            migrationBuilder.RenameTable(
                name: "ProductVariables",
                newName: "ProductVariable");

            migrationBuilder.RenameIndex(
                name: "IX_VariableValues_ProductVariableId",
                table: "VariableValue",
                newName: "IX_VariableValue_ProductVariableId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariables_ProductId",
                table: "ProductVariable",
                newName: "IX_ProductVariable_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariableValue",
                table: "VariableValue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariable",
                table: "ProductVariable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariable_Products_ProductId",
                table: "ProductVariable",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableValue_ProductVariable_ProductVariableId",
                table: "VariableValue",
                column: "ProductVariableId",
                principalTable: "ProductVariable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
