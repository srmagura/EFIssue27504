namespace IntegrationTests;

[TestClass]
public class OrganizationOptionsTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var organizationOptionsSvc = Container.Resolve<IOrganizationOptionsAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var description = await organizationOptionsSvc.GetDefaultProjectDescriptionAsync(HostOrganizationId);
        Assert.IsNull(description);

        await organizationOptionsSvc.SetDefaultProjectDescriptionAsync(HostOrganizationId, "testDescription");

        description = await organizationOptionsSvc.GetDefaultProjectDescriptionAsync(HostOrganizationId);
        Assert.AreEqual("testDescription", description);

        await organizationOptionsSvc.SetDefaultProjectDescriptionAsync(HostOrganizationId, "");

        description = await organizationOptionsSvc.GetDefaultProjectDescriptionAsync(HostOrganizationId);
        Assert.AreEqual("", description);

        var disclaimer = await organizationOptionsSvc.GetNotForConstructionDisclaimerTextAsync(HostOrganizationId);
        Assert.IsNull(disclaimer);

        await organizationOptionsSvc.SetNotForConstructionDisclaimerTextAsync(HostOrganizationId, "testDisclaimer");

        description = await organizationOptionsSvc.GetNotForConstructionDisclaimerTextAsync(HostOrganizationId);
        Assert.AreEqual("testDisclaimer", description);

        await organizationOptionsSvc.SetNotForConstructionDisclaimerTextAsync(HostOrganizationId, "");

        description = await organizationOptionsSvc.GetNotForConstructionDisclaimerTextAsync(HostOrganizationId);
        Assert.AreEqual("", description);
    }
}
