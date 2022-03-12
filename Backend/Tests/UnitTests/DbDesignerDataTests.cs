using DbEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class DbDesignerDataTests
{
    [TestMethod]
    public void CompressAndDecompress()
    {
        var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        var bytes = DbDesignerData.Compress(text);
        var text2 = DbDesignerData.Decompress(bytes);
        Assert.AreEqual(text, text2);
    }
}
