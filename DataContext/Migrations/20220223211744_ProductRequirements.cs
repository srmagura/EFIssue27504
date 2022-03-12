using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ProductRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SvgText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRequirements_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequirements_OrganizationId",
                table: "ProductRequirements",
                column: "OrganizationId");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductRequirements,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductRequirements after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductRequirements after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductRequirements before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRequirements");
        }
    }
}
