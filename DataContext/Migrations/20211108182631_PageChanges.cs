using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class PageChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailFileId",
                table: "Pages",
                newName: "Thumbnail_FileId");

            migrationBuilder.RenameColumn(
                name: "PdfFileId",
                table: "Pages",
                newName: "ProjectId");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Pdf_FileId",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Pdf_FileType",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Pdf_HasValue",
                table: "Pages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail_FileType",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Thumbnail_HasValue",
                table: "Pages",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ProjectId",
                table: "Pages",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Projects_ProjectId",
                table: "Pages",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
CREATE OR ALTER TRIGGER tr_Page_FileCleanup
	ON Pages
	AFTER DELETE
AS
BEGIN
	
	insert into DeletedFiles (Type, FileId)
	select 'Page', Pdf_FileId from DELETED where Pdf_FileId is not null

	insert into DeletedFiles (Type, FileId)
	select 'Thumbnail', Thumbnail_FileId from DELETED where Thumbnail_FileId is not null

END
GO
");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Pages,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Pages after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Pages after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Pages before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP TRIGGER tr_Page_FileCleanup
");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Projects_ProjectId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_ProjectId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Pdf_FileId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Pdf_FileType",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Pdf_HasValue",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Thumbnail_FileType",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Thumbnail_HasValue",
                table: "Pages");

            migrationBuilder.RenameColumn(
                name: "Thumbnail_FileId",
                table: "Pages",
                newName: "ThumbnailFileId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Pages",
                newName: "PdfFileId");
        }
    }
}
