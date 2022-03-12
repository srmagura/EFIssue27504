using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Helpers;

namespace UnitTests;

[TestClass]
public class AutoMapperTests
{
    [TestMethod]
    public void AssertConfigurationIsValid()
    {
        var mapper = AutoMapperUtil.GetMapper();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
