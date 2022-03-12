using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class OrganizationOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultProjectDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultProjectDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultProjectDescriptions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotForConstructionDisclaimerTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Disclaimer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                name: "IX_DefaultProjectDescriptions_OrganizationId",
                table: "DefaultProjectDescriptions",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotForConstructionDisclaimerTexts_OrganizationId",
                table: "NotForConstructionDisclaimerTexts",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DefaultProjectDescriptions,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DefaultProjectDescriptions after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DefaultProjectDescriptions after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DefaultProjectDescriptions before delete
");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimerTexts,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimerTexts after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimerTexts after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.NotForConstructionDisclaimerTexts before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultProjectDescriptions");

            migrationBuilder.DropTable(
                name: "NotForConstructionDisclaimerTexts");
        }
    }
}
