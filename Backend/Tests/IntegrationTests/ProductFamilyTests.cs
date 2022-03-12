namespace IntegrationTests;

[TestClass]
public class ProductFamilyTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var productFamilySvc = Container.Resolve<IProductFamilyAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var productFamilyId = await productFamilySvc.AddAsync(HostOrganizationId, "productFamily");
        Assert.IsNotNull(productFamilyId);

        Assert.IsFalse(await productFamilySvc.NameIsAvailableAsync(HostOrganizationId, "productFamily"));

        await productFamilySvc.SetNameAsync(productFamilyId, "newName");
        await productFamilySvc.SetActiveAsync(productFamilyId, false);

        var productFamilyId2 = await productFamilySvc.AddAsync(HostOrganizationId, "productFamily2");

        var productFamilies = await productFamilySvc.ListAsync(HostOrganizationId, 0, 1000, ActiveFilter.All, search: null);

        Assert.AreEqual(2, productFamilies.TotalFilteredCount);

        productFamilies = await productFamilySvc.ListAsync(HostOrganizationId, 0, 1000, ActiveFilter.InactiveOnly, search: null);

        Assert.AreEqual(1, productFamilies.TotalFilteredCount);
        Assert.AreEqual(productFamilyId, productFamilies.Items[0].Id);
        Assert.IsFalse(productFamilies.Items[0].IsActive);
        Assert.AreEqual("newName", productFamilies.Items[0].Name);

        productFamilies = await productFamilySvc.ListAsync(HostOrganizationId, 0, 1000, ActiveFilter.All, "product");

        Assert.AreEqual(1, productFamilies.TotalFilteredCount);
        Assert.AreEqual(productFamilyId2, productFamilies.Items[0].Id);
    }
}
