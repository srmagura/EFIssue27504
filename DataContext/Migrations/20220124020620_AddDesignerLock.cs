using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class AddDesignerLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesignerLockedById",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DesignerLockedUtc",
                table: "Projects",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DesignerLockedById",
                table: "Projects",
                column: "DesignerLockedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_DesignerLockedById",
                table: "Projects",
                column: "DesignerLockedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_DesignerLockedById",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_DesignerLockedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DesignerLockedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DesignerLockedUtc",
                table: "Projects");
        }
    }
}
