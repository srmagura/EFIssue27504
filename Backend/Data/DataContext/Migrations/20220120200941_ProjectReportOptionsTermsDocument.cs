using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ProjectReportOptionsTermsDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReportOptions_TermsDocumentId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReportOptions_TermsDocumentId",
                table: "Projects",
                column: "ReportOptions_TermsDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_TermsDocuments_ReportOptions_TermsDocumentId",
                table: "Projects",
                column: "ReportOptions_TermsDocumentId",
                principalTable: "TermsDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_TermsDocuments_ReportOptions_TermsDocumentId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ReportOptions_TermsDocumentId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportOptions_TermsDocumentId",
                table: "Projects");
        }
    }
}
