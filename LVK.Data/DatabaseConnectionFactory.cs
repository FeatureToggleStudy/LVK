using System;
using System.Data;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Logging;

namespace LVK.Data
{
    internal class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IDatabaseMigrator _DatabaseMigrator;

        public DatabaseConnectionFactory(
            [NotNull] IContainer container, [NotNull] IConfiguration configuration, [NotNull] ILogger logger, [NotNull] IDatabaseMigrator databaseMigrator)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseMigrator = databaseMigrator ?? throw new ArgumentNullException(nameof(databaseMigrator));
        }

        public T TryCreate<T>(string name, bool autoMigrate)
            where T: class, IDbConnection
        {
            using (_Logger.LogScope(LogLevel.Trace, nameof(DatabaseConnectionFactory) + "." + nameof(TryCreate)))
            {
                var connectionString = _Configuration.Element<string>($"Data/ConnectionStrings/{name}")
                   .Value();

                if (connectionString == null)
                {
                    _Logger.LogDebug($"no connection string for database connection '{name}'");
                    return null;
                }

                var provider = _Container.Resolve<IDatabaseConnectionProvider<T>>(IfUnresolved.ReturnDefaultIfNotRegistered);
                if (provider == null)
                {
                    _Logger.LogError($"no database provider for type '{typeof(T)}' has been registered");
                    return null;
                }

                var connection = provider.Create(connectionString);
                if (autoMigrate)
                    _DatabaseMigrator.Migrate(connection, name);

                return connection;
            }
        }
    }
}