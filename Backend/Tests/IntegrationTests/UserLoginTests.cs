namespace IntegrationTests;

[TestClass]
public class UserLoginTests : IntegrationTest
{
    [TestMethod]
    public async Task LoginSuccess()
    {
        var svc = Container.Resolve<IUserAppService>();

        var organizationId = await AddOrganizationAsync();
        var userId = await AddUserAsync(organizationId, UserRole.BasicUser);

        var user = await svc.GetAsync(userId);
        Assert.IsNotNull(user);

        TestAuthContext.LogOut();

        var loginResult = await svc.LogInAsync(new EmailAddressDto(user.Email.Value), DefaultPassword);

        Assert.IsNotNull(loginResult);
        Assert.AreEqual(LogInResult.Success, loginResult.Result);
        Assert.AreEqual(userId, loginResult.User!.Id);
    }

    [TestMethod]
    public async Task LoginFailsIfWrongPassword()
    {
        var svc = Container.Resolve<IUserAppService>();

        var organizationId = await AddOrganizationAsync();
        var userId = await AddUserAsync(organizationId, UserRole.BasicUser);

        var user = await svc.GetAsync(userId);
        Assert.IsNotNull(user);

        TestAuthContext.LogOut();

        var loginResult = await svc.LogInAsync(new EmailAddressDto(user.Email.Value), "Wrong1234!");

        Assert.IsNotNull(loginResult);
        Assert.AreEqual(LogInResult.InvalidCredentials, loginResult.Result);
        Assert.IsNull(loginResult.User);
    }

    [TestMethod]
    public async Task LoginFailsIfUserInactive()
    {
        var svc = Container.Resolve<IUserAppService>();

        var organizationId = await AddOrganizationAsync();
        var userId = await AddUserAsync(organizationId, UserRole.BasicUser);

        var user = await svc.GetAsync(userId);
        Assert.IsNotNull(user);

        await svc.SetActiveAsync(userId, false);

        TestAuthContext.LogOut();

        var loginResult = await svc.LogInAsync(new EmailAddressDto(user.Email.Value), DefaultPassword);

        Assert.IsNotNull(loginResult);
        Assert.AreEqual(LogInResult.InactiveUser, loginResult.Result);
        Assert.IsNull(loginResult.User);
    }

    [TestMethod]
    public async Task LoginFailsIfOrganizationInactive()
    {
        var userSvc = Container.Resolve<IUserAppService>();
        var organizationSvc = Container.Resolve<IOrganizationAppService>();

        var organizationId = await AddOrganizationAsync();
        var userId = await AddUserAsync(organizationId, UserRole.BasicUser);

        await organizationSvc.SetActiveAsync(organizationId, false);

        var user = await userSvc.GetAsync(userId);
        Assert.IsNotNull(user);

        TestAuthContext.LogOut();

        var loginResult = await userSvc.LogInAsync(new EmailAddressDto(user.Email.Value), DefaultPassword);

        Assert.IsNotNull(loginResult);
        Assert.AreEqual(LogInResult.InactiveOrganization, loginResult.Result);
        Assert.IsNull(loginResult.User);
    }
}
