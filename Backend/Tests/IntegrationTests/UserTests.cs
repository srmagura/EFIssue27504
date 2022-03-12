namespace IntegrationTests
{
    [TestClass]
    public class UserTests : IntegrationTest
    {
        [TestMethod]
        public async Task Crud()
        {
            var userSvc = Container.Resolve<IUserAppService>();

            var email = new EmailAddressDto("jack.test@iticentral.com");
            Assert.IsTrue(await userSvc.UserEmailIsAvailableAsync(email));

            var organizationId = await AddOrganizationAsync();
            var userId = await userSvc.AddAsync(
                organizationId,
                email,
                name: new PersonNameDto("Jack", "Test"),
                role: UserRole.OrganizationAdmin,
                password: DefaultPassword
            );

            Assert.IsNotNull(userId);
            Assert.IsFalse(await userSvc.UserEmailIsAvailableAsync(email));

            var user = await userSvc.GetAsync(userId);
            Assert.IsNotNull(user);
            Assert.AreEqual("Jack", user.Name.First);
            Assert.AreEqual("Test", user.Name.Last);
            Assert.AreEqual(UserRole.OrganizationAdmin, user.Role);
            Assert.IsTrue(user.IsActive);

            await userSvc.SetNameAsync(userId, new PersonNameDto("Jack2", "Test2"));
            await userSvc.SetRoleAsync(userId, UserRole.BasicUser);
            await userSvc.SetActiveAsync(userId, false);

            user = await userSvc.GetAsync(userId);
            Assert.IsNotNull(user);
            Assert.AreEqual("Jack2", user.Name.First);
            Assert.AreEqual("Test2", user.Name.Last);
            Assert.AreEqual(UserRole.BasicUser, user.Role);
            Assert.IsFalse(user.IsActive);
        }
    }
}
