using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterOrderHeadTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "OrderHeads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deliveryTimeFrom",
                table: "OrderHeads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deliveryTimeTo",
                table: "OrderHeads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fullName",
                table: "OrderHeads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "governmentId",
                table: "OrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "paymentMethodId",
                table: "OrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "regionId",
                table: "OrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeads_governmentId",
                table: "OrderHeads",
                column: "governmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeads_paymentMethodId",
                table: "OrderHeads",
                column: "paymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeads_regionId",
                table: "OrderHeads",
                column: "regionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeads_Governments_governmentId",
                table: "OrderHeads",
                column: "governmentId",
                principalTable: "Governments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeads_Regions_regionId",
                table: "OrderHeads",
                column: "regionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeads_UserPaymentMethods_paymentMethodId",
                table: "OrderHeads",
                column: "paymentMethodId",
                principalTable: "UserPaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_Governments_governmentId",
                table: "OrderHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_Regions_regionId",
                table: "OrderHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_UserPaymentMethods_paymentMethodId",
                table: "OrderHeads");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeads_governmentId",
                table: "OrderHeads");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeads_paymentMethodId",
                table: "OrderHeads");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeads_regionId",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "deliveryTimeFrom",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "deliveryTimeTo",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "fullName",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "governmentId",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "paymentMethodId",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "regionId",
                table: "OrderHeads");
        }
    }
}
