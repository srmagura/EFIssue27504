using ITI.Baseline.Util;

namespace AppServices;

public class ProductKitAppService : ApplicationService, IProductKitAppService
{
    private readonly IAppPermissions _perms;
    private readonly IProductKitQueries _queries;
    private readonly IProductKitRepository _repo;
    private readonly IComponentQueries _componentQueries;
    private readonly ISymbolRepository _symbolRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductPhotoRepository _productPhotoRepo;
    private readonly IComponentRepository _componentRepo;
    private readonly IProductFamilyRepository _productFamilyRepo;

    public ProductKitAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProductKitQueries queries,
        IProductKitRepository repo,
        IComponentQueries componentQueries,
        ISymbolRepository symbolRepo,
        ICategoryRepository categoryRepo,
        IProductPhotoRepository productPhotoRepo,
        IComponentRepository componentRepo,
        IProductFamilyRepository productFamilyRepo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _componentQueries = componentQueries;
        _symbolRepo = symbolRepo;
        _categoryRepo = categoryRepo;
        _productPhotoRepo = productPhotoRepo;
        _componentRepo = componentRepo;
        _productFamilyRepo = productFamilyRepo;
    }

    public Task<ProductKitDto?> GetAsync(ProductKitId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productKit = await _queries.GetAsync(id);
                if (productKit == null) return null;

                Authorize.Require(await _perms.CanViewProductKitsAsync(productKit.OrganizationId));

                return productKit;
            }
        );
    }

    public Task<ProductKitVersionDto?> GetVersionAsync(ProductKitVersionId versionId)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var version = await _queries.GetVersionAsync(versionId);
                if (version == null) return null;

                Authorize.Require(await _perms.CanViewProductKitsAsync(version.OrganizationId));

                return version;
            }
        );
    }

    public Task<ProductKitSummaryDto[]> ListAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductKitsAsync(organizationId)),
            () => _queries.ListAsync(organizationId)
        );
    }

    public Task<ProductKitDesignerDto[]> ListForDesignerAsync(ProjectId projectId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductKitsAsync(projectId)),
            () => _queries.ListForDesignerAsync(projectId)
        );
    }

    public Task<ProductKitVersionReferenceDto[]> ListForComponentAsync(ComponentId componentId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(componentId)),
            () => _queries.ListForComponentAsync(componentId)
        );
    }

    public Task<ProductKitVersionReferenceDto[]> ListForComponentVersionAsync(ComponentVersionId componentVersionId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(componentVersionId)),
            () => _queries.ListForComponentVersionAsync(componentVersionId)
        );
    }

    public Task<string> GetNewVersionNameAsync(ProductKitId id)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(id)),
            () => _queries.GetNewVersionNameAsync(id)
        );
    }

    public Task<ProductKitId> AddAsync(
        OrganizationId organizationId,
        CategoryId categoryId,
        string name,
        string description,
        string versionName,
        SymbolId symbolId,
        ProductPhotoId? productPhotoId,
        ComponentVersionId mainComponentVersionId,
        List<ProductKitComponentMapInputDto> componentMaps,
        ProductFamilyId? productFamilyId
    )
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageProductKitsAsync(organizationId)),
            async () =>
            {
                var componentVersions = await _componentRepo.ListVersionsByIdAsync(
                    componentMaps.Select(p => p.ComponentVersionId).ToArray()
                );

                var componentMapEntities = componentVersions
                    .Join(
                        componentMaps,
                        componentVersion => componentVersion.Id,
                        inputMap => inputMap.ComponentVersionId,
                        (componentVersion, inputMap) => new ProductKitComponentMap(organizationId, componentVersion, inputMap.Count)
                    )
                    .ToList();

                var components = await _componentQueries.ListComponentsByVersionId(
                    componentMaps.Select(p => p.ComponentVersionId).ToList()
                );

                var measurementType = ProductKit.GetMeasurementType(components.Select(c => c.MeasurementType).ToList());

                var category = await _categoryRepo.GetAsync(categoryId);
                Require.NotNull(category, "Category doesn't exist.");

                var symbol = await _symbolRepo.GetAsync(symbolId);
                Require.NotNull(symbol, "Symbol doesn't exist.");

                ProductPhoto? productPhoto = null;
                if (productPhotoId != null) productPhoto = await _productPhotoRepo.GetAsync(productPhotoId);

                ProductFamily? productFamily = null;
                if (productFamilyId != null) productFamily = await _productFamilyRepo.GetAsync(productFamilyId);

                var productKit = new ProductKit(organizationId, category, measurementType, productFamily);
                _repo.Add(productKit);

                var version = new ProductKitVersion(
                    organizationId: organizationId,
                    productKitId: productKit.Id,
                    name: name,
                    description: description,
                    versionName: versionName,
                    symbol: symbol,
                    productPhoto: productPhoto,
                    mainComponentVersionId: mainComponentVersionId,
                    componentMaps: componentMapEntities,
                    componentMeasurementTypes: components.Select(p => p.MeasurementType).ToList(),
                    expectedMeasurementType: measurementType,
                    componentVersions: componentVersions
                );
                _repo.AddVersion(version);

                return productKit.Id;
            }
        );
    }

    public Task<ProductKitVersionId> AddVersionAsync(
        OrganizationId organizationId,
        ProductKitId productKitId,
        string name,
        string description,
        string versionName,
        ComponentVersionId mainComponentVersionId,
        List<ProductKitComponentMapInputDto> componentMaps
    )
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageProductKitsAsync(organizationId)),
            async () =>
            {
                var componentVersions = await _componentRepo.ListVersionsByIdAsync(
                    componentMaps.Select(p => p.ComponentVersionId).ToArray()
                );

                var componentMapEntities = componentVersions
                    .Join(
                        componentMaps,
                        componentVersion => componentVersion.Id,
                        inputMap => inputMap.ComponentVersionId,
                        (componentVersion, inputMap) => new ProductKitComponentMap(organizationId, componentVersion, inputMap.Count)
                    )
                    .ToList();

                var components = await _componentQueries.ListComponentsByVersionId(
                    componentMaps.Select(p => p.ComponentVersionId).ToList()
                );

                var productKit = await _queries.GetAsync(productKitId);
                Require.NotNull(productKit, "Product kit does not exist.");

                var currentVersion = await _queries.GetVersionAsync(productKit.Versions[0].Id);
                Require.NotNull(currentVersion, "Product kit version does not exist.");

                var symbol = await _symbolRepo.GetAsync(currentVersion.Symbol.Id);
                Require.NotNull(symbol, "Symbol doesn't exist.");

                ProductPhoto? productPhoto = null;
                if (currentVersion.ProductPhoto != null)
                {
                    productPhoto = await _productPhotoRepo.GetAsync(currentVersion.ProductPhoto.Id);
                }

                var version = new ProductKitVersion(
                    organizationId: organizationId,
                    productKitId: productKitId,
                    name: name,
                    description: description,
                    versionName: versionName,
                    symbol: symbol,
                    productPhoto: productPhoto,
                    mainComponentVersionId: mainComponentVersionId,
                    componentMaps: componentMapEntities,
                    componentMeasurementTypes: components.Select(p => p.MeasurementType).ToList(),
                    expectedMeasurementType: productKit.MeasurementType,
                    componentVersions: componentVersions
                );
                _repo.AddVersion(version);

                return version.Id;
            }
        );
    }

    private async Task<ProductKit> GetDomainEntityAsync(ProductKitId id)
    {
        var productKit = await _repo.GetAsync(id);
        Require.NotNull(productKit, "Could not find product kit.");
        Authorize.Require(await _perms.CanManageProductKitsAsync(productKit.OrganizationId));

        return productKit;
    }

    private async Task<ProductKitVersion> GetDomainEntityAsync(ProductKitVersionId versionId)
    {
        var version = await _repo.GetVersionAsync(versionId);
        Require.NotNull(version, "Could not find product kit version.");
        Authorize.Require(await _perms.CanManageProductKitsAsync(version.OrganizationId));

        return version;
    }

    public Task SetCategoryAsync(ProductKitId id, CategoryId categoryId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productKit = await GetDomainEntityAsync(id);

                var category = await _categoryRepo.GetAsync(categoryId);
                Require.NotNull(category, "Could not find category.");

                productKit.SetCategory(category);
            }
        );
    }

    public Task SetNameAsync(ProductKitVersionId versionId, string name)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(versionId)).SetName(name)
        );
    }

    public Task SetDescriptionAsync(ProductKitVersionId versionId, string description)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(versionId)).SetDescription(description)
        );
    }

    public Task SetSymbolAsync(ProductKitVersionId versionId, SymbolId symbolId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var version = await GetDomainEntityAsync(versionId);

                var symbol = await _symbolRepo.GetAsync(symbolId);
                Require.NotNull(symbol, "Could not find symbol.");

                version.SetSymbol(symbol);
            }
        );
    }

    public Task SetProductPhotoAsync(ProductKitVersionId versionId, ProductPhotoId? productPhotoId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var version = await GetDomainEntityAsync(versionId);

                ProductPhoto? productPhoto = null;
                if (productPhotoId != null)
                {
                    productPhoto = await _productPhotoRepo.GetAsync(productPhotoId);
                    Require.NotNull(productPhoto, "Could not find product photo.");
                }

                version.SetProductPhoto(productPhoto);
            }
        );
    }

    public Task SetActiveAsync(ProductKitId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetProductFamilyAsync(ProductKitId id, ProductFamilyId? productFamilyId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productKit = await GetDomainEntityAsync(id);

                ProductFamily? productFamily = null;
                if (productFamilyId != null)
                {
                    productFamily = await _productFamilyRepo.GetAsync(productFamilyId);
                    Require.NotNull(productFamily, "Could not find product family.");
                }

                productKit.SetProductFamily(productFamily);
            }
        );
    }
}
