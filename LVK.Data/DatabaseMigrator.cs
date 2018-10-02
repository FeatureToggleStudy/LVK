using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Data
{
    internal class DatabaseMigrator : IDatabaseMigrator
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull, ItemNotNull]
        private readonly List<IDatabaseMigration> _Migrations;

        public DatabaseMigrator([NotNull] IContainer container, [NotNull] ILogger logger, [NotNull, ItemNotNull] IEnumerable<IDatabaseMigration> migrations, [NotNull, ItemNotNull] IEnumerable<IDatabaseMigrationsProvider> migrationsProviders)
        {
            if (migrations is null)
                throw new ArgumentNullException(nameof(migrations));

            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _Migrations = migrations.ToList();
            foreach (var provider in migrationsProviders)
                _Migrations.AddRange(provider.Provide());
        }

        public async Task Migrate(IDbConnection connection, string databaseName)
        {
            if (connection is null)
                throw new ArgumentNullException(nameof(connection));

            if (databaseName is null)
                throw new ArgumentNullException(nameof(databaseName));

            var versionHandler = GetVersionHandler(connection.GetType());
            using (connection.OpenScope())
            {
                int version;

                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    version = await versionHandler.GetCurrentVersion(connection, transaction);
                    _Logger.Log(LogLevel.Verbose, $"database is version {version}");
                    
                    transaction.Commit();
                }

                var upgradeMigrations = WorkoutMigrations(databaseName, version);
                foreach (var migration in upgradeMigrations)
                {
                    using (_Logger.LogScope(LogLevel.Information, $"migrating database from version {migration.From} to {migration.To}"))
                    {
                        await migration.PerformMigration(connection);

                        using (IDbTransaction transaction = connection.BeginTransaction())
                        {
                            await migration.PerformMigration(connection, transaction);
                            await versionHandler.SetCurrentVersion(connection, transaction, migration.To);
                            transaction.Commit();
                        }
                    }
                }
            }
        }

        private List<IDatabaseMigration> WorkoutMigrations(string databaseName, int currentVersion)
        {
            var migrationsBySourceVersion = (
                from migration in _Migrations
                where StringComparer.InvariantCultureIgnoreCase.Equals(databaseName, migration.DatabaseName)
                select migration).ToLookup(migration => migration.From);

            var result = new List<IDatabaseMigration>();

            while (true)
            {
                var nextMigration = migrationsBySourceVersion[currentVersion]
                   .Where(migration => migration.To > migration.From)
                   .OrderByDescending(migration => migration.To)
                   .FirstOrDefault();

                if (nextMigration == null)
                    break;

                result.Add(nextMigration);
                currentVersion = nextMigration.To;
            }

            return result;
        }

        [NotNull]
        private IDatabaseVersionHandler GetVersionHandler(Type connectionType)
        {
            var handlerType = typeof(IDatabaseVersionHandler<>).MakeGenericType(connectionType);
            var handler = _Container.Resolve(handlerType, IfUnresolved.ReturnDefaultIfNotRegistered) as IDatabaseVersionHandler;
            if (handler == null)
                throw new InvalidOperationException("Unable to migrate database of type '{connection.GetType()}', no version handler registered");

            return handler;
        }
    }
}