using DbApplicationImpl.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class VersionNameUtilTests
{
    [TestMethod]
    public void GetNewVersionName_AccountsForGaps()
    {
        var versionNames = new[] { "Jan22", "Jan22 (3)" };
        var versionName = VersionNameUtil.GetNewVersionName(versionNames, "Jan22");

        Assert.AreEqual("Jan22 (2)", versionName);
    }
}
