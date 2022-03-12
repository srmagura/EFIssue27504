using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class CompanyContactInfoUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReportOptions_CompanyContactInfo_Url",
                table: "Projects",
                newName: "ReportOptions_CompanyContactInfo_Url_Value");

            migrationBuilder.AlterColumn<string>(
                name: "ReportOptions_CompanyContactInfo_Name",
                table: "Projects",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_Url_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Url_HasValue",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ReportOptions_CompanyContactInfo_Url_Value",
                table: "Projects",
                newName: "ReportOptions_CompanyContactInfo_Url");

            migrationBuilder.AlterColumn<string>(
                name: "ReportOptions_CompanyContactInfo_Name",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}
