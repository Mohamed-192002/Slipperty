using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStockTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_UpdateStockCount
ON OrderDetails
AFTER INSERT
AS
BEGIN
    -- Declare variables to store inserted values
    DECLARE @CombinationId INT, @Qty INT, @ProductId INT, @StockCount INT;

    -- Get the values from the inserted row
    SELECT @CombinationId = CombinationId, @Qty = Qty, @ProductId = ProductId
    FROM inserted;

    -- Check if CombinationId is not NULL (or check for a valid value)
    IF @CombinationId IS NOT NULL
    BEGIN
        -- Retrieve the current StockCount for the combination and product
        SELECT @StockCount = StockCount
        FROM VariableCombinations
        WHERE Id = @CombinationId AND ProductId = @ProductId;

        -- Check if the stock count is less than the ordered quantity
        IF @Qty > @StockCount
        BEGIN
            -- If quantity is greater than stock count, set stock count to 0
            UPDATE vc
            SET vc.StockCount = 0
            FROM VariableCombinations vc
            WHERE vc.Id = @CombinationId AND vc.ProductId = @ProductId;
        END
        ELSE
        BEGIN
            -- Otherwise, subtract the quantity from stock count
            UPDATE vc
            SET vc.StockCount = vc.StockCount - @Qty
            FROM VariableCombinations vc
            WHERE vc.Id = @CombinationId AND vc.ProductId = @ProductId;
        END
    END
END;

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
