using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterOrderHeadTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeads_OrderHeadId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_Governments_governmentId",
                table: "OrderHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_Regions_regionId",
                table: "OrderHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_UserPaymentMethods_paymentMethodId",
                table: "OrderHeads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHeads",
                table: "OrderHeads");

            migrationBuilder.RenameTable(
                name: "OrderHeads",
                newName: "OrderHead");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeads_regionId",
                table: "OrderHead",
                newName: "IX_OrderHead_regionId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeads_paymentMethodId",
                table: "OrderHead",
                newName: "IX_OrderHead_paymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeads_governmentId",
                table: "OrderHead",
                newName: "IX_OrderHead_governmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHead",
                table: "OrderHead",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHead_OrderHeadId",
                table: "OrderDetails",
                column: "OrderHeadId",
                principalTable: "OrderHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHead_Governments_governmentId",
                table: "OrderHead",
                column: "governmentId",
                principalTable: "Governments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHead_Regions_regionId",
                table: "OrderHead",
                column: "regionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHead_UserPaymentMethods_paymentMethodId",
                table: "OrderHead",
                column: "paymentMethodId",
                principalTable: "UserPaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHead_OrderHeadId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHead_Governments_governmentId",
                table: "OrderHead");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHead_Regions_regionId",
                table: "OrderHead");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHead_UserPaymentMethods_paymentMethodId",
                table: "OrderHead");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHead",
                table: "OrderHead");

            migrationBuilder.RenameTable(
                name: "OrderHead",
                newName: "OrderHeads");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHead_regionId",
                table: "OrderHeads",
                newName: "IX_OrderHeads_regionId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHead_paymentMethodId",
                table: "OrderHeads",
                newName: "IX_OrderHeads_paymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHead_governmentId",
                table: "OrderHeads",
                newName: "IX_OrderHeads_governmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHeads",
                table: "OrderHeads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeads_OrderHeadId",
                table: "OrderDetails",
                column: "OrderHeadId",
                principalTable: "OrderHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
