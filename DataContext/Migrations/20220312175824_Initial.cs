using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHost = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ShortName_Value = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefaultProjectDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultProjectDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultProjectDescriptions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogoSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DarkLogo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DarkLogo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LightLogo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LightLogo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogoSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogoSets_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotForConstructionDisclaimers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotForConstructionDisclaimers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotForConstructionDisclaimers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFamilies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFamilies_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Photo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Photo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPhotos_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "SheetTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SheetNumberPrefix = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SheetNamePrefix = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheetTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SheetTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SvgText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symbols_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TermsDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    File_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    File_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermsDocuments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email_Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name_First = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name_Last = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementType = table.Column<int>(type: "int", nullable: false),
                    IsVideoDisplay = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    VisibleToCustomer = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_ComponentTypes_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Components_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    SymbolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_HasValue = table.Column<bool>(type: "bit", nullable: true),
                    Address_Line1 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Address_Line2 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Address_State = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Address_PostalCode_Value = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EstimatedSquareFeet = table.Column<int>(type: "int", nullable: false),
                    Photo_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Photo_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    BudgetOptions_CostAdjustment_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BudgetOptions_DepositPercentage_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BudgetOptions_ShowPricingInBudgetBreakdown = table.Column<bool>(type: "bit", nullable: false),
                    BudgetOptions_ShowPricePerSquareFoot = table.Column<bool>(type: "bit", nullable: false),
                    ReportOptions_SigneeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportOptions_PreparerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportOptions_TitleBlockSheetNameFontSize = table.Column<int>(type: "int", nullable: false),
                    ReportOptions_IncludeCompassInFooter = table.Column<bool>(type: "bit", nullable: false),
                    ReportOptions_CompassAngle = table.Column<int>(type: "int", nullable: false),
                    ReportOptions_CompanyContactInfo_Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ReportOptions_CompanyContactInfo_Url_Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportOptions_CompanyContactInfo_Email_Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ReportOptions_CompanyContactInfo_Phone_Value = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ReportOptions_LogoSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportOptions_TermsDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DesignerLockedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignerLockedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_LogoSets_ReportOptions_LogoSetId",
                        column: x => x.ReportOptions_LogoSetId,
                        principalTable: "LogoSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_TermsDocuments_ReportOptions_TermsDocumentId",
                        column: x => x.ReportOptions_TermsDocumentId,
                        principalTable: "TermsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Users_DesignerLockedById",
                        column: x => x.DesignerLockedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComponentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    SellPrice_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Url_Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Make = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    VendorPartNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OrganizationPartNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    WhereToBuy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Style = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductKits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProductFamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKits_ProductFamilies_ProductFamilyId",
                        column: x => x.ProductFamilyId,
                        principalTable: "ProductFamilies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    File_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProcessingStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProcessingEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PercentComplete_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Imports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Pdf_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Pdf_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Thumbnail_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Thumbnail_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    SheetTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SheetNumberSuffix = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SheetNameSuffix = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_SheetTypes_SheetTypeId",
                        column: x => x.SheetTypeId,
                        principalTable: "SheetTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectPublications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublishedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionNumber = table.Column<int>(type: "int", nullable: false),
                    ReportsSentToCustomer = table.Column<bool>(type: "bit", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPublications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPublications_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectPublications_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectPublications_Users_PublishedById",
                        column: x => x.PublishedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductKitVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductKitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    SellPrice_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SymbolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductPhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MainComponentVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKitVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKitVersions_ComponentVersions_MainComponentVersionId",
                        column: x => x.MainComponentVersionId,
                        principalTable: "ComponentVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignerData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Json = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    JsonSchemaVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignerData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignerData_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignerData_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectPublicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    File_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    File_FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProcessingEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PercentComplete_Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_ProjectPublications_ProjectPublicationId",
                        column: x => x.ProjectPublicationId,
                        principalTable: "ProjectPublications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKitComponentMaps_ProductKitVersions_ProductKitVersionId",
                        column: x => x.ProductKitVersionId,
                        principalTable: "ProductKitVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductKitReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductKitVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DateCreatedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKitReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKitReferences_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKitReferences_ProductKitVersions_ProductKitVersionId",
                        column: x => x.ProductKitVersionId,
                        principalTable: "ProductKitVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductKitReferences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_OrganizationId",
                table: "Categories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SymbolId",
                table: "Categories",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentTypeId",
                table: "Components",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_OrganizationId",
                table: "Components",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentTypes_OrganizationId_Name",
                table: "ComponentTypes",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentVersions_ComponentId_VersionName",
                table: "ComponentVersions",
                columns: new[] { "ComponentId", "VersionName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentVersions_OrganizationId",
                table: "ComponentVersions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultProjectDescriptions_OrganizationId",
                table: "DefaultProjectDescriptions",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignerData_OrganizationId",
                table: "DesignerData",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerData_PageId_Type",
                table: "DesignerData",
                columns: new[] { "PageId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imports_OrganizationId",
                table: "Imports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Imports_ProjectId",
                table: "Imports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LogoSets_OrganizationId_Name",
                table: "LogoSets",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotForConstructionDisclaimers_OrganizationId",
                table: "NotForConstructionDisclaimers",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                table: "Organizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ShortName_Value",
                table: "Organizations",
                column: "ShortName_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_OrganizationId",
                table: "Pages",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ProjectId",
                table: "Pages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_SheetTypeId",
                table: "Pages",
                column: "SheetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFamilies_OrganizationId_Name",
                table: "ProductFamilies",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

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
                name: "IX_ProductKitReferences_OrganizationId",
                table: "ProductKitReferences",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitReferences_ProductKitVersionId",
                table: "ProductKitReferences",
                column: "ProductKitVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitReferences_ProjectId",
                table: "ProductKitReferences",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKits_CategoryId",
                table: "ProductKits",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKits_OrganizationId",
                table: "ProductKits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKits_ProductFamilyId",
                table: "ProductKits",
                column: "ProductFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKitVersions_MainComponentVersionId",
                table: "ProductKitVersions",
                column: "MainComponentVersionId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_OrganizationId_Name",
                table: "ProductPhotos",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequirements_OrganizationId",
                table: "ProductRequirements",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublications_OrganizationId",
                table: "ProjectPublications",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublications_ProjectId_RevisionNumber",
                table: "ProjectPublications",
                columns: new[] { "ProjectId", "RevisionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublications_PublishedById",
                table: "ProjectPublications",
                column: "PublishedById");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DesignerLockedById",
                table: "Projects",
                column: "DesignerLockedById");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId_Name",
                table: "Projects",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReportOptions_LogoSetId",
                table: "Projects",
                column: "ReportOptions_LogoSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReportOptions_TermsDocumentId",
                table: "Projects",
                column: "ReportOptions_TermsDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OrganizationId",
                table: "Reports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProjectId",
                table: "Reports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProjectPublicationId",
                table: "Reports",
                column: "ProjectPublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SheetTypes_OrganizationId",
                table: "SheetTypes",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_OrganizationId_Name",
                table: "Symbols",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TermsDocuments_OrganizationId_Number",
                table: "TermsDocuments",
                columns: new[] { "OrganizationId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Value",
                table: "Users",
                column: "Email_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "DefaultProjectDescriptions");

            migrationBuilder.DropTable(
                name: "DesignerData");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropTable(
                name: "NotForConstructionDisclaimers");

            migrationBuilder.DropTable(
                name: "ProductKitComponentMaps");

            migrationBuilder.DropTable(
                name: "ProductKitReferences");

            migrationBuilder.DropTable(
                name: "ProductRequirements");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "ProductKitVersions");

            migrationBuilder.DropTable(
                name: "ProjectPublications");

            migrationBuilder.DropTable(
                name: "SheetTypes");

            migrationBuilder.DropTable(
                name: "ComponentVersions");

            migrationBuilder.DropTable(
                name: "ProductKits");

            migrationBuilder.DropTable(
                name: "ProductPhotos");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ProductFamilies");

            migrationBuilder.DropTable(
                name: "LogoSets");

            migrationBuilder.DropTable(
                name: "TermsDocuments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ComponentTypes");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
