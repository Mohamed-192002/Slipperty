using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductSizeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ProductSize",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_ColorId",
                table: "ProductSize",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_ProductColor_ColorId",
                table: "ProductSize",
                column: "ColorId",
                principalTable: "ProductColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_ProductColor_ColorId",
                table: "ProductSize");

            migrationBuilder.DropIndex(
                name: "IX_ProductSize_ColorId",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ProductSize");
        }
    }
}
