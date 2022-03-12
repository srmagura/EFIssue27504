using ITI.DDD.Core;

namespace IntegrationTests;

[TestClass]
public class ProductKitTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var productKitSvc = Container.Resolve<IProductKitAppService>();
        var componentSvc = Container.Resolve<IComponentAppService>();
        var categorySvc = Container.Resolve<ICategoryAppService>();
        var productFamilySvc = Container.Resolve<IProductFamilyAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var versionName = DateTime.UtcNow.ToString("MMMyy");

        var componentTypeId = await AddComponentTypeAsync(HostOrganizationId);

        var mainComponentId = await componentSvc.AddAsync(
            HostOrganizationId,
            componentTypeId,
            MeasurementType.Normal,
            true,
            true,
            "HD TV",
            versionName,
            1000m,
            "https://bestbuy.com",
            "make",
            "model",
            "vendorPartNumber",
            organizationPartNumber: null,
            whereToBuy: null,
            style: null,
            color: null,
            internalNotes: null
        );

        var componentId1 = await componentSvc.AddAsync(
            HostOrganizationId,
            componentTypeId,
            MeasurementType.Normal,
            false,
            true,
            "TV Mounting Bracket",
            versionName,
            200m,
            "https://bestbuy.com",
            "make",
            "model",
            "vendorPartNumber1",
            organizationPartNumber: null,
            whereToBuy: null,
            style: null,
            color: null,
            internalNotes: null
        );

        var componentId2 = await componentSvc.AddAsync(
            HostOrganizationId,
            componentTypeId,
            MeasurementType.Normal,
            false,
            true,
            "HDMI Cord",
            versionName,
            5m,
            "https://bestbuy.com",
            "make",
            "model",
            "vendorPartNumber2",
            organizationPartNumber: null,
            whereToBuy: null,
            style: null,
            color: null,
            internalNotes: null
        );

        var symbolId = await AddSymbolAsync(HostOrganizationId);
        var productPhotoId = await AddProductPhotoAsync(HostOrganizationId);

        var tree = new TreeInputDto();
        tree.Children.Add(new TreeInputDto());

        tree.Children[0].Category = new CategoryInputDto(new CategoryId(), "HD TVs")
        {
            SymbolId = symbolId,
            Color = "#000000",
            IsActive = true
        };

        await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);
        var actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
        var categoryId = actualTree.Children[0].Category!.Id;

        var mainComponent = await componentSvc.GetAsync(mainComponentId);
        var component1 = await componentSvc.GetAsync(componentId1);
        var component2 = await componentSvc.GetAsync(componentId2);

        var componentMapInputDtos = new List<ProductKitComponentMapInputDto>()
        {
            new ProductKitComponentMapInputDto(null, mainComponent!.Versions[0].Id, 1),
            new ProductKitComponentMapInputDto(null, component1!.Versions[0].Id, 1),
            new ProductKitComponentMapInputDto(null, component2!.Versions[0].Id, 2)
        };

        var productKitId = await productKitSvc.AddAsync(
            HostOrganizationId,
            categoryId,
            "Version1",
            "TV, mounting bracket and HDMI cables",
            versionName,
            symbolId,
            productPhotoId,
            mainComponent.Versions[0].Id,
            componentMapInputDtos,
            productFamilyId: null
        );
        Assert.IsNotNull(productKitId);

        var productKit = await productKitSvc.GetAsync(productKitId);
        Assert.IsNotNull(productKit);
        Assert.AreEqual(categoryId, productKit.CategoryId);
        Assert.IsNull(productKit.ProductFamily);

        var productFamilyId = await productFamilySvc.AddAsync(HostOrganizationId, "productFamily");
        await productKitSvc.SetProductFamilyAsync(productKitId, productFamilyId);

        productKit = await productKitSvc.GetAsync(productKitId);
        Assert.IsNotNull(productKit);
        Assert.IsNotNull(productKit.ProductFamily);
        Assert.AreEqual(productFamilyId, productKit.ProductFamily.Id);
        Assert.AreEqual("productFamily", productKit.ProductFamily.Name);

        var productKitVersion = await productKitSvc.GetVersionAsync(productKit.Versions[0].Id);
        Assert.IsNotNull(productKitVersion);
        Assert.AreEqual("TV, mounting bracket and HDMI cables", productKitVersion.Description);

        var newVersionName = await productKitSvc.GetNewVersionNameAsync(productKitId);
        Assert.AreEqual($"{versionName} (2)", newVersionName);

        var productKitSummary = (await productKitSvc.ListAsync(HostOrganizationId)).Single();

        Assert.AreEqual(productKitId, productKitSummary.Id);
        Assert.AreEqual(MeasurementType.Normal, productKitSummary.MeasurementType);
        Assert.IsTrue(productKitSummary.IsActive);
        Assert.AreEqual(versionName, productKitSummary.CurrentVersionName);
        Assert.AreEqual("Version1", productKitSummary.Name);
        Assert.AreEqual("HD TV", productKitSummary.MainComponentName);
        Assert.AreEqual(1210m, productKitSummary.SellPrice);

        var versionId = await productKitSvc.AddVersionAsync(
            HostOrganizationId,
            productKitId,
            "Version2",
            "Still an HD TV setup",
            newVersionName,
            mainComponent.Versions[0].Id,
            componentMapInputDtos
        );

        Assert.IsNotNull(versionId);

        var newVersion = await productKitSvc.GetVersionAsync(versionId);
        Assert.IsNotNull(newVersion);

        tree.Children[0].Category = new CategoryInputDto(new CategoryId(), "Home Theater")
        {
            SymbolId = symbolId,
            Color = "#000000",
            IsActive = true
        };
        await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);
        actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
        var categoryId2 = actualTree.Children[0].Category!.Id;

        await productKitSvc.SetCategoryAsync(productKitId, categoryId2);
        await productKitSvc.SetActiveAsync(productKitId, false);

        productKit = await productKitSvc.GetAsync(productKitId);

        Assert.AreEqual(categoryId2, productKit!.CategoryId);
        Assert.IsFalse(productKit.IsActive);

        var symbolId2 = await AddSymbolAsync(HostOrganizationId);
        var productPhotoId2 = await AddProductPhotoAsync(HostOrganizationId);

        await productKitSvc.SetNameAsync(versionId, "HD TV");
        await productKitSvc.SetDescriptionAsync(versionId, "Changed the description");
        await productKitSvc.SetSymbolAsync(versionId, symbolId2);
        await productKitSvc.SetProductPhotoAsync(versionId, productPhotoId2);

        productKitVersion = await productKitSvc.GetVersionAsync(versionId);

        Assert.AreEqual("HD TV", productKitVersion!.Name);
        Assert.AreEqual("Changed the description", productKitVersion.Description);
        Assert.AreEqual(symbolId2, productKitVersion.Symbol.Id);
        Assert.AreEqual(productPhotoId2, productKitVersion.ProductPhoto!.Id);

        var listForComponent = await productKitSvc.ListForComponentVersionAsync(mainComponent.Versions[0].Id);
        Assert.AreEqual(2, listForComponent.Length);
    }

    [TestMethod]
    public async Task MainComponentMustBelongToProductKit()
    {
        var productKitSvc = Container.Resolve<IProductKitAppService>();
        var componentSvc = Container.Resolve<IComponentAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var versionName = DateTime.UtcNow.ToString("MMMyy");

        var mainComponentId = await AddComponentAsync(HostOrganizationId);
        var componentId1 = await AddComponentAsync(HostOrganizationId);

        var symbolId = await AddSymbolAsync(HostOrganizationId);
        var categoryId = await AddCategoryAsync(HostOrganizationId, symbolId);

        var mainComponent = await componentSvc.GetAsync(mainComponentId);
        var component1 = await componentSvc.GetAsync(componentId1);

        var componentMapInputDtos = new List<ProductKitComponentMapInputDto>()
        {
            new ProductKitComponentMapInputDto(null, component1!.Versions[0].Id, 1),
        };

        var e = await AssertionUtil.ThrowsExceptionAsync<ValidationException>(
            () => productKitSvc.AddAsync(
                HostOrganizationId,
                categoryId,
                "Version1",
                "TV, mounting bracket and HDMI cables",
                versionName,
                symbolId,
                productPhotoId: null,
                mainComponent!.Versions[0].Id,
                componentMapInputDtos,
                productFamilyId: null
            )
        );
        Assert.AreEqual("The main component does not correspond to one of the product kit's components.", e.Message);
    }

    [TestMethod]
    public async Task CantChangeMeasurementType()
    {
        var productKitSvc = Container.Resolve<IProductKitAppService>();
        var componentSvc = Container.Resolve<IComponentAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var versionName = DateTime.UtcNow.ToString("MMMyy");

        var normalComponentId = await AddComponentAsync(HostOrganizationId);
        var linearComponentId = await AddComponentAsync(HostOrganizationId, MeasurementType.Linear);

        var symbolId = await AddSymbolAsync(HostOrganizationId);
        var categoryId = await AddCategoryAsync(HostOrganizationId, symbolId);

        var component1 = await componentSvc.GetAsync(normalComponentId);
        var component2 = await componentSvc.GetAsync(linearComponentId);

        var componentMapInputDtos = new List<ProductKitComponentMapInputDto>()
        {
            new ProductKitComponentMapInputDto(null, component1!.Versions[0].Id, 1)
        };

        var productKitId = await productKitSvc.AddAsync(
            HostOrganizationId,
            categoryId,
            "Version1",
            "TV, mounting bracket and HDMI cables",
            versionName,
            symbolId,
            productPhotoId: null,
            component1!.Versions[0].Id,
            componentMapInputDtos,
            productFamilyId: null
        );

        Assert.IsNotNull(productKitId);

        versionName = await productKitSvc.GetNewVersionNameAsync(productKitId);

        componentMapInputDtos = new List<ProductKitComponentMapInputDto>()
        {
            new ProductKitComponentMapInputDto(null, component1!.Versions[0].Id, 1),
            new ProductKitComponentMapInputDto(null, component2!.Versions[0].Id, 1)
        };

        var e = await AssertionUtil.ThrowsExceptionAsync<ValidationException>(
            () => productKitSvc.AddVersionAsync(
                HostOrganizationId,
                productKitId,
                "Version1",
                "TV, mounting bracket and HDMI cables",
                versionName,
                component1!.Versions[0].Id,
                componentMapInputDtos
            )
        );
        Assert.AreEqual("A product kit's measurement type cannot change.", e.Message);
    }
}
