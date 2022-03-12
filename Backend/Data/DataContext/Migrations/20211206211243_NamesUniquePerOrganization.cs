using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class NamesUniquePerOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Symbols_Name",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_OrganizationId",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProductPhotos_Name",
                table: "ProductPhotos");

            migrationBuilder.DropIndex(
                name: "IX_ProductPhotos_OrganizationId",
                table: "ProductPhotos");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_ShortName_Value",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Thread",
                table: "LogEntries");

            migrationBuilder.AlterColumn<string>(
                name: "ShortName_Value",
                table: "Organizations",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_OrganizationId_Name",
                table: "Symbols",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId_Name",
                table: "Projects",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_OrganizationId_Name",
                table: "ProductPhotos",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ShortName_Value",
                table: "Organizations",
                column: "ShortName_Value",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Symbols_OrganizationId_Name",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganizationId_Name",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProductPhotos_OrganizationId_Name",
                table: "ProductPhotos");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_ShortName_Value",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "ShortName_Value",
                table: "Organizations",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AddColumn<string>(
                name: "Thread",
                table: "LogEntries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name",
                table: "Symbols",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_OrganizationId",
                table: "Symbols",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_Name",
                table: "ProductPhotos",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_OrganizationId",
                table: "ProductPhotos",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ShortName_Value",
                table: "Organizations",
                column: "ShortName_Value",
                unique: true,
                filter: "[ShortName_Value] IS NOT NULL");
        }
    }
}
