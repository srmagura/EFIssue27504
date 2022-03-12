using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    SymbolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_OrganizationId",
                table: "Categories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SymbolId",
                table: "Categories",
                column: "SymbolId");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Categories,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Categories after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Categories after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Categories before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProductPhotos_Name",
                table: "ProductPhotos");
        }
    }
}
