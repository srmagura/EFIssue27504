using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddImport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    File_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProcessingStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProcessingEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PercentComplete_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PercentComplete_HasValue = table.Column<bool>(type: "bit", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Imports_OrganizationId",
                table: "Imports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Imports_ProjectId",
                table: "Imports",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imports");
        }
    }
}
