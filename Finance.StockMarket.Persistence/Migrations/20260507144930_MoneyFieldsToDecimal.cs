using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.StockMarket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoneyFieldsToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear non-numeric legacy string values before type conversion
            migrationBuilder.Sql("UPDATE [Stocks] SET [MarketCap] = '0' WHERE [MarketCap] = '' OR TRY_CONVERT(decimal(18,2), [MarketCap]) IS NULL");
            migrationBuilder.Sql("UPDATE [Stocks] SET [CurrentPrice] = '0' WHERE [CurrentPrice] = '' OR TRY_CONVERT(decimal(18,2), [CurrentPrice]) IS NULL");
            migrationBuilder.Sql("UPDATE [Investments] SET [InvestedAmount] = '0' WHERE [InvestedAmount] = '' OR TRY_CONVERT(decimal(18,2), [InvestedAmount]) IS NULL");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarketCap",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "InvestedAmount",
                table: "Investments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MarketCap",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentPrice",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "InvestedAmount",
                table: "Investments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
