using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductsTables8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCountsOffer_Products_ProductId",
                table: "ProductCountsOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVideo_Products_ProductId",
                table: "ProductVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProduct_Products_ProductId",
                table: "RelatedProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedProduct",
                table: "RelatedProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVideo",
                table: "ProductVideo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCountsOffer",
                table: "ProductCountsOffer");

            migrationBuilder.RenameTable(
                name: "RelatedProduct",
                newName: "RelatedProducts");

            migrationBuilder.RenameTable(
                name: "ProductVideo",
                newName: "ProductVideos");

            migrationBuilder.RenameTable(
                name: "ProductCountsOffer",
                newName: "ProductCountsOffers");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedProduct_ProductId",
                table: "RelatedProducts",
                newName: "IX_RelatedProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVideo_ProductId",
                table: "ProductVideos",
                newName: "IX_ProductVideos_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCountsOffer_ProductId",
                table: "ProductCountsOffers",
                newName: "IX_ProductCountsOffers_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedProducts",
                table: "RelatedProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVideos",
                table: "ProductVideos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCountsOffers",
                table: "ProductCountsOffers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCountsOffers_Products_ProductId",
                table: "ProductCountsOffers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVideos_Products_ProductId",
                table: "ProductVideos",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Products_ProductId",
                table: "RelatedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCountsOffers_Products_ProductId",
                table: "ProductCountsOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVideos_Products_ProductId",
                table: "ProductVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Products_ProductId",
                table: "RelatedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedProducts",
                table: "RelatedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVideos",
                table: "ProductVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCountsOffers",
                table: "ProductCountsOffers");

            migrationBuilder.RenameTable(
                name: "RelatedProducts",
                newName: "RelatedProduct");

            migrationBuilder.RenameTable(
                name: "ProductVideos",
                newName: "ProductVideo");

            migrationBuilder.RenameTable(
                name: "ProductCountsOffers",
                newName: "ProductCountsOffer");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedProducts_ProductId",
                table: "RelatedProduct",
                newName: "IX_RelatedProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVideos_ProductId",
                table: "ProductVideo",
                newName: "IX_ProductVideo_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCountsOffers_ProductId",
                table: "ProductCountsOffer",
                newName: "IX_ProductCountsOffer_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedProduct",
                table: "RelatedProduct",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVideo",
                table: "ProductVideo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCountsOffer",
                table: "ProductCountsOffer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCountsOffer_Products_ProductId",
                table: "ProductCountsOffer",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVideo_Products_ProductId",
                table: "ProductVideo",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProduct_Products_ProductId",
                table: "RelatedProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
