namespace IntegrationTests;

[TestClass]
public class ComponentTypeTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var componentTypeSvc = Container.Resolve<IComponentTypeAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var componentTypeId = await componentTypeSvc.AddAsync(HostOrganizationId, "componentType");
        Assert.IsNotNull(componentTypeId);

        Assert.IsFalse(await componentTypeSvc.NameIsAvailableAsync(HostOrganizationId, "componentType"));

        await componentTypeSvc.SetNameAsync(componentTypeId, "newName");
        await componentTypeSvc.SetActiveAsync(componentTypeId, false);

        await componentTypeSvc.AddAsync(HostOrganizationId, "componentType2");

        var componentTypes = await componentTypeSvc.ListAsync(HostOrganizationId, ActiveFilter.All);

        Assert.AreEqual(2, componentTypes.Length);

        componentTypes = await componentTypeSvc.ListAsync(HostOrganizationId, ActiveFilter.InactiveOnly);

        Assert.AreEqual(1, componentTypes.Length);
        Assert.AreEqual(componentTypeId, componentTypes[0].Id);
        Assert.IsFalse(componentTypes[0].IsActive);
        Assert.AreEqual("newName", componentTypes[0].Name);
    }
}
