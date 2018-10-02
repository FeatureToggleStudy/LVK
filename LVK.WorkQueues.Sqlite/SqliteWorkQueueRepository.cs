using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using JetBrains.Annotations;

using LVK.Data;
using LVK.Security.Cryptography;

using Microsoft.Data.Sqlite;

namespace LVK.WorkQueues.Sqlite
{
    public class SqliteWorkQueueRepository : IWorkQueueRepository
    {
        private const int _SqliteConstraintViolationErrorCode = 19;

        [NotNull]
        private readonly IDatabaseConnectionFactory _DatabaseConnectionFactory;

        [NotNull]
        private readonly IDatabaseMigrator _DatabaseMigrator;

        [NotNull]
        private readonly IHasher _Hasher;

        [NotNull]
        private static readonly object _Lock = new object();

        public SqliteWorkQueueRepository([NotNull] IDatabaseConnectionFactory databaseConnectionFactory, [NotNull] IDatabaseMigrator databaseMigrator, [NotNull] IHasher hasher)
        {
            _DatabaseConnectionFactory = databaseConnectionFactory ?? throw new ArgumentNullException(nameof(databaseConnectionFactory));
            _DatabaseMigrator = databaseMigrator ?? throw new ArgumentNullException(nameof(databaseMigrator));
            _Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        [CanBeNull]
        private SqliteConnection TryCreateConnection() => _DatabaseConnectionFactory.TryCreate<SqliteConnection>("sqlite_workqueue", false);

        public async Task EnqueueManyAsync(IEnumerable<WorkQueueItem> items)
        {
            using (var databaseConnection = TryCreateConnection())
            {
                if (databaseConnection == null)
                    return;

                databaseConnection.Open();
                await _DatabaseMigrator.Migrate(databaseConnection, "SqliteWorkqueue");

                using (var transaction = databaseConnection.BeginTransaction())
                {
                    foreach (var item in items)
                    {
                        var json = item.Payload.ToString();
                        var hash = _Hasher.Hash(json);
                        try
                        {
                            await databaseConnection.ExecuteAsync(
                                "INSERT INTO queue (type, payload, hash, when_to_process, retry_count) VALUES (@Type, @Payload, @Hash, @WhenToProcess, @RetryCount)",
                                new
                                {
                                    item.Type,
                                    Payload = json,
                                    Hash = hash,
                                    item.WhenToProcess,
                                    item.RetryCount
                                }, transaction);
                        }
                        catch (SqliteException ex) when (ex.SqliteErrorCode == _SqliteConstraintViolationErrorCode)
                        {
                            // ignored
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public async Task FaultedAsync(WorkQueueItem item)
        {
            using (var databaseConnection = TryCreateConnection())
            {
                if (databaseConnection == null)
                    return;

                databaseConnection.Open();
                await _DatabaseMigrator.Migrate(databaseConnection, "SqliteWorkqueue");

                using (var transaction = databaseConnection.BeginTransaction())
                {
                    var json = item.Payload.ToString();
                    var hash = _Hasher.Hash(json);
                    try
                    {
                        await databaseConnection.ExecuteAsync(
                            "INSERT INTO faulted (type, payload, hash) VALUES (@Type, @Payload, @Hash)", new { item.Type, Payload = json, Hash = hash },
                            transaction);
                    }
                    catch (SqliteException ex) when (ex.SqliteErrorCode == _SqliteConstraintViolationErrorCode)
                    {
                        // ignored
                    }

                    transaction.Commit();
                }
            }
        }

        public async Task<WorkQueueItem?> DequeueAsync()
        {
            using (var databaseConnection = TryCreateConnection())
            {
                if (databaseConnection == null)
                    return null;

                databaseConnection.Open();
                await _DatabaseMigrator.Migrate(databaseConnection, "SqliteWorkqueue");

                using (var transaction = databaseConnection.BeginTransaction())
                {
                    var first = (await databaseConnection.QueryAsync<WorkQueueEntity>("SELECT * FROM queue WHERE when_to_process <= @cutoff ORDER BY when_to_process LIMIT 1", new { cutoff = DateTime.Now }, transaction)).FirstOrDefault();

                    if (first == null)
                        return null;

                    await databaseConnection.ExecuteAsync("DELETE FROM queue WHERE id = @Id", first);
                    transaction.Commit();

                    return first.ToItem();
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                var connection = TryCreateConnection();
                bool result = connection != null;
                connection?.Dispose();
                return result;
            }
        }
    }
}