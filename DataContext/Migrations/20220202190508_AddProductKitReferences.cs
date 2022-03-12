using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddProductKitReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductKitReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductKitVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKitReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKitReferences_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKitReferences_ProductKitVersions_ProductKitVersionId",
                        column: x => x.ProductKitVersionId,
                        principalTable: "ProductKitVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductKitReferences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitReferences_OrganizationId",
                table: "ProductKitReferences",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitReferences_ProductKitVersionId",
                table: "ProductKitReferences",
                column: "ProductKitVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitReferences_ProjectId",
                table: "ProductKitReferences",
                column: "ProjectId");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitReferences,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitReferences after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitReferences after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitReferences before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductKitReferences");
        }
    }
}
