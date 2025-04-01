using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTable14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManufacturingId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufacturingId",
                table: "Products",
                column: "ManufacturingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Manufacturing_ManufacturingId",
                table: "Products",
                column: "ManufacturingId",
                principalTable: "Manufacturing",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Manufacturing_ManufacturingId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ManufacturingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturingId",
                table: "Products");
        }
    }
}
