﻿// <auto-generated />
using System;
using DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataContext.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20211203202013_MissingProductPhotoIndex")]
    partial class MissingProductPhotoIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DbEntities.DbCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SymbolId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("SymbolId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DbEntities.DbOrganization", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHost")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("DbEntities.DbPage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("DbEntities.DbProductPhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OrganizationId");

                    b.ToTable("ProductPhotos");
                });

            modelBuilder.Entity("DbEntities.DbProject", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EstimatedSquareFeet")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OrganizationId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DbEntities.DbSymbol", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SvgText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OrganizationId");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("DbEntities.DbUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateCreatedUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ITI.DDD.Logging.LogEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hostname")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Level")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Process")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Thread")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("UserId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("UserName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTimeOffset>("WhenUtc")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("WhenUtc");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FriendlyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Xml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("DbEntities.DbCategory", b =>
                {
                    b.HasOne("DbEntities.DbOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DbEntities.DbSymbol", "Symbol")
                        .WithMany()
                        .HasForeignKey("SymbolId");

                    b.Navigation("Organization");

                    b.Navigation("Symbol");
                });

            modelBuilder.Entity("DbEntities.DbOrganization", b =>
                {
                    b.OwnsOne("ValueObjects.OrganizationShortName", "ShortName", b1 =>
                        {
                            b1.Property<Guid>("DbOrganizationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("nvarchar(16)");

                            b1.HasKey("DbOrganizationId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasFilter("[ShortName_Value] IS NOT NULL");

                            b1.ToTable("Organizations");

                            b1.WithOwner()
                                .HasForeignKey("DbOrganizationId");
                        });

                    b.Navigation("ShortName");
                });

            modelBuilder.Entity("DbEntities.DbPage", b =>
                {
                    b.HasOne("DbEntities.DbOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DbEntities.DbProject", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DbEntities.ValueObjects.DbFileRef", "Pdf", b1 =>
                        {
                            b1.Property<Guid>("DbPageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FileId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FileType")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("nvarchar(32)");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.HasKey("DbPageId");

                            b1.ToTable("Pages");

                            b1.WithOwner()
                                .HasForeignKey("DbPageId");
                        });

                    b.OwnsOne("DbEntities.ValueObjects.DbFileRef", "Thumbnail", b1 =>
                        {
                            b1.Property<Guid>("DbPageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FileId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FileType")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("nvarchar(32)");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.HasKey("DbPageId");

                            b1.ToTable("Pages");

                            b1.WithOwner()
                                .HasForeignKey("DbPageId");
                        });

                    b.Navigation("Organization");

                    b.Navigation("Pdf")
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Thumbnail")
                        .IsRequired();
                });

            modelBuilder.Entity("DbEntities.DbProductPhoto", b =>
                {
                    b.HasOne("DbEntities.DbOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DbEntities.ValueObjects.DbFileRef", "Photo", b1 =>
                        {
                            b1.Property<Guid>("DbProductPhotoId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FileId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FileType")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("nvarchar(32)");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.HasKey("DbProductPhotoId");

                            b1.ToTable("ProductPhotos");

                            b1.WithOwner()
                                .HasForeignKey("DbProductPhotoId");
                        });

                    b.Navigation("Organization");

                    b.Navigation("Photo")
                        .IsRequired();
                });

            modelBuilder.Entity("DbEntities.DbProject", b =>
                {
                    b.HasOne("DbEntities.DbOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DbEntities.ValueObjects.DbFileRef", "Photo", b1 =>
                        {
                            b1.Property<Guid>("DbProjectId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FileId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FileType")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("nvarchar(32)");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.HasKey("DbProjectId");

                            b1.ToTable("Projects");

                            b1.WithOwner()
                                .HasForeignKey("DbProjectId");
                        });

                    b.OwnsOne("ValueObjects.PartialAddress", "Address", b1 =>
                        {
                            b1.Property<Guid>("DbProjectId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<string>("Line1")
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.Property<string>("Line2")
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.Property<string>("State")
                                .HasMaxLength(16)
                                .HasColumnType("nvarchar(16)");

                            b1.HasKey("DbProjectId");

                            b1.ToTable("Projects");

                            b1.WithOwner()
                                .HasForeignKey("DbProjectId");

                            b1.OwnsOne("ValueObjects.PostalCode", "PostalCode", b2 =>
                                {
                                    b2.Property<Guid>("PartialAddressDbProjectId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<bool?>("HasValue")
                                        .HasColumnType("bit");

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(16)
                                        .HasColumnType("nvarchar(16)");

                                    b2.HasKey("PartialAddressDbProjectId");

                                    b2.ToTable("Projects");

                                    b2.WithOwner()
                                        .HasForeignKey("PartialAddressDbProjectId");
                                });

                            b1.Navigation("PostalCode");
                        });

                    b.OwnsOne("ValueObjects.ProjectBudgetOptions", "BudgetOptions", b1 =>
                        {
                            b1.Property<Guid>("DbProjectId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<bool>("ShowPricePerSquareFoot")
                                .HasColumnType("bit");

                            b1.Property<bool>("ShowPricingInBudgetBreakdown")
                                .HasColumnType("bit");

                            b1.HasKey("DbProjectId");

                            b1.ToTable("Projects");

                            b1.WithOwner()
                                .HasForeignKey("DbProjectId");

                            b1.OwnsOne("ValueObjects.Percentage", "CostAdjustment", b2 =>
                                {
                                    b2.Property<Guid>("ProjectBudgetOptionsDbProjectId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<bool?>("HasValue")
                                        .HasColumnType("bit");

                                    b2.Property<decimal>("Value")
                                        .HasColumnType("decimal(18,4)");

                                    b2.HasKey("ProjectBudgetOptionsDbProjectId");

                                    b2.ToTable("Projects");

                                    b2.WithOwner()
                                        .HasForeignKey("ProjectBudgetOptionsDbProjectId");
                                });

                            b1.OwnsOne("ValueObjects.Percentage", "DepositPercentage", b2 =>
                                {
                                    b2.Property<Guid>("ProjectBudgetOptionsDbProjectId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<bool?>("HasValue")
                                        .HasColumnType("bit");

                                    b2.Property<decimal>("Value")
                                        .HasColumnType("decimal(18,4)");

                                    b2.HasKey("ProjectBudgetOptionsDbProjectId");

                                    b2.ToTable("Projects");

                                    b2.WithOwner()
                                        .HasForeignKey("ProjectBudgetOptionsDbProjectId");
                                });

                            b1.Navigation("CostAdjustment")
                                .IsRequired();

                            b1.Navigation("DepositPercentage")
                                .IsRequired();
                        });

                    b.OwnsOne("ValueObjects.ProjectReportOptions", "ReportOptions", b1 =>
                        {
                            b1.Property<Guid>("DbProjectId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("CompassAngle")
                                .HasColumnType("int");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<bool>("IncludeCompassInFooter")
                                .HasColumnType("bit");

                            b1.Property<string>("PreparerName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SigneeName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("TitleBlockSheetNameFontSize")
                                .HasColumnType("int");

                            b1.HasKey("DbProjectId");

                            b1.ToTable("Projects");

                            b1.WithOwner()
                                .HasForeignKey("DbProjectId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("BudgetOptions")
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Photo");

                    b.Navigation("ReportOptions")
                        .IsRequired();
                });

            modelBuilder.Entity("DbEntities.DbSymbol", b =>
                {
                    b.HasOne("DbEntities.DbOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("DbEntities.DbUser", b =>
                {
                    b.HasOne("DbEntities.DbOrganization", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("ITI.Baseline.Passwords.EncodedPassword", "EncodedPassword", b1 =>
                        {
                            b1.Property<Guid>("DbUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("nvarchar(128)");

                            b1.HasKey("DbUserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("DbUserId");
                        });

                    b.OwnsOne("ITI.Baseline.ValueObjects.EmailAddress", "Email", b1 =>
                        {
                            b1.Property<Guid>("DbUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)");

                            b1.HasKey("DbUserId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("DbUserId");
                        });

                    b.OwnsOne("ValueObjects.PersonName", "Name", b1 =>
                        {
                            b1.Property<Guid>("DbUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("First")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.Property<bool?>("HasValue")
                                .HasColumnType("bit");

                            b1.Property<string>("Last")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.HasKey("DbUserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("DbUserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("EncodedPassword");

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("DbEntities.DbOrganization", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
