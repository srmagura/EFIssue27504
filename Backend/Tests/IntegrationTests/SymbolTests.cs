namespace IntegrationTests
{
    [TestClass]
    public class SymbolTests : IntegrationTest
    {
        [TestMethod]
        public async Task Crud()
        {
            var symbolSvc = Container.Resolve<ISymbolAppService>();

            using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

            var svgText1 = @"<svg xmlns=""http://www.w3.org/2000/svg""
     xmlns:xlink=""http://www.w3.org/1999/xlink"">

    <text x=""20"" y=""40"">Example SVG text 1</text>
</svg>";
            var svgText2 = @"<svg xmlns=""http://www.w3.org/2000/svg""
     xmlns:xlink=""http://www.w3.org/1999/xlink"">

    <text x=""10"" y=""20"">Example SVG text 2</text>
</svg>";

            Assert.IsTrue(await symbolSvc.NameIsAvailableAsync(HostOrganizationId, "mySymbol"));

            SymbolId symbolId = await symbolSvc.AddAsync(
                HostOrganizationId,
                name: "mySymbol",
                svgText1
            );

            Assert.IsNotNull(symbolId);
            Assert.IsFalse(await symbolSvc.NameIsAvailableAsync(HostOrganizationId, "mySymbol"));

            var symbol = await symbolSvc.GetAsync(symbolId);

            Assert.IsNotNull(symbol);
            Assert.AreEqual(symbolId, symbol.Id);

            Assert.AreEqual("mySymbol", symbol.Name);
            Assert.AreEqual(svgText1, symbol.SvgText);
            Assert.AreEqual(true, symbol.IsActive);

            //

            await symbolSvc.SetNameAsync(symbolId, "symbol2");
            await symbolSvc.SetSvgTextAsync(symbolId, svgText2);
            await symbolSvc.SetActiveAsync(symbolId, false);

            symbol = await symbolSvc.GetAsync(symbolId);

            Assert.IsNotNull(symbol);
            Assert.AreEqual("symbol2", symbol.Name);
            Assert.AreEqual(svgText2, symbol.SvgText);
            Assert.AreEqual(false, symbol.IsActive);

            //

            var symbolList = await symbolSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, null);
            Assert.AreEqual(1, symbolList.TotalFilteredCount);
            Assert.AreEqual(symbolId, symbolList.Items[0].Id);

            symbolList = await symbolSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.ActiveOnly, null);
            Assert.AreEqual(0, symbolList.TotalFilteredCount);

            symbolList = await symbolSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, "mbol2");
            Assert.AreEqual(1, symbolList.TotalFilteredCount);
            Assert.AreEqual(symbolId, symbolList.Items[0].Id);

            symbolList = await symbolSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, "mbol3");
            Assert.AreEqual(0, symbolList.TotalFilteredCount);
        }
    }
}
