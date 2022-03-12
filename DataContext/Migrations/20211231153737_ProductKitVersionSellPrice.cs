using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ProductKitVersionSellPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SellPrice_HasValue",
                table: "ProductKitVersions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellPrice_Value",
                table: "ProductKitVersions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellPrice_HasValue",
                table: "ProductKitVersions");

            migrationBuilder.DropColumn(
                name: "SellPrice_Value",
                table: "ProductKitVersions");
        }
    }
}
