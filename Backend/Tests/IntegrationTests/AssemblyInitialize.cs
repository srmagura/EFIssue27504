using DataContext;

namespace IntegrationTests
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
