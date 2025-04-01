using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class makeAdditionalPriceNotRequered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderUrgentDetails_OrderID",
                table: "OrderUrgentDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails");

            migrationBuilder.AlterColumn<double>(
                name: "AdditionalPrice",
                table: "OrderUrgentDetails",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_OrderUrgentDetails_OrderID",
                table: "OrderUrgentDetails",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderFollowUps_OrderID",
                table: "OrderFollowUps",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFollowUps_OrderHeads_OrderID",
                table: "OrderFollowUps",
                column: "OrderID",
                principalTable: "OrderHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderFollowUps_OrderHeads_OrderID",
                table: "OrderFollowUps");

            migrationBuilder.DropIndex(
                name: "IX_OrderUrgentDetails_OrderID",
                table: "OrderUrgentDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderFollowUps_OrderID",
                table: "OrderFollowUps");

            migrationBuilder.AlterColumn<double>(
                name: "AdditionalPrice",
                table: "OrderUrgentDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderUrgentDetails_OrderID",
                table: "OrderUrgentDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails",
                column: "OrderID");
        }
    }
}
