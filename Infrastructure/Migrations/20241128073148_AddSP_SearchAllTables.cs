using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSP_SearchAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER     	    PROC [dbo].[SearchAllTables] (@SearchStr nvarchar(100), @ColNameToSearch nvarchar(100))
			AS
			BEGIN
				CREATE TABLE #Results (ColumnName nvarchar(370), ColumnValue nvarchar(3630))
				SET NOCOUNT ON
				DECLARE @TableName nvarchar(256), @SearchStr2 nvarchar(110) , @ColumnName nvarchar(128), @isNumeric  int
				SET  @TableName = ''
				--SET @SearchStr2 = QUOTENAME('%' + @SearchStr + '%','''')
				SET @SearchStr2 = QUOTENAME('' + @SearchStr + '','''')
				SET @isNumeric = CASE WHEN @ColNameToSearch like '%id%' THEN 1 ELSE 0 END  --isnumeric(@SearchStr)
				WHILE @TableName IS NOT NULL
				BEGIN
					SET @ColumnName = ''
					SET @TableName = 
					(
						SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
						FROM 	INFORMATION_SCHEMA.TABLES
						WHERE 		TABLE_TYPE = 'BASE TABLE'
							AND	QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName
							AND	OBJECTPROPERTY(
									OBJECT_ID(
										QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
										 ), 'IsMSShipped'
										   ) = 0
					)
					WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL)
					BEGIN
						if (@isNumeric = 1)
							BEGIN
								SET @ColumnName =
								(
									SELECT  MIN(QUOTENAME(COLUMN_NAME))
									FROM 	INFORMATION_SCHEMA.COLUMNS col
									WHERE 		TABLE_SCHEMA	= PARSENAME(@TableName, 2)
										AND	TABLE_NAME	= PARSENAME(@TableName, 1)
										AND	DATA_TYPE IN ('int', 'smallint', 'bigint') 
										AND	QUOTENAME(COLUMN_NAME) > @ColumnName
										AND QUOTENAME(COLUMN_NAME) LIKE '%' + @ColNameToSearch + '%'
								)
							END
					 else
							BEGIN
								SET @ColumnName =
								(
									SELECT  MIN(QUOTENAME(COLUMN_NAME))
									FROM 	INFORMATION_SCHEMA.COLUMNS col
									WHERE 		TABLE_SCHEMA	= PARSENAME(@TableName, 2)
										AND	TABLE_NAME	= PARSENAME(@TableName, 1)
										AND	DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') 
										AND	QUOTENAME(COLUMN_NAME) > @ColumnName
										AND QUOTENAME(COLUMN_NAME) LIKE '%' + @ColNameToSearch + '%'
								)
							END
							IF @ColumnName IS NOT NULL
							BEGIN
								INSERT INTO #Results
								EXEC
								(
									'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630) 
									FROM ' + @TableName + ' (NOLOCK) ' +
									' WHERE ' + @ColumnName + ' = ' + @SearchStr2
								)
						END
					END	
				END
				--SELECT ColumnName, ColumnValue FROM #Results
				SELECT count(ColumnName)[Occurenrce] FROM #Results
			END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop Proc if exists SearchAllTables");
        }
    }
}
