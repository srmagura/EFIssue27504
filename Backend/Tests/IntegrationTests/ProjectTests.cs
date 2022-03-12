using Settings;

namespace IntegrationTests;

[TestClass]
public class ProjectTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var projectSvc = Container.Resolve<IProjectAppService>();
        var termsDocSvc = Container.Resolve<ITermsDocumentAppService>();
        var projectSettings = Container.Resolve<ProjectSettings>();
        var companyContactInfo = new CompanyContactInfoDto("companyName", "https://testwebsite.com", new EmailAddressDto("test@example.com"), new PhoneNumberDto("5555555555"));

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        Assert.IsTrue(await projectSvc.NameIsAvailableAsync(HostOrganizationId, "myProject"));

        var logoSetId = await AddLogoSetAsync(HostOrganizationId);
        var termsDocumentId = await AddTermsDocumentAsync(HostOrganizationId);
        var termsDocument = (await termsDocSvc.ListAsync(HostOrganizationId, 0, 1000, ActiveFilter.All)).Items.First(p => p.Id == termsDocumentId);

        var projectId = await projectSvc.AddAsync(
            HostOrganizationId,
            name: "myProject",
            shortName: "myProj",
            description: "description",
            address: new PartialAddressDto("Line 1", null, null, null, null),
            customerName: "customerName",
            signeeName: "signeeName",
            logoSetId: logoSetId,
            termsDocumentId: termsDocumentId,
            estimatedSquareFeet: 1000
        );

        Assert.IsNotNull(projectId);
        Assert.IsFalse(await projectSvc.NameIsAvailableAsync(HostOrganizationId, "myProject"));

        var project = await projectSvc.GetAsync(projectId);
        Assert.IsNotNull(project);
        Assert.AreEqual("myProject", project.Name);
        Assert.AreEqual("myProj", project.ShortName);
        Assert.AreEqual("description", project.Description);
        Assert.IsNull(project.Photo);

        Assert.AreEqual("Line 1", project.Address.Line1);
        Assert.IsNull(project.Address.Line2);
        Assert.IsNull(project.Address.City);
        Assert.IsNull(project.Address.State);
        Assert.IsNull(project.Address.PostalCode);

        Assert.AreEqual("customerName", project.CustomerName);
        Assert.AreEqual(1000, project.EstimatedSquareFeet);

        Assert.AreEqual(0m, project.BudgetOptions.CostAdjustment);
        Assert.AreEqual(projectSettings.DefaultDepositPercentage, project.BudgetOptions.DepositPercentage);
        Assert.AreEqual(true, project.BudgetOptions.ShowPricingInBudgetBreakdown);
        Assert.AreEqual(true, project.BudgetOptions.ShowPricePerSquareFoot);

        Assert.AreEqual("Automation User", project.ReportOptions.PreparerName);
        Assert.AreEqual("signeeName", project.ReportOptions.SigneeName);
        Assert.AreEqual(20, project.ReportOptions.TitleBlockSheetNameFontSize);
        Assert.AreEqual(true, project.ReportOptions.IncludeCompassInFooter);
        Assert.AreEqual(0, project.ReportOptions.CompassAngle);
        Assert.IsNull(project.ReportOptions.CompanyContactInfo);

        //

        await projectSvc.SetNameAsync(projectId, "myProject2", "myProj2");
        await projectSvc.SetDescriptionAsync(projectId, "description2");
        await projectSvc.SetCustomerNameAsync(projectId, "customerName2");
        await projectSvc.SetEstimatedSquareFeetAsync(projectId, 1500);
        await projectSvc.SetBudgetOptionsAsync(projectId, new ProjectBudgetOptionsDto(0.5m, 0.5m, false, false));
        await projectSvc.SetReportOptionsAsync(projectId, new ProjectReportOptionsDto("signeeName2", "preparerName", logoSetId, termsDocumentId, termsDocument.Number, 30, false, 5, companyContactInfo));
        using (var stream = GetResourceStream("toad.png"))
            await projectSvc.SetPhotoAsync(projectId, stream, "image/png");
        await projectSvc.SetActiveAsync(projectId, false);

        project = await projectSvc.GetAsync(projectId);
        Assert.IsNotNull(project);
        Assert.AreEqual("myProject2", project.Name);
        Assert.AreEqual("myProj2", project.ShortName);
        Assert.AreEqual("description2", project.Description);
        Assert.IsNotNull(project.Photo);
        Assert.AreEqual("image/png", project.Photo.FileType);

        Assert.AreEqual("customerName2", project.CustomerName);
        Assert.AreEqual(1500, project.EstimatedSquareFeet);

        Assert.AreEqual(0.5m, project.BudgetOptions.CostAdjustment);
        Assert.AreEqual(0.5m, project.BudgetOptions.DepositPercentage);
        Assert.AreEqual(false, project.BudgetOptions.ShowPricingInBudgetBreakdown);
        Assert.AreEqual(false, project.BudgetOptions.ShowPricePerSquareFoot);

        Assert.AreEqual("preparerName", project.ReportOptions.PreparerName);
        Assert.AreEqual("signeeName2", project.ReportOptions.SigneeName);
        Assert.AreEqual(30, project.ReportOptions.TitleBlockSheetNameFontSize);
        Assert.AreEqual(false, project.ReportOptions.IncludeCompassInFooter);
        Assert.AreEqual(5, project.ReportOptions.CompassAngle);

        Assert.IsNotNull(project.ReportOptions.CompanyContactInfo);
        Assert.AreEqual(companyContactInfo.Name, project.ReportOptions.CompanyContactInfo.Name);
        Assert.AreEqual(companyContactInfo.Url, project.ReportOptions.CompanyContactInfo.Url);
        Assert.AreEqual(companyContactInfo.Email.Value, project.ReportOptions.CompanyContactInfo.Email.Value);
        Assert.AreEqual(companyContactInfo.Phone.Value, project.ReportOptions.CompanyContactInfo.Phone.Value);

        using (var stream = new MemoryStream())
        {
            var projectPhoto = await projectSvc.GetPhotoAsync(projectId, stream);
            Assert.AreEqual("image/png", projectPhoto);
            AssertionUtil.StreamContainsData(stream);
        }

        //

        var projectList = await projectSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, null);
        Assert.AreEqual(1, projectList.TotalFilteredCount);
        Assert.AreEqual(projectId.Guid, projectList.Items[0].Id.Guid);

        projectList = await projectSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.ActiveOnly, null);
        Assert.AreEqual(0, projectList.TotalFilteredCount);

        projectList = await projectSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, "roject2");
        Assert.AreEqual(1, projectList.TotalFilteredCount);
        Assert.AreEqual(projectId.Guid, projectList.Items[0].Id.Guid);

        projectList = await projectSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, "roject3");
        Assert.AreEqual(0, projectList.TotalFilteredCount);
    }

    [TestMethod] // Regression test
    public async Task EmptyAddressFields()
    {
        var projectSvc = Container.Resolve<IProjectAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var logoSetId = await AddLogoSetAsync(HostOrganizationId);
        var termsDocumentId = await AddTermsDocumentAsync(HostOrganizationId);

        var projectId = await projectSvc.AddAsync(
            HostOrganizationId,
            name: "myProject",
            shortName: "myProj",
            description: "description",
            address: new PartialAddressDto("", "", "", "", ""), // Fields are empty instead of null
            customerName: "customerName",
            signeeName: "signeeName",
            logoSetId: logoSetId,
            termsDocumentId: termsDocumentId,
            estimatedSquareFeet: 1000
        );

        Assert.IsNotNull(projectId);
    }

    [TestMethod]
    public async Task DesignerLock()
    {
        var projectSvc = Container.Resolve<IProjectAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);
        var project = await projectSvc.GetAsync(projectId);
        Assert.IsNull(project!.DesignerLockedBy);
        Assert.IsNull(project.DesignerLockedUtc);

        await projectSvc.AcquireDesignerLockAsync(projectId);
        project = await projectSvc.GetAsync(projectId);
        Assert.IsNotNull(project!.DesignerLockedBy);
        Assert.AreEqual(AutomationUser.Id, project.DesignerLockedBy.Id);
        Assert.IsNotNull(project.DesignerLockedUtc);

        var otherUserId = await AddUserAsync(HostOrganizationId, UserRole.BasicUser);
        var otherUser = await GetUserAsync(otherUserId);
        TestAuthContext.SetUser(otherUser);

        await projectSvc.AcquireDesignerLockAsync(projectId);
        project = await projectSvc.GetAsync(projectId);
        Assert.IsNotNull(project!.DesignerLockedBy);
        Assert.AreEqual(otherUserId, project.DesignerLockedBy.Id);
        Assert.IsNotNull(project.DesignerLockedUtc);

        await projectSvc.ReleaseDesignerLockAsync(projectId);
        project = await projectSvc.GetAsync(projectId);
        Assert.IsNull(project!.DesignerLockedBy);
        Assert.IsNull(project.DesignerLockedUtc);
    }
}
