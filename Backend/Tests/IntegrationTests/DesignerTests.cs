using ITI.DDD.Core;

namespace IntegrationTests;

[TestClass]
public class DesignerTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var designerSvc = Container.Resolve<IDesignerAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);
        var pageId = await AddPageAsync(HostOrganizationId, projectId, index: 0);

        var dtos = await designerSvc.ListAsync(projectId);
        Assert.AreEqual(0, dtos.Length);

        var lockNotAcquiredException = await AssertionUtil.ThrowsExceptionAsync<DomainException>(
            () => designerSvc.SetAsync(pageId, DesignerDataType.PlacedProductKits, "foobar")
        );
        Assert.IsTrue(lockNotAcquiredException.Message.Contains("You must own the designer lock"));

        await AcquireDesignerLockAsync(projectId);

        // Exclude PlacedProductKits for convenience since it requires legitimate JSON
        var types = Enum.GetValues<DesignerDataType>()
            .Except(new[] { DesignerDataType.PlacedProductKits })
            .ToArray();

        foreach (var type in types)
        {
            await designerSvc.SetAsync(pageId, type, $"{type}Json");
        }

        dtos = await designerSvc.ListAsync(projectId);

        foreach (var type in types)
        {
            var dto = dtos.First(d => d.Type == type);
            Assert.AreEqual(pageId, dto.PageId);
            Assert.AreEqual(dto.Json, $"{type}Json");
        }

        foreach (var type in types)
        {
            await designerSvc.SetAsync(pageId, type, $"{type}Json2");
        }

        dtos = await designerSvc.ListAsync(projectId);

        foreach (var type in types)
        {
            var dto = dtos.First(d => d.Type == type);
            Assert.AreEqual(pageId, dto.PageId);
            Assert.AreEqual(dto.Json, $"{type}Json2");
        }
    }

    [TestMethod]
    public async Task ListJsonAsync_DoesNotReturnDataForInactivePages()
    {
        var designerSvc = Container.Resolve<IDesignerAppService>();
        var pageSvc = Container.Resolve<IPageAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);
        var pageId = await AddPageAsync(HostOrganizationId, projectId, index: 0);

        await AcquireDesignerLockAsync(projectId);
        await designerSvc.SetAsync(pageId, DesignerDataType.Notes, "json");
        await pageSvc.SetActiveAsync(pageId, false);

        Assert.AreEqual(0, (await designerSvc.ListAsync(projectId)).Length);
    }
}
