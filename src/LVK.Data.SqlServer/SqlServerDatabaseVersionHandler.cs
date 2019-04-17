using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Dapper;

using LVK.Core;

namespace LVK.Data.SqlServer
{
    internal class SqlServerDatabaseVersionHandler : IDatabaseVersionHandler<SqlConnection>
    {
        public async Task<int> GetCurrentVersion(IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync("IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__version') BEGIN CREATE TABLE __version (VERSION INT NOT NULL) END", transaction).NotNull();

            return await connection.QueryFirstOrDefaultAsync<int>("SELECT VERSION FROM __version").NotNull();
        }

        public async Task SetCurrentVersion(IDbConnection connection, IDbTransaction transaction, int version)
        {
            int rowsAffected = await connection.ExecuteAsync("UPDATE __version SET VERSION = @version", new { version }, transaction).NotNull();
            if (rowsAffected == 0)
                await connection.ExecuteAsync("INSERT INTO __version (VERSION) VALUES (@version)", new { version }, transaction).NotNull();
        }
    }
}
