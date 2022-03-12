using AppDTOs.Designer;
using AppDTOs.Report;
using DbApplicationImpl.Util;
using ITI.Baseline.Util;

namespace DbApplicationImpl;

public class EfProductKitQueries : Queries<AppDataContext>, IProductKitQueries
{
    private readonly IMapper _mapper;
    private readonly IAppPermissionsQueries _appPermissionsQueries;

    public EfProductKitQueries(IUnitOfWorkProvider uowp, IMapper mapper, IAppPermissionsQueries appPermissionsQueries) : base(uowp)
    {
        _mapper = mapper;
        _appPermissionsQueries = appPermissionsQueries;
    }

    public async Task<ProductKitDto?> GetAsync(ProductKitId id)
    {
        var q = Context.ProductKits
            .Include(p => p.Versions)
            .Where(p => p.Id == id.Guid);

        var dto = await _mapper.ProjectToDtoAsync<DbProductKit, ProductKitDto>(q);
        if (dto == null) return null;

        dto.Versions = dto.Versions.OrderByDescending(v => v.DateCreatedUtc).ToList();
        return dto;
    }

    public async Task<ProductKitVersionDto?> GetVersionAsync(ProductKitVersionId id)
    {
        var q = Context.ProductKitVersions
            .Include(p => p.Symbol)
            .Include(p => p.ProductPhoto)
            .Include(p => p.ComponentMaps)
            .ThenInclude(p => p.ComponentVersion)
            .ThenInclude(p => p!.Component)
            .Where(p => p.Id == id.Guid);

        var dto = await _mapper.ProjectToDtoAsync<DbProductKitVersion, ProductKitVersionDto>(q);
        if (dto == null) return null;

        dto.ComponentMaps = dto.ComponentMaps
            .OrderByDescending(m => m.ComponentVersionId == dto.MainComponentVersion.Id)
            .ThenByDescending(m => m.SellPrice)
            .ToList();

        return dto;
    }

