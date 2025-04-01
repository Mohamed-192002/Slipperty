using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class missedOrdersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "OrderHeads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "OrderHeads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OrderHeads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactMethods",
                columns: table => new
                {
                    ContactMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactMethodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMethods", x => x.ContactMethodId);
                });

            migrationBuilder.CreateTable(
                name: "ContactStatuses",
                columns: table => new
                {
                    ContactStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactStatusName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactStatuses", x => x.ContactStatusId);
                });

            migrationBuilder.CreateTable(
                name: "OperationTypes",
                columns: table => new
                {
                    OperationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTypes", x => x.OperationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "OrderTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    PerformedBy = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_OrderTransactions_OrderHeads_OrderID",
                        column: x => x.OrderID,
                        principalTable: "OrderHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderFollowUps",
                columns: table => new
                {
                    FollowUpID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    PerformedBy = table.Column<int>(type: "int", nullable: false),
                    ContactMethodId = table.Column<int>(type: "int", nullable: true),
                    ContactStatusId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFollowUps", x => x.FollowUpID);
                    table.ForeignKey(
                        name: "FK_OrderFollowUps_ContactMethods_ContactMethodId",
                        column: x => x.ContactMethodId,
                        principalTable: "ContactMethods",
                        principalColumn: "ContactMethodId");
                    table.ForeignKey(
                        name: "FK_OrderFollowUps_ContactStatuses_ContactStatusId",
                        column: x => x.ContactStatusId,
                        principalTable: "ContactStatuses",
                        principalColumn: "ContactStatusId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeads_StatusId",
                table: "OrderHeads",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFollowUps_ContactMethodId",
                table: "OrderFollowUps",
                column: "ContactMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFollowUps_ContactStatusId",
                table: "OrderFollowUps",
                column: "ContactStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransactions_OrderID",
                table: "OrderTransactions",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeads_OrderStatuses_StatusId",
                table: "OrderHeads",
                column: "StatusId",
                principalTable: "OrderStatuses",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeads_OrderStatuses_StatusId",
                table: "OrderHeads");

            migrationBuilder.DropTable(
                name: "OperationTypes");

            migrationBuilder.DropTable(
                name: "OrderFollowUps");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "OrderTransactions");

            migrationBuilder.DropTable(
                name: "ContactMethods");

            migrationBuilder.DropTable(
                name: "ContactStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeads_StatusId",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "OrderHeads");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OrderHeads");
        }
    }
}
