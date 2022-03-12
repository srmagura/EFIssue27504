namespace IntegrationTests
{
    [TestClass]
    public class LogoSetTests : IntegrationTest
    {
        [TestMethod]
        public async Task Crud()
        {
            var logoSetSvc = Container.Resolve<ILogoSetAppService>();

            using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

            LogoSetId logoSetId;
            using (var darkLogoStream = GetResourceStream("system7-dark.svg"))
            {
                using (var lightLogoStream = GetResourceStream("system7-light.svg"))
                {
                    logoSetId = await logoSetSvc.AddAsync(
                        HostOrganizationId,
                        "newLogoSet",
                        darkLogoStream,
                        "image/svg",
                        lightLogoStream,
                        "image/svg"
                    );
                }
            }

            Assert.IsNotNull(logoSetId);

            using (var stream = new MemoryStream())
            {
                var darkLogo = await logoSetSvc.GetDarkLogoAsync(logoSetId, stream);
                Assert.AreEqual("image/svg", darkLogo);
                AssertionUtil.StreamContainsData(stream);
            }

            using (var stream = new MemoryStream())
            {
                var lightLogo = await logoSetSvc.GetLightLogoAsync(logoSetId, stream);
                Assert.AreEqual("image/svg", lightLogo);
                AssertionUtil.StreamContainsData(stream);
            }

            var logoSetList = await logoSetSvc.ListAsync(HostOrganizationId);

            Assert.AreEqual("newLogoSet", logoSetList[0].Name);
            Assert.IsTrue(logoSetList[0].IsActive);

            await logoSetSvc.SetNameAsync(logoSetId, "newName");
            await logoSetSvc.SetActiveAsync(logoSetId, false);

            using (var stream = GetResourceStream("mwa-dark.png"))
            {
                await logoSetSvc.SetDarkLogoAsync(logoSetId, stream, "image/png");
            }

            using (var stream = GetResourceStream("mwa-light.png"))
            {
                await logoSetSvc.SetLightLogoAsync(logoSetId, stream, "image/png");
            }

            logoSetList = await logoSetSvc.ListAsync(HostOrganizationId);

            Assert.AreEqual("newName", logoSetList[0].Name);
            Assert.IsFalse(logoSetList[0].IsActive);

            using (var stream = new MemoryStream())
            {
                var darkLogo = await logoSetSvc.GetDarkLogoAsync(logoSetId, stream);
                Assert.AreEqual("image/png", darkLogo);
                AssertionUtil.StreamContainsData(stream);
            }

            using (var stream = new MemoryStream())
            {
                var lightLogo = await logoSetSvc.GetLightLogoAsync(logoSetId, stream);
                Assert.AreEqual("image/png", lightLogo);
                AssertionUtil.StreamContainsData(stream);
            }
        }
    }
}
