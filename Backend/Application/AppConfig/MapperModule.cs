#pragma warning disable CS0618 // Type or member is obsolete - allow using constructors marked as obsolete
using AppDTOs;
using AppDTOs.Designer;
using Autofac;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DbEntities;
using DbEntities.ValueObjects;
using Entities;
using Identities;
using ITI.Baseline.Passwords;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Infrastructure.DataMapping;
using ValueObjects;

namespace AppConfig;

internal class MapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddCollectionMappers();

            // VALUE OBJECTS
            cfg.CreateMap<EncodedPassword, EncodedPassword>();

            cfg.CreateMap<EmailAddress, EmailAddress>();
            cfg.CreateMap<EmailAddress, EmailAddressDto>();

            cfg.CreateMap<PhoneNumber, PhoneNumber>();
            cfg.CreateMap<PhoneNumber, PhoneNumberDto>();

            cfg.CreateMap<PersonName, PersonName>();
            cfg.CreateMap<PersonName, PersonNameDto>();

            cfg.CreateMap<OrganizationShortName, string>().ConvertUsing(n => n.Value);

            cfg.CreateMap<PostalCode, PostalCode>();
            cfg.CreateMap<PostalCode?, string?>().ConvertUsing(p => p == null ? null : p.Value);
            cfg.CreateMap<string?, PostalCode?>().ConvertUsing(p => p == null ? null : new PostalCode(p));

            cfg.CreateMap<PartialAddress, PartialAddress>();
            cfg.CreateMap<PartialAddress, PartialAddressDto>();

            cfg.CreateMap<Percentage, Percentage>();
            cfg.CreateMap<Percentage, decimal>().ConvertUsing(p => p.Value);
            cfg.CreateMap<decimal, Percentage>().ConvertUsing(p => new Percentage(p));

            cfg.CreateMap<Money, Money>();
            cfg.CreateMap<Money, decimal>().ConvertUsing(p => p.Value);
            cfg.CreateMap<decimal, Money>().ConvertUsing(p => new Money(p));

            cfg.CreateMap<Url, Url>();
            cfg.CreateMap<Url?, string?>().ConvertUsing(p => p == null ? null : p.Value);
            cfg.CreateMap<string?, Url?>().ConvertUsing(p => p == null ? null : new Url(p));

            cfg.CreateMap<CompanyContactInfo, CompanyContactInfo>();
            cfg.CreateMap<CompanyContactInfo, CompanyContactInfoDto>();

            cfg.CreateMap<ProjectBudgetOptions, ProjectBudgetOptions>();
            cfg.CreateMap<ProjectBudgetOptions, ProjectBudgetOptionsDto>();

            cfg.CreateMap<ProjectReportOptions, ProjectReportOptions>();

            // USERS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new UserId(guid));
            cfg.CreateMap<User, DbUser>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbUser, UserDto>();
            cfg.CreateMap<DbUser, UserSummaryDto>();
            cfg.CreateMap<DbUser, UserReferenceDto>();

            // ORGANIZATIONS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new OrganizationId(guid));
            cfg.CreateMap<Organization, DbOrganization>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbOrganization, OrganizationDto>();
            cfg.CreateMap<DbOrganization, OrganizationSummaryDto>();
            cfg.CreateMap<DbOrganization, OrganizationReferenceDto>();

            // PROJECTS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProjectId(guid));
            cfg.CreateMap<Project, DbProject>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbProject, ProjectDto>();
            cfg.CreateMap<DbProject, ProjectSummaryDto>();

            // PROJECT REPORT OPTIONS
            cfg.CreateMap<ProjectReportOptions, DbProjectReportOptions>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbProjectReportOptions, ProjectReportOptionsDto>()
                .ForMember(p => p.TermsDocumentNumber, opt => opt.MapFrom(db => db.TermsDocument!.Number));

            // FILES / IMPORT
            MapperConfigurationUtil.MapIdentity(cfg, guid => new FileId(guid));
            cfg.CreateMap<FileRef, DbFileRef>()
                .ReverseMap();
            cfg.CreateMap<DbFileRef, FileRefDto>();

            // SYMBOLS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new SymbolId(guid));
            cfg.CreateMap<Symbol, DbSymbol>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbSymbol, SymbolDto>();
            cfg.CreateMap<DbSymbol, SymbolSummaryDto>();

            // PRODUCT PHOTOS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductPhotoId(guid));
            cfg.CreateMap<ProductPhoto, DbProductPhoto>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbProductPhoto, ProductPhotoDto>();
            cfg.CreateMap<DbProductPhoto, ProductPhotoSummaryDto>();

            // CATEGORIES
            MapperConfigurationUtil.MapIdentity(cfg, guid => new CategoryId(guid));
            cfg.CreateMap<Category, DbCategory>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbCategory, CategoryDto>();

            // IMPORTS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ImportId(guid));

            cfg.CreateMap<Import, DbPageImport>(MemberList.Source)
                .ReverseMap()
                .ConstructUsing((db, ctx) => new Import(
                    placeholder: true,
                    organizationId: new OrganizationId(db.OrganizationId),
                    projectId: new ProjectId(db.ProjectId),
                    filename: db.Filename,
                    file: ctx.Mapper.Map<FileRef>(db.File)
                ));

            cfg.CreateMap<DbPageImport, ImportDto>();

            // PROJECT PUBLICATIONS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProjectPublicationId(guid));

            cfg.CreateMap<ProjectPublication, DbProjectPublication>(MemberList.Source)
                .ReverseMap()
                .ConstructUsing(db => new ProjectPublication(
                    new OrganizationId(db.OrganizationId),
                    new ProjectId(db.ProjectId),
                    new UserId(db.PublishedById)
                ));

            cfg.CreateMap<DbProjectPublication, ProjectPublicationSummaryDto>();

            // REPORTS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ReportId(guid));
            cfg.CreateMap<Report, DbReport>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbReport, ReportSystemDto>();
            cfg.CreateMap<DbReport, ReportDto>();

            // PAGES
            MapperConfigurationUtil.MapIdentity(cfg, guid => new PageId(guid));
            cfg.CreateMap<Page, DbPage>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbPage, PageDto>();
            cfg.CreateMap<DbPage, PageSummaryDto>();

            // COMPONENTS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ComponentId(guid));
            cfg.CreateMap<Component, DbComponent>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbComponent, ComponentDto>();

            // COMPONENT VERSIONS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ComponentVersionId(guid));
            cfg.CreateMap<ComponentVersion, DbComponentVersion>(MemberList.Source)
                .ReverseMap()
                .ConstructUsing((db, ctx) => new ComponentVersion(
                    new OrganizationId(db.OrganizationId),
                    new ComponentId(db.ComponentId),
                    db.DisplayName,
                    db.VersionName,
                    db.SellPrice,
                    db.Make,
                    db.Model,
                    db.VendorPartNumber
                ));

            cfg.CreateMap<DbComponentVersion, ComponentVersionDto>();

            // COMPONENT TYPES
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ComponentTypeId(guid));
            cfg.CreateMap<ComponentType, DbComponentType>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbComponentType, ComponentTypeDto>();

            // PRODUCT KITS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductKitId(guid));
            cfg.CreateMap<ProductKit, DbProductKit>(MemberList.Source)
                .ReverseMap()
                .ConstructUsing((db, ctx) =>
                    new ProductKit(
                        new OrganizationId(db.OrganizationId),
                        new CategoryId(db.CategoryId),
                        ctx.Mapper.Map<ProductFamilyId?>(db.ProductFamilyId)
                    )
                );

            cfg.CreateMap<DbProductKit, ProductKitDto>();

            // PRODUCT KIT VERSIONS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductKitVersionId(guid));
            cfg.CreateMap<ProductKitVersion, DbProductKitVersion>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbProductKitVersion, ProductKitVersionDto>();
            cfg.CreateMap<DbProductKitVersion, ProductKitVersionSummaryDto>();
            cfg.CreateMap<DbProductKitVersion, ProductKitVersionReferenceDto>();

            // PRODUCT KIT COMPONENT MAPS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductKitComponentMapId(guid));

            cfg.CreateMap<ProductKitComponentMap, DbProductKitComponentMap>(MemberList.Source)
                .ReverseMap();

            cfg.CreateMap<DbProductKitComponentMap, ProductKitComponentMapDto>()
                .ForCtorParam("componentId", opt => opt.MapFrom(db => db.ComponentVersion!.ComponentId))
                .ForMember(p => p.ComponentId, opt => opt.MapFrom(db => db.ComponentVersion!.ComponentId))
                .ForMember(p => p.IsVideoDisplay, opt => opt.MapFrom(db => db.ComponentVersion!.Component!.IsVideoDisplay))
                .ForMember(p => p.MeasurementType, opt => opt.MapFrom(db => db.ComponentVersion!.Component!.MeasurementType))
                .ForMember(p => p.VisibleToCustomer, opt => opt.MapFrom(db => db.ComponentVersion!.Component!.VisibleToCustomer))
                .ForCtorParam("displayName", opt => opt.MapFrom(db => db.ComponentVersion!.DisplayName))
                .ForMember(p => p.DisplayName, opt => opt.MapFrom(db => db.ComponentVersion!.DisplayName))
                .ForCtorParam("versionName", opt => opt.MapFrom(db => db.ComponentVersion!.VersionName))
                .ForMember(p => p.VersionName, opt => opt.MapFrom(db => db.ComponentVersion!.VersionName))
                .ForMember(p => p.Url, opt => opt.MapFrom(db => db.ComponentVersion!.Url))
                .ForMember(p => p.SellPrice, opt => opt.MapFrom(db => db.ComponentVersion!.SellPrice))
                .ForCtorParam("make", opt => opt.MapFrom(db => db.ComponentVersion!.Make))
                .ForMember(p => p.Make, opt => opt.MapFrom(db => db.ComponentVersion!.Make))
                .ForCtorParam("model", opt => opt.MapFrom(db => db.ComponentVersion!.Model))
                .ForMember(p => p.Model, opt => opt.MapFrom(db => db.ComponentVersion!.Model))
                .ForCtorParam("vendorPartNumber", opt => opt.MapFrom(db => db.ComponentVersion!.VendorPartNumber))
                .ForMember(p => p.VendorPartNumber, opt => opt.MapFrom(db => db.ComponentVersion!.VendorPartNumber))
                .ForMember(p => p.OrganizationPartNumber, opt => opt.MapFrom(db => db.ComponentVersion!.OrganizationPartNumber));

            // PRODUCT KIT REFERENCES
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductKitReferenceId(guid));

            cfg.CreateMap<ProductKitReference, DbProductKitReference>(MemberList.Source)
                .ReverseMap();

            // DESIGNER
            cfg.CreateMap<DbDesignerData, DesignerDataDto>();

            // TERMS DOCUMENTS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new TermsDocumentId(guid));
            cfg.CreateMap<TermsDocument, DbTermsDocument>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbTermsDocument, TermsDocumentDto>();
            cfg.CreateMap<DbTermsDocument, TermsDocumentSummaryDto>();

            // LOGO SETS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new LogoSetId(guid));
            cfg.CreateMap<LogoSet, DbLogoSet>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbLogoSet, LogoSetDto>();

            // SHEET TYPES
            MapperConfigurationUtil.MapIdentity(cfg, guid => new SheetTypeId(guid));
            cfg.CreateMap<SheetType, DbSheetType>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbSheetType, SheetTypeSummaryDto>();

            // PRODUCT FAMILIES
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductFamilyId(guid));
            cfg.CreateMap<ProductFamily, DbProductFamily>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbProductFamily, ProductFamilyReferenceDto>();

            // ORGANIZATION OPTIONS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new DefaultProjectDescriptionId(guid));
            cfg.CreateMap<DefaultProjectDescription, DbDefaultProjectDescription>(MemberList.Source)
                .ReverseMap();

            MapperConfigurationUtil.MapIdentity(cfg, guid => new NotForConstructionDisclaimerTextId(guid));
            cfg.CreateMap<NotForConstructionDisclaimer, DbNotForConstructionDisclaimer>(MemberList.Source)
                .ReverseMap();

            // PRODUCT REQUIREMENTS
            MapperConfigurationUtil.MapIdentity(cfg, guid => new ProductRequirementId(guid));
            cfg.CreateMap<ProductRequirement, DbProductRequirement>(MemberList.Source)
                .ReverseMap();
            cfg.CreateMap<DbProductRequirement, ProductRequirementDto>();
        });

        var mapper = new Mapper(config);
        builder.RegisterInstance<IMapper>(mapper);
    }
}
