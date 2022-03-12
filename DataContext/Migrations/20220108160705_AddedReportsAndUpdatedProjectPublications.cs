using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddedReportsAndUpdatedProjectPublications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPublications_Projects_ProjectId",
                table: "ProjectPublications");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPublications_ProjectId",
                table: "ProjectPublications");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "ProjectPublications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProjectPublications,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProjectPublications after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProjectPublications after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProjectPublications before delete
");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectPublicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    File_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    File_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    File_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProcessingEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_ProjectPublications_ProjectPublicationId",
                        column: x => x.ProjectPublicationId,
                        principalTable: "ProjectPublications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublications_OrganizationId",
                table: "ProjectPublications",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublications_ProjectId_RevisionNumber",
                table: "ProjectPublications",
                columns: new[] { "ProjectId", "RevisionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OrganizationId",
                table: "Reports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProjectId",
                table: "Reports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProjectPublicationId",
                table: "Reports",
                column: "ProjectPublicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPublications_Organizations_OrganizationId",
                table: "ProjectPublications",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPublications_Projects_ProjectId",
                table: "ProjectPublications",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Reports,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Reports after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Reports after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Reports before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPublications_Organizations_OrganizationId",
                table: "ProjectPublications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPublications_Projects_ProjectId",
                table: "ProjectPublications");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPublications_OrganizationId",
                table: "ProjectPublications");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPublications_ProjectId_RevisionNumber",
                table: "ProjectPublications");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "ProjectPublications");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublications_ProjectId",
                table: "ProjectPublications",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPublications_Projects_ProjectId",
                table: "ProjectPublications",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
