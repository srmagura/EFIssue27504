using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class IntegrateProductKitsAndFamilies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductFamilies_OrganizationId",
                table: "ProductFamilies");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductFamilyId",
                table: "ProductKits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductKits_ProductFamilyId",
                table: "ProductKits",
                column: "ProductFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFamilies_OrganizationId_Name",
                table: "ProductFamilies",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductKits_ProductFamilies_ProductFamilyId",
                table: "ProductKits",
                column: "ProductFamilyId",
                principalTable: "ProductFamilies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductKits_ProductFamilies_ProductFamilyId",
                table: "ProductKits");

            migrationBuilder.DropIndex(
                name: "IX_ProductKits_ProductFamilyId",
                table: "ProductKits");

            migrationBuilder.DropIndex(
                name: "IX_ProductFamilies_OrganizationId_Name",
                table: "ProductFamilies");

            migrationBuilder.DropColumn(
                name: "ProductFamilyId",
                table: "ProductKits");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFamilies_OrganizationId",
                table: "ProductFamilies",
                column: "OrganizationId");
        }
    }
}
