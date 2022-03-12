using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ProductFamiliesRLS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductFamilies,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductFamilies after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductFamilies after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductFamilies before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
