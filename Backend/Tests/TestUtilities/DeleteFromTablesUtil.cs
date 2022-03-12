using System.Text;
using Microsoft.Data.SqlClient;

namespace TestUtilities
{
    public static class DeleteFromTablesUtil
    {
        public static async Task DeleteFromTablesAsync(string connectionString)
        {
            await DeleteFromTablesAsync(connectionString, new List<string>() { });
        }

        private static async Task DeleteFromTablesAsync(string connectionString, List<string> except)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var tableNames = await GetTableNamesAsync(conn, except);

            try
            {
                {
                    // disable constraints
                    var cmdText = @"ALTER SECURITY POLICY rls.SecurityPolicy WITH(STATE = OFF);
                                   EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT all'";
                    using var cmd = new SqlCommand(cmdText, conn);
                    await cmd.ExecuteNonQueryAsync();
                }

                {
                    var cmdText = new StringBuilder();

                    foreach (var tableName in tableNames)
                    {
                        cmdText.AppendLine($"DELETE FROM {tableName};");
                    }

                    using var cmd = new SqlCommand(cmdText.ToString(), conn);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                // enable constraints
                var cmdText = @"ALTER SECURITY POLICY rls.SecurityPolicy WITH(STATE = ON);
                               EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? CHECK CONSTRAINT all'";
                using var cmd = new SqlCommand(cmdText, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private static async Task<List<string>> GetTableNamesAsync(SqlConnection conn, List<string> except)
        {
            var tableNames = new List<string>();

            var cmdText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " +
                      "WHERE TABLE_TYPE = 'BASE TABLE'";

            using (var cmd = new SqlCommand(cmdText, conn))
            {
                using var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }

            except = except.Concat(new List<string> { "__EFMigrationsHistory" }).ToList();
            return tableNames.Except(except).ToList();
        }
    }
}
