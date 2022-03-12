using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class AddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Line1 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Address_Line2 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Address_PostalCode_Value = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Address_PostalCode_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    Address_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedSquareFeet = table.Column<int>(type: "int", nullable: false),
                    Photo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Photo_FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    BudgetOptions_CostAdjustment_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BudgetOptions_CostAdjustment_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    BudgetOptions_DepositPercentage_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BudgetOptions_DepositPercentage_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    BudgetOptions_ShowPricingInBudgetBreakdown = table.Column<bool>(type: "bit", nullable: false),
                    BudgetOptions_ShowPricePerSquareFoot = table.Column<bool>(type: "bit", nullable: false),
                    BudgetOptions_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    ReportOptions_SigneeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportOptions_PreparerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportOptions_TitleBlockSheetNameFontSize = table.Column<int>(type: "int", nullable: false),
                    ReportOptions_IncludeCompassInFooter = table.Column<bool>(type: "bit", nullable: false),
                    ReportOptions_CompassAngle = table.Column<int>(type: "int", nullable: false),
                    ReportOptions_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
