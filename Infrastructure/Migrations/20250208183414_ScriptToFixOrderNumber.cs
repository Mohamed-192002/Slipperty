using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ScriptToFixOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.AlterColumn<string>(
                    name: "OrderNo",
                    table: "OrderHeads",
                    type: "VARCHAR(MAX)",
                    nullable: false,
                    oldClrType: typeof(string));
              
                migrationBuilder.Sql(@"
                      WITH RankedOrders AS (
                            SELECT
                                Id,
                                FORMAT(ISNULL(RegDate, GETDATE()), 'yyMMdd') AS FormattedDate,
                                RIGHT('0000' + CAST(ROW_NUMBER() OVER (PARTITION BY FORMAT(RegDate, 'yyMMdd') ORDER BY Id) AS VARCHAR), 4) AS RowNum
                            FROM OrderHeads
                        )
                        UPDATE o
                        SET o.orderNo = CAST(r.FormattedDate + r.RowNum AS NVARCHAR(20))
                        FROM OrderHeads o
                                 JOIN RankedOrders r ON o.Id = r.Id;

                ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
