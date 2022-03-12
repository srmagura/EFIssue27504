using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ProductKits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ComponentVersions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ProductKits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKits_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductKitVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductKitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    SymbolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductPhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKitVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKitVersions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKitVersions_ProductKits_ProductKitId",
                        column: x => x.ProductKitId,
                        principalTable: "ProductKits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductKitVersions_ProductPhotos_ProductPhotoId",
                        column: x => x.ProductPhotoId,
                        principalTable: "ProductPhotos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductKitVersions_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductKitComponentMaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductKitVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKitComponentMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKitComponentMaps_ComponentVersions_ComponentVersionId",
                        column: x => x.ComponentVersionId,
                        principalTable: "ComponentVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKitComponentMaps_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductKitComponentMaps_ProductKitVersions_ProductKitVersionId",
                        column: x => x.ProductKitVersionId,
                        principalTable: "ProductKitVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentVersions_OrganizationId",
                table: "ComponentVersions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitComponentMaps_ComponentVersionId",
                table: "ProductKitComponentMaps",
                column: "ComponentVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitComponentMaps_OrganizationId",
                table: "ProductKitComponentMaps",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitComponentMaps_ProductKitVersionId",
                table: "ProductKitComponentMaps",
                column: "ProductKitVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKits_CategoryId",
                table: "ProductKits",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKits_OrganizationId",
                table: "ProductKits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitVersions_OrganizationId",
                table: "ProductKitVersions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitVersions_ProductKitId_VersionName",
                table: "ProductKitVersions",
                columns: new[] { "ProductKitId", "VersionName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitVersions_ProductPhotoId",
                table: "ProductKitVersions",
                column: "ProductPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitVersions_SymbolId",
                table: "ProductKitVersions",
                column: "SymbolId");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKits,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKits after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKits after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKits before delete
");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitVersions,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitVersions after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitVersions after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitVersions before delete
");

            migrationBuilder.Sql(@"
alter security policy rls.SecurityPolicy
    add filter predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitComponentMaps,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitComponentMaps after insert,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitComponentMaps after update,
    add block predicate rls.OrganizationIdPredicate(OrganizationId) on dbo.ProductKitComponentMaps before delete
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductKitComponentMaps");

            migrationBuilder.DropTable(
                name: "ProductKitVersions");

            migrationBuilder.DropTable(
                name: "ProductKits");

            migrationBuilder.DropIndex(
                name: "IX_ComponentVersions_OrganizationId",
                table: "ComponentVersions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ComponentVersions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentVersions_OrganizationId_Name",
                table: "ComponentVersions",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);
        }
    }
}
