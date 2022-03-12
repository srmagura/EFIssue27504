using System.Text.Json;
using AppDTOs.Designer;

namespace IntegrationTests;

[TestClass]
public class ProductKitReferenceTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var componentSvc = Container.Resolve<IComponentAppService>();
        var productKitSvc = Container.Resolve<IProductKitAppService>();
        var designerSvc = Container.Resolve<IDesignerAppService>();
        var productKitReferenceSvc = Container.Resolve<IProductKitReferenceAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);
        var symbolId = await AddSymbolAsync(HostOrganizationId);
        var categoryId = await AddCategoryAsync(HostOrganizationId, symbolId);

        var componentId = await AddComponentAsync(HostOrganizationId);
        var component = await componentSvc.GetAsync(componentId);
        Assert.IsNotNull(component);

        var productKitId = await AddProductKitAsync(
            HostOrganizationId,
            categoryId,
            symbolId,
            component.Versions[0].Id
        );
        var productKit = await productKitSvc.GetAsync(productKitId);
        Assert.IsNotNull(productKit);

        var productKitVersion0 = await productKitSvc.GetVersionAsync(productKit.Versions.Single().Id);
        Assert.IsNotNull(productKitVersion0);

        var placedProductKits = new PlacedProductKitDto[]
        {
            new PlacedProductKitDto(
                id: Guid.NewGuid(),
                productKitId: productKitId,
                position: new double[] { 0, 0 },
                rotation: 0,
                lengthInches: null
            )
        };
        var placedProductKitJson = JsonSerializer.Serialize(placedProductKits);

        var pageId = await AddPageAsync(HostOrganizationId, projectId, 0);
        await AcquireDesignerLockAsync(projectId);
        await designerSvc.SetAsync(pageId, DesignerDataType.PlacedProductKits, placedProductKitJson);

        var productKitVersion1VersionName = "1";
        Assert.AreNotEqual(productKitVersion0.VersionName, productKitVersion1VersionName);

        var productKitVersion1Id = await productKitSvc.AddVersionAsync(
            HostOrganizationId,
            productKitId,
            name: productKitVersion0.Name,
            description: productKitVersion0.Description,
            versionName: productKitVersion1VersionName,
            mainComponentVersionId: component.Versions.Single().Id,
            new List<ProductKitComponentMapInputDto>
            {
                new(
                    id: productKitVersion0.ComponentMaps.Single().Id,
                    componentVersionId: component.Versions.Single().Id,
                    count: 1
                )
            }
        );

        var reference = (await productKitReferenceSvc.ListAsync(projectId)).Single();
        Assert.AreEqual(productKitId, reference.ProductKitId);
        Assert.AreEqual(productKitVersion0.Id, reference.ProductKitVersionId);
        Assert.AreEqual(productKitVersion0.VersionName, reference.VersionName);
        Assert.AreEqual(productKitVersion0.Name, reference.ProductKitName);
        Assert.AreEqual(productKitVersion0.MainComponentVersion.DisplayName, reference.MainComponentName);
        Assert.AreEqual(productKitVersion0.SellPrice, reference.SellPrice);
        Assert.AreEqual(productKitVersion1Id, reference.LatestProductKitVersionId);
        Assert.AreEqual(productKitVersion1VersionName, reference.LatestVersionName);
        Assert.IsNull(reference.Tag);

        // The designer should get version 0 of the product kit
        var designerProductKit = (await productKitSvc.ListForDesignerAsync(projectId)).Single();
        Assert.AreEqual(productKitId, designerProductKit.Id);
        Assert.AreEqual(productKitVersion0.VersionName, designerProductKit.VersionName);
        Assert.IsNull(null, designerProductKit.Tag);

        await productKitReferenceSvc.SetTagAsync(reference.Id, "R1");
        await productKitReferenceSvc.SetProductKitVersionAsync(reference.Id, productKitVersion1Id);

        // The designer should get version 1 of the product kit
        designerProductKit = (await productKitSvc.ListForDesignerAsync(projectId)).Single();
        Assert.AreEqual(productKitVersion1VersionName, designerProductKit.VersionName);
        Assert.AreEqual("R1", designerProductKit.Tag);

        // Downgrade and then update all
        await productKitReferenceSvc.SetProductKitVersionAsync(reference.Id, productKitVersion0.Id);
        await productKitReferenceSvc.UpdateAllAsync(projectId);

        // The designer should get version 1 of the product kit
        designerProductKit = (await productKitSvc.ListForDesignerAsync(projectId)).Single();
        Assert.AreEqual(productKitVersion1VersionName, designerProductKit.VersionName);

        // Remove product kit reference
        await designerSvc.SetAsync(pageId, DesignerDataType.PlacedProductKits, "[]");
        Assert.AreEqual(0, (await productKitReferenceSvc.ListAsync(projectId)).Length);
    }
}
