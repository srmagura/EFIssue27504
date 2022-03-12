using InfraInterfaces;

namespace IntegrationTests
{
    [TestClass]
    public class ProductPhotoTests : IntegrationTest
    {
        [TestMethod]
        public async Task Crud()
        {
            var fileStore = Container.Resolve<IFileStore>();
            var path = Container.Resolve<IFilePathBuilder>();
            var productPhotoSvc = Container.Resolve<IProductPhotoAppService>();

            using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

            Assert.IsTrue(await productPhotoSvc.NameIsAvailableAsync(HostOrganizationId, "myProductPhoto"));

            ProductPhotoId productPhotoId;
            using (var stream = GetResourceStream("frog.jpg"))
            {
                productPhotoId = await productPhotoSvc.AddAsync(
                    HostOrganizationId,
                    name: "myProductPhoto",
                    stream,
                    "image/jpeg"
                );
            }

            Assert.IsNotNull(productPhotoId);
            Assert.IsFalse(await productPhotoSvc.NameIsAvailableAsync(HostOrganizationId, "myProductPhoto"));

            var productPhoto = await productPhotoSvc.GetAsync(productPhotoId);

            Assert.IsNotNull(productPhoto);
            Assert.AreEqual(productPhotoId, productPhoto.Id);

            var originalFileId = productPhoto.Photo.FileId;
            Assert.AreEqual("image/jpeg", productPhoto.Photo.FileType);
            Assert.IsTrue(await fileStore.ExistsAsync(path.ForProductPhoto(productPhoto.Photo.FileId)));

            Assert.AreEqual(HostOrganizationId, productPhoto.OrganizationId);
            Assert.AreEqual("myProductPhoto", productPhoto.Name);
            Assert.AreEqual(true, productPhoto.IsActive);

            using (var stream = new MemoryStream())
            {
                var productPhotoImage = await productPhotoSvc.GetImageAsync(productPhotoId, stream);
                Assert.AreEqual("image/jpeg", productPhotoImage);
                AssertionUtil.StreamContainsData(stream);
            }

            //

            using (var stream = GetResourceStream("toad.png"))
                await productPhotoSvc.SetPhotoAsync(productPhotoId, stream, "image/png");

            await productPhotoSvc.SetNameAsync(productPhotoId, "productPhoto2");
            await productPhotoSvc.SetActiveAsync(productPhotoId, false);

            productPhoto = await productPhotoSvc.GetAsync(productPhotoId);

            Assert.IsNotNull(productPhoto);
            Assert.AreEqual(originalFileId, productPhoto.Photo.FileId);
            Assert.AreEqual("image/png", productPhoto.Photo.FileType);
            Assert.IsTrue(await fileStore.ExistsAsync(path.ForProductPhoto(productPhoto.Photo.FileId)));

            Assert.AreEqual("productPhoto2", productPhoto.Name);
            Assert.AreEqual(false, productPhoto.IsActive);

            using (var stream = new MemoryStream())
            {
                var productPhotoImage = await productPhotoSvc.GetImageAsync(productPhotoId, stream);
                Assert.AreEqual("image/png", productPhotoImage);
                AssertionUtil.StreamContainsData(stream);
            }

            //

            var productPhotoList = await productPhotoSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, null);
            Assert.AreEqual(1, productPhotoList.TotalFilteredCount);
            Assert.AreEqual(productPhotoId, productPhotoList.Items[0].Id);

            productPhotoList = await productPhotoSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.ActiveOnly, null);
            Assert.AreEqual(0, productPhotoList.TotalFilteredCount);

            productPhotoList = await productPhotoSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, "ductPhoto2");
            Assert.AreEqual(1, productPhotoList.TotalFilteredCount);
            Assert.AreEqual(productPhotoId, productPhotoList.Items[0].Id);

            productPhotoList = await productPhotoSvc.ListAsync(HostOrganizationId, 0, 1, ActiveFilter.InactiveOnly, "ductPhoto3");
            Assert.AreEqual(0, productPhotoList.TotalFilteredCount);
        }
    }
}
