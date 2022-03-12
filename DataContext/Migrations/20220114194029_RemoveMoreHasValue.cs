using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class RemoveMoreHasValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkLogo_HasValue",
                table: "LogoSets");

            migrationBuilder.DropColumn(
                name: "LightLogo_HasValue",
                table: "LogoSets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DarkLogo_HasValue",
                table: "LogoSets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LightLogo_HasValue",
                table: "LogoSets",
                type: "bit",
                nullable: true);
        }
    }
}
