using DataContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace DevTests
{
    [TestClass]
    public abstract class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void MigrateDatabase(TestContext _)
        {
            var connectionStrings = IntegrationTest.GetConnectionStrings();
            AppDataContext.MigrateForDevelopment(connectionStrings.AppDataContext);

            StoredProcedureLoader.AddForEachTable(connectionStrings.AppDataContext);
        }
    }
}
