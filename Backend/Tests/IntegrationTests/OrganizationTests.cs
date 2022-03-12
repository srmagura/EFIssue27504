namespace IntegrationTests
{
    [TestClass]
    public class OrganizationTests : IntegrationTest
    {
        [TestMethod]
        public async Task Crud()
        {
            var organizationSvc = Container.Resolve<IOrganizationAppService>();
            var userSvc = Container.Resolve<IUserAppService>();

            Assert.IsTrue(await organizationSvc.NameIsAvailableAsync("myOrganization"));
            Assert.IsTrue(await organizationSvc.ShortNameIsAvailableAsync("myOrg"));

            var organizationId = await organizationSvc.AddAsync(
                name: "myOrganization",
                shortName: "myOrg",
                ownerEmail: new EmailAddressDto("owner@example2.com"),
                ownerName: new PersonNameDto("Owner", "Todd"),
                ownerPassword: DefaultPassword
            );

            Assert.IsNotNull(organizationId);
            Assert.AreEqual(organizationId, (await organizationSvc.GetByShortNameAsync("myOrg"))?.Id);

            Assert.IsFalse(await organizationSvc.NameIsAvailableAsync("myOrganization"));
            Assert.IsFalse(await organizationSvc.ShortNameIsAvailableAsync("myOrg"));

            var organization = await organizationSvc.GetAsync(organizationId);
            Assert.IsNotNull(organization);
            Assert.AreEqual("myOrganization", organization.Name);
            Assert.AreEqual("myOrg", organization.ShortName);

            await organizationSvc.SetNameAsync(organizationId, "myOrganization2");
            await organizationSvc.SetShortNameAsync(organizationId, "myOrg2");
            await organizationSvc.SetActiveAsync(organizationId, false);

            organization = await organizationSvc.GetAsync(organizationId);
            Assert.IsNotNull(organization);
            Assert.AreEqual("myOrganization2", organization.Name);
            Assert.AreEqual("myOrg2", organization.ShortName);
            Assert.IsFalse(organization.IsActive);

            var ownerId = (await userSvc.ListAsync(
                organizationId,
                skip: 0,
                take: 10,
                activeFilter: ActiveFilter.ActiveOnly,
                search: null
            )).Items.Single().Id;

            var owner = await userSvc.GetAsync(ownerId);
            Assert.IsNotNull(owner);
            Assert.AreEqual(UserRole.OrganizationAdmin, owner.Role);
        }
    }
}