    // Inefficient but simplest way to do it, optimize if it becomes a problem
    public async Task<ProductKitSummaryDto[]> ListAsync(OrganizationId organizationId)
    {
        var q = Context.ProductKits
            .Include(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .ThenInclude(p => p.Symbol)
            .Include(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .ThenInclude(p => p.MainComponentVersion)
            .Where(p => p.OrganizationId == organizationId.Guid);

        return (await q.ToArrayAsync())
            .Where(p => p.Versions.Count == 1)
            .OrderBy(p => p.Versions[0].Name.ToLowerInvariant())
            .Select(p =>
            {
                var v = p.Versions[0];

                return new ProductKitSummaryDto(
                    id: new ProductKitId(p.Id),
                    measurementType: p.MeasurementType,
                    isActive: p.IsActive,
                    currentVersionName: v.VersionName,
                    name: v.Name,
                    sellPrice: v.SellPrice.Value,
                    symbolSvgText: v.Symbol!.SvgText,
                    mainComponentName: $"{v.MainComponentVersion!.Make} {v.MainComponentVersion!.Model} {v.MainComponentVersion!.DisplayName}"
                );
            })
            .ToArray();
    }

    // Inefficient but simplest way to do it, optimize if it becomes a problem
    public async Task<ProductKitDesignerDto[]> ListForDesignerAsync(ProjectId projectId)
    {
        // Optimization: use cache
        var organizationId = await _appPermissionsQueries.OrganizationOfAsync(projectId);

        var references = await Context.ProductKitReferences
            .AsNoTracking()
            .Include(r => r.ProductKitVersion)
            .ThenInclude(v => v!.Symbol)
            .Include(r => r.ProductKitVersion)
            .ThenInclude(v => v!.ProductKit)
            .Include(r => r.ProductKitVersion)
            .ThenInclude(v => v!.MainComponentVersion)
            .Where(r => r.ProjectId == projectId.Guid)
            .ToArrayAsync();

        var allProductKits = await Context.ProductKits
            .AsNoTracking()
            .Include(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .ThenInclude(p => p.Symbol)
            .Include(p => p.Versions
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(1)
            )
            .ThenInclude(p => p.MainComponentVersion)
            .Where(p => p.OrganizationId == organizationId.Guid)
            .ToArrayAsync();

        var referencedProductKitGuids = references
            .Select(r => r.ProductKitVersion!.ProductKitId)
            .ToHashSet();

        // Remove product kits if they are explicitly referenced
        var unreferencedProductKits = allProductKits
            .Where(p => !referencedProductKitGuids.Contains(p.Id))
            .ToArray();

        var unreferencedProductKitDtos = unreferencedProductKits
            .Where(p => p.Versions.Count == 1)
            .Select(p =>
            {
                var v = p.Versions[0];

                return new ProductKitDesignerDto(
                    id: new ProductKitId(p.Id),
                    categoryId: new CategoryId(p.CategoryId),
                    measurementType: p.MeasurementType,
                    isActive: p.IsActive,
                    versionId: new ProductKitVersionId(v.Id),
                    versionName: v.VersionName,
                    name: v.Name,
                    sellPrice: v.SellPrice.Value,
                    productPhotoId: v.ProductPhotoId != null
                        ? new ProductPhotoId(v.ProductPhotoId.Value)
                        : null,
                    symbolSvgText: v.Symbol!.SvgText,
                    mainComponentName: v.MainComponentVersion!.DisplayName,
                    tag: null
                );
            });

        var referencedProductKitDtos = references
            .Select(r =>
            {
                var v = r.ProductKitVersion!;

                return new ProductKitDesignerDto(
                    id: new ProductKitId(v.ProductKitId),
                    categoryId: new CategoryId(v.ProductKit!.CategoryId),
                    measurementType: v.ProductKit.MeasurementType,
                    isActive: v.ProductKit.IsActive,
                    versionId: new ProductKitVersionId(v.Id),
                    versionName: v.VersionName,
                    name: v.Name,
                    sellPrice: v.SellPrice.Value,
                    productPhotoId: v.ProductPhotoId != null
                        ? new ProductPhotoId(v.ProductPhotoId.Value)
                        : null,
                    symbolSvgText: v.Symbol!.SvgText,
                    mainComponentName: v.MainComponentVersion!.DisplayName,
                    tag: r.Tag
                );
            });

        return unreferencedProductKitDtos
            .Concat(referencedProductKitDtos)
            .OrderBy(p => p.Name.ToLowerInvariant())
            .ToArray();
    }

    public async Task<ProductKitReportDto[]> ListForReportAsync(ProjectId projectId)
    {
        return await Context.ProductKitReferences
            .AsNoTracking()
            .Where(r => r.ProjectId == projectId.Guid)
            .Select(r => new ProductKitReportDto(
                    new ProductKitId(r.ProductKitVersion!.ProductKitId),
                    r.ProductKitVersion!.ProductKit!.Category!.Name,
                    r.ProductKitVersion!.ProductKit!.Category!.Color,
                    r.ProductKitVersion!.Name,
                    r.ProductKitVersion!.ProductPhoto != null
                        ? new FileId(r.ProductKitVersion!.ProductPhoto.Photo.FileId)
                        : null,
                    new SymbolId(r.ProductKitVersion!.SymbolId),
                    r.ProductKitVersion!.Symbol!.SvgText,
                    r.ProductKitVersion!.MainComponentVersion!.DisplayName,
                    r.ProductKitVersion!.MainComponentVersion.Url != null
                        ? r.ProductKitVersion!.MainComponentVersion.Url.Value
                        : null
            ))
            .ToArrayAsync();
    }

    public async Task<ProductKitVersionReferenceDto[]> ListForComponentAsync(ComponentId componentId)
    {
        var q = Context.ProductKitVersions
            .Where(p => p.ComponentMaps
                .Any(pp => pp.ComponentVersion!.ComponentId == componentId.Guid)
            )
            .OrderBy(p => p.Name)
            .ThenBy(p => p.DateCreatedUtc);

        return await _mapper.ProjectToDtoArrayAsync<DbProductKitVersion, ProductKitVersionReferenceDto>(q);
    }

    public async Task<ProductKitVersionReferenceDto[]> ListForComponentVersionAsync(ComponentVersionId componentVersionId)
    {
        var q = Context.ProductKitVersions
            .Where(p => p.ComponentMaps
                .Any(pp => pp.ComponentVersionId == componentVersionId.Guid)
            )
            .OrderBy(p => p.Name)
            .ThenBy(p => p.DateCreatedUtc);

        return await _mapper.ProjectToDtoArrayAsync<DbProductKitVersion, ProductKitVersionReferenceDto>(q);
    }

    public async Task<string> GetNewVersionNameAsync(ProductKitId id)
    {
        var nowName = DateTime.UtcNow.ToString("MMMyy");

        var versionNames = await Context.ProductKitVersions
            .Where(p => p.ProductKitId == id.Guid && p.VersionName.StartsWith(nowName))
            .Select(p => p.VersionName)
            .ToArrayAsync();

        return VersionNameUtil.GetNewVersionName(versionNames, nowName);
    }
}
