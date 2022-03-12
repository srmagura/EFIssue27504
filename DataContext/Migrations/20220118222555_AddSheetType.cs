using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddSheetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SheetNameSuffix",
                table: "Pages",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SheetNumberSuffix",
                table: "Pages",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SheetTypeId",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LogoSets",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "SheetTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SheetNumberPrefix = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SheetNamePrefix = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheetTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SheetTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReportOptions_LogoSetId",
                table: "Projects",
                column: "ReportOptions_LogoSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_SheetTypeId",
                table: "Pages",
                column: "SheetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SheetTypes_OrganizationId",
                table: "SheetTypes",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_SheetTypes_SheetTypeId",
                table: "Pages",
                column: "SheetTypeId",
                principalTable: "SheetTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                table: "Projects",
                column: "ReportOptions_LogoSetId",
                principalTable: "LogoSets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_SheetTypes_SheetTypeId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "SheetTypes");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ReportOptions_LogoSetId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Pages_SheetTypeId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "SheetNameSuffix",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "SheetNumberSuffix",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "SheetTypeId",
                table: "Pages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LogoSets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);
        }
    }
}
