namespace IntegrationTests;

[TestClass]
public class TermsDocumentTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var termsDocSvc = Container.Resolve<ITermsDocumentAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        TermsDocumentId termsDocId;
        using (var stream = GetResourceStream("billing-rates.pdf"))
        {
            termsDocId = await termsDocSvc.AddAsync(HostOrganizationId, stream);
        }

        Assert.IsNotNull(termsDocId);

        using (var stream = new MemoryStream())
        {
            var pdf = await termsDocSvc.GetPdfAsync(termsDocId, stream);
            Assert.AreEqual("application/pdf", pdf);
            AssertionUtil.StreamContainsData(stream);
        }

        TermsDocumentId termsDocId2;
        using (var stream = GetResourceStream("construction-drawings.pdf"))
        {
            termsDocId2 = await termsDocSvc.AddAsync(HostOrganizationId, stream);
        }

        Assert.IsNotNull(termsDocId2);
        await termsDocSvc.SetActiveAsync(termsDocId2, false);

        var termsDocList = await termsDocSvc.ListAsync(HostOrganizationId, 0, 2, ActiveFilter.All);
        Assert.AreEqual(2, termsDocList.TotalFilteredCount);

        termsDocList = await termsDocSvc.ListAsync(HostOrganizationId, 0, 2, ActiveFilter.ActiveOnly);
        Assert.AreEqual(1, termsDocList.TotalFilteredCount);
        Assert.AreEqual(termsDocId, termsDocList.Items[0].Id);
        Assert.AreEqual(1, termsDocList.Items[0].Number);

        termsDocList = await termsDocSvc.ListAsync(HostOrganizationId, 0, 2, ActiveFilter.InactiveOnly);
        Assert.AreEqual(1, termsDocList.TotalFilteredCount);
        Assert.AreEqual(termsDocId2, termsDocList.Items[0].Id);
        Assert.AreEqual(2, termsDocList.Items[0].Number);
    }
}
