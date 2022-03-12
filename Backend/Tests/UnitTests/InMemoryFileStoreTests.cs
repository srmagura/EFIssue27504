using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace UnitTests;

[TestClass]
public class InMemoryFileStoreTests
{
    [TestMethod]
    public async Task Basic()
    {
        var store = new InMemoryFileStore();

        var path = $"in-memory/test/{Guid.NewGuid()}";

        using (var ms = new MemoryStream())
        {
            using var wr = new StreamWriter(ms);

            wr.WriteLine("Test line 1");
            wr.WriteLine("Test line 2");
            wr.Flush();

            ms.Seek(0, SeekOrigin.Begin);
            await store.PutAsync(path, ms);
        }

        Assert.IsTrue(await store.ExistsAsync(path));

        using (var ms = new MemoryStream())
        {
            await store.GetAsync(path, ms);

            ms.Seek(0, SeekOrigin.Begin);
            using var sr = new StreamReader(ms);

            var line1 = sr.ReadLine();
            Assert.AreEqual("Test line 1", line1);

            var line2 = sr.ReadLine();
            Assert.AreEqual("Test line 2", line2);
        }

        await store.RemoveAsync(path);

        Assert.IsFalse(await store.ExistsAsync(path));
    }
}
