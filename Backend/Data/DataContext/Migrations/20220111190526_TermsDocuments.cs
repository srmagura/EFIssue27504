using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class TermsDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TermsDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    File_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    File_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    File_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermsDocuments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TermsDocuments_OrganizationId_Number",
                table: "TermsDocuments",
                columns: new[] { "OrganizationId", "Number" },
                unique: true);

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.TermsDocuments,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.TermsDocuments after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.TermsDocuments after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.TermsDocuments before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermsDocuments");
        }
    }
}
