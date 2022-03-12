using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class LogoSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReportOptions_LogoSetId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LogoSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DarkLogo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DarkLogo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DarkLogo_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    LightLogo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LightLogo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LightLogo_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogoSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogoSets_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogoSets_OrganizationId_Name",
                table: "LogoSets",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.LogoSets,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.LogoSets after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.LogoSets after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.LogoSets before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogoSets");

            migrationBuilder.DropColumn(
                name: "ReportOptions_LogoSetId",
                table: "Projects");
        }
    }
}
