using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class Components : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementType = table.Column<int>(type: "int", nullable: false),
                    IsTV = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    SellPrice_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellPrice_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    Url_Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentVersions_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentVersions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_OrganizationId",
                table: "Components",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentVersions_ComponentId_VersionName",
                table: "ComponentVersions",
                columns: new[] { "ComponentId", "VersionName" },
                unique: true);

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Components,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Components after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Components after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.Components before delete
");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ComponentVersions,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ComponentVersions after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ComponentVersions after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ComponentVersions before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentVersions");

            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
