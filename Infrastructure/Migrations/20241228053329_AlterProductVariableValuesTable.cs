using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductVariableValuesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductVariables");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "VariableValues",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "VariableValues");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductVariables",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
