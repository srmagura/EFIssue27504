namespace IntegrationTests;

[TestClass]
public class ProductRequirementTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var productRequirementSvc = Container.Resolve<IProductRequirementAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var productRequirementId = await productRequirementSvc.AddAsync(HostOrganizationId, "requirement1", "svgText1");
        Assert.IsNotNull(productRequirementId);

        var productRequirement = await productRequirementSvc.GetAsync(productRequirementId);
        Assert.IsNotNull(productRequirement);
        Assert.AreEqual("requirement1", productRequirement.Label);
        Assert.AreEqual("svgText1", productRequirement.SvgText);

        await productRequirementSvc.SetLabelAsync(productRequirementId, "newLabel");
        await productRequirementSvc.SetSvgTextAsync(productRequirementId, "newSvgText");

        productRequirement = await productRequirementSvc.GetAsync(productRequirementId);
        Assert.IsNotNull(productRequirement);
        Assert.AreEqual("newLabel", productRequirement.Label);
        Assert.AreEqual("newSvgText", productRequirement.SvgText);

        var productRequirementId2 = await productRequirementSvc.AddAsync(HostOrganizationId, "requirement2", "svgText2");

        var productRequirements = await productRequirementSvc.ListAsync(HostOrganizationId);

        Assert.AreEqual(2, productRequirements.Length);
        Assert.AreEqual(productRequirementId, productRequirements[0].Id);
        Assert.AreEqual("newLabel", productRequirements[0].Label);
        Assert.AreEqual(productRequirementId2, productRequirements[1].Id);
        Assert.AreEqual("requirement2", productRequirements[1].Label);

        await productRequirementSvc.SetIndexAsync(productRequirementId2, 0);
        productRequirements = await productRequirementSvc.ListAsync(HostOrganizationId);

        Assert.AreEqual(productRequirementId, productRequirements[1].Id);
        Assert.AreEqual("newLabel", productRequirements[1].Label);
        Assert.AreEqual(productRequirementId2, productRequirements[0].Id);
        Assert.AreEqual("requirement2", productRequirements[0].Label);

        await productRequirementSvc.RemoveAsync(productRequirementId2);
        productRequirements = await productRequirementSvc.ListAsync(HostOrganizationId);

        Assert.AreEqual(1, productRequirements.Length);
        Assert.AreEqual(productRequirementId, productRequirements[0].Id);
        Assert.AreEqual("newLabel", productRequirements[0].Label);
    }
}
