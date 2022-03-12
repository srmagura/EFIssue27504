using Permissions;

namespace IntegrationTests
{
    [TestClass]
    public class PermissionsTests : IntegrationTest
    {
        [TestMethod]
        public async Task SysAdminViewOtherOrganizationData()
        {
            var perms = Container.Resolve<IAppPermissions>();

            var organizationId = new OrganizationId();

            Assert.IsTrue(await perms.CanViewUsersAsync(organizationId));
            Assert.IsTrue(await perms.CanManageUsersAsync(organizationId));

            Assert.IsTrue(await perms.CanViewProductKitsAsync(organizationId));
            Assert.IsFalse(await perms.CanManageProductKitsAsync(organizationId));

            Assert.IsTrue(await perms.CanViewComponentsAsync(organizationId));
            Assert.IsFalse(await perms.CanManageComponentsAsync(organizationId));

            Assert.IsTrue(await perms.CanViewProductFamiliesAsync(organizationId));
            Assert.IsFalse(await perms.CanManageProductFamiliesAsync(organizationId));

            Assert.IsTrue(await perms.CanViewProductRequirementsAsync(organizationId));
            Assert.IsFalse(await perms.CanManageProductRequirementsAsync(organizationId));

            Assert.IsTrue(await perms.CanViewCategoriesAsync(organizationId));
            Assert.IsFalse(await perms.CanManageCategoriesAsync(organizationId));

            Assert.IsTrue(await perms.CanViewProductPhotosAsync(organizationId));
            Assert.IsFalse(await perms.CanManageProductPhotosAsync(organizationId));

            Assert.IsTrue(await perms.CanViewSymbolsAsync(organizationId));
            Assert.IsFalse(await perms.CanManageSymbolsAsync(organizationId));

            Assert.IsFalse(await perms.CanViewProjectsAsync(organizationId));
            Assert.IsFalse(await perms.CanManageProjectsAsync(organizationId));
        }
    }
}
