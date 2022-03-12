using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities
{
    public static class AssertionUtil
    {
        public static async Task<T> ThrowsExceptionAsync<T>(Func<Task> action)
            where T : Exception
        {
            Assert.IsFalse(ConsoleLogWriter.HasErrors);

            var e = await Assert.ThrowsExceptionAsync<T>(action);

            ConsoleLogWriter.ClearErrors();
            return e;
        }

        public static void StreamContainsData(MemoryStream memoryStream)
        {
            Assert.IsTrue(memoryStream.CanRead);
            Assert.IsTrue(memoryStream.ToArray().Length > 0);
        }
    }
}
