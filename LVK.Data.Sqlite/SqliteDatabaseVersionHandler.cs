using System.Data;
using System.Threading.Tasks;

using Dapper;

using Microsoft.Data.Sqlite;

namespace LVK.Data.Sqlite
{
    internal class SqliteDatabaseVersionHandler : IDatabaseVersionHandler<SqliteConnection>
    {
        public async Task<int> GetCurrentVersion(IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync("CREATE TABLE IF NOT EXISTS __version (VERSION INTEGER NOT NULL)", transaction);

            return await connection.QueryFirstOrDefaultAsync<int>("SELECT VERSION FROM __version");
        }

        public async Task SetCurrentVersion(IDbConnection connection, IDbTransaction transaction, int version)
        {
            int rowsAffected = await connection.ExecuteAsync("UPDATE __version SET VERSION = @version", new { version }, transaction);
            if (rowsAffected == 0)
                await connection.ExecuteAsync("INSERT INTO __version (VERSION) VALUES (@version)", new { version }, transaction);
        }
    }
}