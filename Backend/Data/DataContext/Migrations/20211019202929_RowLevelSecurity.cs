using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class RowLevelSecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a separate schema to house the RLS policy and function(s)
            migrationBuilder.Sql(@"EXECUTE sp_executesql N'create schema rls;';");

            migrationBuilder.Sql(@"
EXECUTE sp_executesql N'
create function rls.OrganizationIdPredicate(@OrganizationId uniqueidentifier)
    returns table
    with schemabinding
    as return
    select 1 as it
    where @OrganizationId >= CAST(SESSION_CONTEXT(N''OrganizationLowId'') as uniqueidentifier) and @OrganizationId <= CAST(SESSION_CONTEXT(N''OrganizationHighId'') as uniqueidentifier);
';");

            migrationBuilder.Sql(@"
create security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Symbols,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Symbols after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Symbols after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Symbols before delete
with (state = on)
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop security policy rls.SecurityPolicy");
            migrationBuilder.Sql("drop function rls.OrganizationIdPredicate");
            migrationBuilder.Sql("drop schema rls");
        }
    }
}
