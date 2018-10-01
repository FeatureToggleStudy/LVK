using System.Data;

using Microsoft.Data.Sqlite;

namespace LVK.Data.Sqlite
{
    internal class SqliteDatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        public string Type => "sqlite";
        
        public IDbConnection Create(string connectionString) => new SqliteConnection(connectionString);
    }
}