using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderModificationDeclinedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "OrderFollowUps",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "OrderModificationDeclined",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderModificationDeclined", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderModificationDeclined_OrderHeads_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderModificationDeclined_OrderId",
                table: "OrderModificationDeclined",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderModificationDeclined");

            migrationBuilder.DropIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "OrderFollowUps",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHoldingDetails_OrderID",
                table: "OrderHoldingDetails",
                column: "OrderID",
                unique: true);
        }
    }
}
