using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class LogoSetIdRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                table: "Projects");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportOptions_LogoSetId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                table: "Projects",
                column: "ReportOptions_LogoSetId",
                principalTable: "LogoSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                table: "Projects");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportOptions_LogoSetId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                table: "Projects",
                column: "ReportOptions_LogoSetId",
                principalTable: "LogoSets",
                principalColumn: "Id");
        }
    }
}
