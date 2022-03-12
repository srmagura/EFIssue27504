using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class CompanyContactInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_Email_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportOptions_CompanyContactInfo_Email_Value",
                table: "Projects",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportOptions_CompanyContactInfo_Name",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_Phone_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportOptions_CompanyContactInfo_Phone_Value",
                table: "Projects",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportOptions_CompanyContactInfo_Url",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Email_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Email_Value",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Name",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Phone_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Phone_Value",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Url",
                table: "Projects");
        }
    }
}
