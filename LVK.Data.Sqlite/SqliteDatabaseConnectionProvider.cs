using Microsoft.Data.Sqlite;

namespace LVK.Data.Sqlite
{
    internal class SqliteDatabaseConnectionProvider : IDatabaseConnectionProvider<SqliteConnection>
    {
        public SqliteConnection Create(string connectionString) => new SqliteConnection(connectionString);
    }
}