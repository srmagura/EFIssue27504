using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddSheetTypeRLS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.SheetTypes,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.SheetTypes after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.SheetTypes after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.SheetTypes before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // nothing here
        }
    }
}
