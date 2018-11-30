using System.Data;
using System.Threading.Tasks;

using Dapper;

using LVK.Core;
using LVK.Data;

namespace LVK.WorkQueues.Sqlite.Migrations
{
    internal class Version0To1 : IDatabaseMigration
    {
        public string DatabaseName => "SqliteWorkqueue";

        public int From => 0;

        public int To => 1;

        public Task PerformMigration(IDbConnection connection) => connection.ExecuteAsync("PRAGMA journal_model=WAL").NotNull();

        public async Task PerformMigration(IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                @"CREATE TABLE IF NOT EXISTS queue
                (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    type TEXT NOT NULL,
                    payload TEXT NOT NULL,
                    hash TEXT NOT NULL,
                    when_to_process DATETIME NOT NULL,
                    retry_count INT NOT NULL
                )", transaction).NotNull();

            await connection.ExecuteAsync(@"CREATE INDEX idx_when_to_process ON queue(when_to_process)", transaction).NotNull();
            await connection.ExecuteAsync(@"CREATE UNIQUE INDEX idx_queue_hash ON queue(hash)", transaction).NotNull();

            await connection.ExecuteAsync(
                @"CREATE TABLE IF NOT EXISTS faulted
                (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    type TEXT NOT NULL,
                    payload TEXT NOT NULL,
                    hash TEXT NOT NULL
                )", transaction).NotNull();
            await connection.ExecuteAsync(@"CREATE UNIQUE INDEX idx_faulted_hash ON faulted(hash)", transaction).NotNull();
        }
    }
}