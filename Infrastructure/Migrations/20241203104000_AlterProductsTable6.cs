using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTable6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "VariableValue",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VariableCombination",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariableCombination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariableCombination_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariableValue_ProductId",
                table: "VariableValue",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VariableCombination_ProductId",
                table: "VariableCombination",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableValue_Products_ProductId",
                table: "VariableValue",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariableValue_Products_ProductId",
                table: "VariableValue");

            migrationBuilder.DropTable(
                name: "VariableCombination");

            migrationBuilder.DropIndex(
                name: "IX_VariableValue_ProductId",
                table: "VariableValue");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "VariableValue");
        }
    }
}
