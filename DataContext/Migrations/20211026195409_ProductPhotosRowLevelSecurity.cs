using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class ProductPhotosRowLevelSecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductPhotos,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductPhotos after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductPhotos after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductPhotos before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // suppress warning
        }
    }
}
