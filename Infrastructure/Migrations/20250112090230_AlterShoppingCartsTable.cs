using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterShoppingCartsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "offerId",
                table: "ShoppingCarts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_offerId",
                table: "ShoppingCarts",
                column: "offerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_ProductCountsOffers_offerId",
                table: "ShoppingCarts",
                column: "offerId",
                principalTable: "ProductCountsOffers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_ProductCountsOffers_offerId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_offerId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "offerId",
                table: "ShoppingCarts");
        }
    }
}
