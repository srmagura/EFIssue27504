﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class ProjectRowLevelSecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Projects,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Projects after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Projects after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Projects before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // suppress warning
        }
    }
}