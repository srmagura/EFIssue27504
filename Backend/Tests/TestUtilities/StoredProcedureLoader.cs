using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;

namespace TestUtilities
{
    public static class StoredProcedureLoader
    {
        public static bool IsAzureSql(string connectionString)
        {
            return connectionString.Contains("database.windows.net", StringComparison.InvariantCultureIgnoreCase);
        }

        // DeleteFromTablesUtil uses sp_MSforeachtable, which doesn't exist by
        // default in Azure SQL, so we add it here.
        public static void AddForEachTable(string connectionString)
        {
            if (!IsAzureSql(connectionString)) return;

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sp_MSforeachtable.sql");
            var cmdText = File.ReadAllText(path);

            var server = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(cmdText);
        }
    }
}
