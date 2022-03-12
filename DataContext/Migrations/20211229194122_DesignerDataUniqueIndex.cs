using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class DesignerDataUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DesignerData_PageId",
                table: "DesignerData");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerData_PageId_Type",
                table: "DesignerData",
                columns: new[] { "PageId", "Type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DesignerData_PageId_Type",
                table: "DesignerData");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerData_PageId",
                table: "DesignerData",
                column: "PageId");
        }
    }
}
