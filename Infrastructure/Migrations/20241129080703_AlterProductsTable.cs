using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductImage_IconImageId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductImage_MainImageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_IconImageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MainImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IconImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "IconImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainImageUrl",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "IconImageId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MainImageId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_IconImageId",
                table: "Products",
                column: "IconImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MainImageId",
                table: "Products",
                column: "MainImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductImage_IconImageId",
                table: "Products",
                column: "IconImageId",
                principalTable: "ProductImage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductImage_MainImageId",
                table: "Products",
                column: "MainImageId",
                principalTable: "ProductImage",
                principalColumn: "Id");
        }
    }
}
