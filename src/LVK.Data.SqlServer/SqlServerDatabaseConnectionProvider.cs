using System.Data.SqlClient;

namespace LVK.Data.SqlServer
{
    internal class SqlServerDatabaseConnectionProvider : IDatabaseConnectionProvider<SqlConnection>
    {
        public SqlConnection Create(string connectionString) => new SqlConnection(connectionString);
    }
}