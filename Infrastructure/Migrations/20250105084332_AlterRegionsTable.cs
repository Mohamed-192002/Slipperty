using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRegionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GovernmentId",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_GovernmentId",
                table: "Regions",
                column: "GovernmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Governments_GovernmentId",
                table: "Regions",
                column: "GovernmentId",
                principalTable: "Governments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Governments_GovernmentId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_GovernmentId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "GovernmentId",
                table: "Regions");
        }
    }
}
