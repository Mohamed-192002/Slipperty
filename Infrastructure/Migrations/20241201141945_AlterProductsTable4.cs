using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTable4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "ProductVariable");

            migrationBuilder.DropColumn(
                name: "EnableProductVariables",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "VariableValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductVariableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariableValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariableValue_ProductVariable_ProductVariableId",
                        column: x => x.ProductVariableId,
                        principalTable: "ProductVariable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariableValue_ProductVariableId",
                table: "VariableValue",
                column: "ProductVariableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VariableValue");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ProductVariable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableProductVariables",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
