using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class MoreComponentFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ComponentVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPartNumber",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorPartNumber",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhereToBuy",
                table: "ComponentVersions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "Make",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "OrganizationPartNumber",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "VendorPartNumber",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "WhereToBuy",
                table: "ComponentVersions");
        }
    }
}
