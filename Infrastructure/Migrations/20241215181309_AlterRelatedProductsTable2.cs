using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRelatedProductsTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Products_RelatedProductId",
                table: "RelatedProducts");

            migrationBuilder.DropIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProducts");
        }
    }
}
