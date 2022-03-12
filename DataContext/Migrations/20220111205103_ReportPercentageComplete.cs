using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ReportPercentageComplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PercentComplete_HasValue",
                table: "Reports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentComplete_Value",
                table: "Reports",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentComplete_HasValue",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PercentComplete_Value",
                table: "Reports");
        }
    }
}
