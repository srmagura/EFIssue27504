namespace IntegrationTests;

[TestClass]
public class SheetTypeTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var sheetTypeSvc = Container.Resolve<ISheetTypeAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var id = await sheetTypeSvc.AddAsync(HostOrganizationId, "T1.0", "mySheetNamePrefix");

        var sheetType = (await sheetTypeSvc.ListAsync(HostOrganizationId)).Single();
        Assert.AreEqual(id, sheetType.Id);
        Assert.AreEqual("T1.0", sheetType.SheetNumberPrefix);
        Assert.AreEqual("mySheetNamePrefix", sheetType.SheetNamePrefix);
        Assert.IsTrue(sheetType.IsActive);

        await sheetTypeSvc.SetPrefixesAsync(id, "T1.1", "mySheetNamePrefix2");
        await sheetTypeSvc.SetActiveAsync(id, false);

        sheetType = (await sheetTypeSvc.ListAsync(HostOrganizationId)).Single();
        Assert.AreEqual("T1.1", sheetType.SheetNumberPrefix);
        Assert.AreEqual("mySheetNamePrefix2", sheetType.SheetNamePrefix);
        Assert.IsFalse(sheetType.IsActive);
    }
}
