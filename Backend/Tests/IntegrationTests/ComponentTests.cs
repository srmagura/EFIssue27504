namespace IntegrationTests;

[TestClass]
public class ComponentTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var componentSvc = Container.Resolve<IComponentAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var componentTypeId = await AddComponentTypeAsync(HostOrganizationId);

        var componentId = await componentSvc.AddAsync(
            HostOrganizationId,
            componentTypeId,
            MeasurementType.Normal,
            true,
            true,
            "Component1",
            DateTime.UtcNow.ToString("MMMyy"),
            100.001m,
            "https://cooltvs.com",
            "make",
            "model",
            "vendorPartNumber",
            organizationPartNumber: null,
            whereToBuy: null,
            style: null,
            color: null,
            internalNotes: null
        );

        Assert.IsNotNull(componentId);

        var newVersionName = await componentSvc.GetNewVersionNameAsync(componentId);
        Assert.AreEqual(DateTime.UtcNow.ToString("MMMyy") + " (2)", newVersionName);

        var versionId = await componentSvc.AddVersionAsync(
            componentId,
            "Component1",
            newVersionName,
            125m,
            "https://cooltvs.com",
            "make",
            "model",
            "vendorPartNumber",
            organizationPartNumber: null,
            whereToBuy: null,
            style: null,
            color: null,
            internalNotes: null
        );

        var componentSummary = (await componentSvc.ListAsync(HostOrganizationId)).Single();

        Assert.AreEqual("Component1", componentSummary.DisplayName);
        Assert.AreEqual(newVersionName, componentSummary.CurrentVersionName);
        Assert.AreEqual(125m, componentSummary.SellPrice);

        await componentSvc.UpdateVersionAsync(
            versionId,
            "NewName",
            "http://oktvs.com",
            "make",
            "model",
            "vendorPartNumber",
            organizationPartNumber: null,
            whereToBuy: null,
            style: null,
            color: null,
            internalNotes: null
        );

        componentSummary = (await componentSvc.ListAsync(HostOrganizationId)).Single();

        Assert.AreEqual("Component type 1", componentSummary.ComponentTypeName);
        Assert.AreEqual("NewName", componentSummary.DisplayName);
        Assert.AreEqual(newVersionName, componentSummary.CurrentVersionName);
        Assert.AreEqual(125m, componentSummary.SellPrice);

        var component = await componentSvc.GetAsync(componentId);

        Assert.IsNotNull(component);
        Assert.AreEqual(componentTypeId, component.ComponentType.Id);
        Assert.AreEqual(MeasurementType.Normal, component.MeasurementType);
        Assert.IsTrue(component.IsVideoDisplay);
        Assert.IsTrue(component.IsActive);
        Assert.AreEqual(2, component.Versions.Count);

        await componentSvc.SetActiveAsync(componentId, false);

        component = await componentSvc.GetAsync(componentId);

        Assert.IsNotNull(component);
        Assert.IsFalse(component.IsActive);
    }
}
