using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderCancelationDetailsTableAlterColumnUserIdToRejectionReasoneId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "OrderCancelationDetails");

            migrationBuilder.AddColumn<int>(
                name: "RejectionReasoneId",
                table: "OrderCancelationDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReasoneId",
                table: "OrderCancelationDetails");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "OrderCancelationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
