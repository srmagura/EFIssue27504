using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHost = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EstimatedSquareFeet = table.Column<int>(type: "int", nullable: false),
                    Photo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Photo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ReportOptions_SigneeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportOptions_PreparerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportOptions_TitleBlockSheetNameFontSize = table.Column<int>(type: "int", nullable: false),
                    ReportOptions_IncludeCompassInFooter = table.Column<bool>(type: "bit", nullable: false),
                    ReportOptions_CompassAngle = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DesignerLockedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    File_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProcessingStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProcessingEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Imports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Pdf_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Pdf_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Thumbnail_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Thumbnail_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    SheetNumberSuffix = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SheetNameSuffix = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Imports_OrganizationId",
                table: "Imports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Imports_ProjectId",
                table: "Imports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                table: "Organizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_OrganizationId",
                table: "Pages",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ProjectId",
                table: "Pages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId_Name",
                table: "Projects",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
