namespace IntegrationTests;

[TestClass]
public class PageTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var pageSvc = Container.Resolve<IPageAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);

        await ImportAsync(projectId, "floorplan.pdf");

        var pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        Assert.AreEqual(3, pages.Count);

        var firstPageId = pages[0].Id;

        using (var pdfStream = new MemoryStream())
        {
            var fileType = await pageSvc.GetPdfAsync(firstPageId, pdfStream);
            AssertionUtil.StreamContainsData(pdfStream);
            Assert.AreEqual("application/pdf", fileType);
        }

        using (var thumbnailStream = new MemoryStream())
        {
            var fileType = await pageSvc.GetThumbnailAsync(firstPageId, thumbnailStream);
            AssertionUtil.StreamContainsData(thumbnailStream);
            Assert.AreEqual("image/jpeg", fileType);
        }

        Assert.AreEqual(0, pages[0].Index);
        await pageSvc.SetIndexAsync(pages[1].Id, true);

        pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        Assert.AreEqual(0, pages[0].Index);
        Assert.AreEqual(1, pages[1].Index);
        Assert.AreEqual(2, pages[2].Index);
        Assert.AreEqual(firstPageId, pages[1].Id);

        await pageSvc.DuplicateAsync(pages[0].Id);
        pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        Assert.AreEqual(0, pages[0].Index);
        Assert.AreEqual(1, pages[1].Index);
        Assert.AreEqual(2, pages[2].Index);
        Assert.AreEqual(3, pages[3].Index);
        Assert.AreEqual(firstPageId, pages[2].Id);

        await pageSvc.SetActiveAsync(firstPageId, false);
        pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        Assert.AreEqual(3, pages.Count);
        Assert.AreEqual(0, pages[0].Index);
        Assert.AreEqual(1, pages[1].Index);
        Assert.AreEqual(2, pages[2].Index);

        await pageSvc.SetActiveAsync(firstPageId, true);
        pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        Assert.AreEqual(firstPageId, pages[^1].Id);
    }

    [TestMethod]
    public async Task SetIndex()
    {
        var pageSvc = Container.Resolve<IPageAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);

        var firstPageId = await AddPageAsync(HostOrganizationId, projectId, index: 0);
        var secondPageId = await AddPageAsync(HostOrganizationId, projectId, index: 1);
        var thirdPageId = await AddPageAsync(HostOrganizationId, projectId, index: 2);

        var pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        CollectionAssert.AreEqual(
            new List<PageId> { firstPageId, secondPageId, thirdPageId },
            pages.Select(p => p.Id).ToList()
        );

        await pageSvc.SetIndexAsync(thirdPageId, decreaseIndex: true);
        pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        CollectionAssert.AreEqual(
            new List<PageId> { firstPageId, thirdPageId, secondPageId },
            pages.Select(p => p.Id).ToList()
        );

        await pageSvc.SetIndexAsync(firstPageId, decreaseIndex: false);
        pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        CollectionAssert.AreEqual(
            new List<PageId>
            {
                thirdPageId,
                firstPageId,
                secondPageId
            },
            pages.Select(p => p.Id).ToList()
        );
    }

    [TestMethod]
    public async Task SetSheetNumberAndName()
    {
        var pageSvc = Container.Resolve<IPageAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var sheetTypeId = await AddSheetTypeAsync(HostOrganizationId);
        var projectId = await AddProjectAsync(HostOrganizationId);
        var pageId = await AddPageAsync(HostOrganizationId, projectId, index: 0);

        var page = (await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly)).Single();
        Assert.IsNull(page.SheetTypeId);
        Assert.IsNull(page.SheetNumberSuffix);
        Assert.IsNull(page.SheetNameSuffix);

        await pageSvc.SetSheetNumberAndNameAsync(pageId, sheetTypeId, "00", "mySheetNameSuffix");

        page = (await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly)).Single();
        Assert.AreEqual(sheetTypeId, page.SheetTypeId);
        Assert.AreEqual("00", page.SheetNumberSuffix);
        Assert.AreEqual("mySheetNameSuffix", page.SheetNameSuffix);
    }
}
