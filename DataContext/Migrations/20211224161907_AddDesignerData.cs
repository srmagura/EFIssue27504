using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddDesignerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DesignerData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Json = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignerData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignerData_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignerData_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesignerData_OrganizationId",
                table: "DesignerData",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerData_PageId",
                table: "DesignerData",
                column: "PageId");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DesignerData,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DesignerData after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DesignerData after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.DesignerData before delete
");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Imports,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Imports after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Imports after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Imports before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesignerData");
        }
    }
}
