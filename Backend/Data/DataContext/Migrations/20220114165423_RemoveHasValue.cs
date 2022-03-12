using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class RemoveHasValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email_HasValue",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EncodedPassword_HasValue",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name_HasValue",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "File_HasValue",
                table: "TermsDocuments");

            migrationBuilder.DropColumn(
                name: "File_HasValue",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PercentComplete_HasValue",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BudgetOptions_CostAdjustment_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BudgetOptions_DepositPercentage_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BudgetOptions_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Photo_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Email_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Phone_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_CompanyContactInfo_Url_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_HasValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Photo_HasValue",
                table: "ProductPhotos");

            migrationBuilder.DropColumn(
                name: "SellPrice_HasValue",
                table: "ProductKitVersions");

            migrationBuilder.DropColumn(
                name: "Pdf_HasValue",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Thumbnail_HasValue",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ShortName_HasValue",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "File_HasValue",
                table: "Imports");

            migrationBuilder.DropColumn(
                name: "PercentComplete_HasValue",
                table: "Imports");

            migrationBuilder.DropColumn(
                name: "SellPrice_HasValue",
                table: "ComponentVersions");

            migrationBuilder.DropColumn(
                name: "Url_HasValue",
                table: "ComponentVersions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Email_HasValue",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EncodedPassword_HasValue",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Name_HasValue",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "File_HasValue",
                table: "TermsDocuments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "File_HasValue",
                table: "Reports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PercentComplete_HasValue",
                table: "Reports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Address_PostalCode_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BudgetOptions_CostAdjustment_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BudgetOptions_DepositPercentage_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BudgetOptions_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Photo_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_Email_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_Phone_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_CompanyContactInfo_Url_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportOptions_HasValue",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Photo_HasValue",
                table: "ProductPhotos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SellPrice_HasValue",
                table: "ProductKitVersions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pdf_HasValue",
                table: "Pages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Thumbnail_HasValue",
                table: "Pages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShortName_HasValue",
                table: "Organizations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "File_HasValue",
                table: "Imports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PercentComplete_HasValue",
                table: "Imports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SellPrice_HasValue",
                table: "ComponentVersions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Url_HasValue",
                table: "ComponentVersions",
                type: "bit",
                nullable: true);
        }
    }
}
