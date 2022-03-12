using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class RemoveDeletedFilesTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP TRIGGER tr_Page_FileCleanup
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // suppress warning
        }
    }
}
