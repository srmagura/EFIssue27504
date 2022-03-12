using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class RenameOrganizationOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    drop filter predicate on dbo.NotForConstructionDisclaimerTexts,
    drop block predicate on dbo.NotForConstructionDisclaimerTexts after insert,
    drop block predicate on dbo.NotForConstructionDisclaimerTexts after update,
    drop block predicate on dbo.NotForConstructionDisclaimerTexts before delete
");

            migrationBuilder.DropTable(
                name: "NotForConstructionDisclaimerTexts");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DefaultProjectDescriptions",
                newName: "Text");

            migrationBuilder.CreateTable(
                name: "NotForConstructionDisclaimers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotForConstructionDisclaimers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotForConstructionDisclaimers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotForConstructionDisclaimers_OrganizationId",
                table: "NotForConstructionDisclaimers",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimers,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimers after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimers after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimers before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotForConstructionDisclaimers");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "DefaultProjectDescriptions",
                newName: "Description");

            migrationBuilder.CreateTable(
                name: "NotForConstructionDisclaimerTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Disclaimer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotForConstructionDisclaimerTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotForConstructionDisclaimerTexts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotForConstructionDisclaimerTexts_OrganizationId",
                table: "NotForConstructionDisclaimerTexts",
                column: "OrganizationId",
                unique: true);
        }
    }
}
