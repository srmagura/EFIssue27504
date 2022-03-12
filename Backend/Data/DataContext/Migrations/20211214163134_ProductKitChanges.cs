using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class ProductKitChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ProductKitComponentMaps");

            migrationBuilder.AddColumn<Guid>(
                name: "MainComponentVersionId",
                table: "ProductKitVersions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitVersions_MainComponentVersionId",
                table: "ProductKitVersions",
                column: "MainComponentVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductKitVersions_ComponentVersions_MainComponentVersionId",
                table: "ProductKitVersions",
                column: "MainComponentVersionId",
                principalTable: "ComponentVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductKitVersions_ComponentVersions_MainComponentVersionId",
                table: "ProductKitVersions");

            migrationBuilder.DropIndex(
                name: "IX_ProductKitVersions_MainComponentVersionId",
                table: "ProductKitVersions");

            migrationBuilder.DropColumn(
                name: "MainComponentVersionId",
                table: "ProductKitVersions");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ProductKitComponentMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
