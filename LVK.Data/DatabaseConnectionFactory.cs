using System;
using System.Collections.Generic;
using System.Data;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;
using LVK.Logging;

namespace LVK.Data
{
    internal class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IDatabaseMigrator _DatabaseMigrator;

        [NotNull]
        private readonly Dictionary<string, IDatabaseConnectionProvider> _ProvidersByType =
            new Dictionary<string, IDatabaseConnectionProvider>(StringComparer.InvariantCultureIgnoreCase);

        public DatabaseConnectionFactory([NotNull, ItemNotNull] IEnumerable<IDatabaseConnectionProvider> providers, [NotNull] IConfiguration configuration, [NotNull] ILogger logger, [NotNull] IDatabaseMigrator databaseMigrator)
        {
            if (providers is null)
                throw new ArgumentNullException(nameof(providers));

            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseMigrator = databaseMigrator ?? throw new ArgumentNullException(nameof(databaseMigrator));

            foreach (var provider in providers)
                _ProvidersByType[provider.Type] = provider;
        }

        public IDbConnection TryCreate(string name, bool autoMigrate)
        {
            using (_Logger.LogScope(LogLevel.Trace, nameof(DatabaseConnectionFactory) + "." + nameof(TryCreate)))
            {
                var configuration = ReadConnectionStringFromConfiguration(name);
                if (configuration == null)
                {
                    _Logger.LogDebug($"no connection string for database connection '{name}'");
                    return null;
                }

                var (success, type, connectionString) = SplitConnectionString(configuration);
                if (!success)
                {
                    _Logger.LogError($"invalid connection string for database connection '{name}'");
                    return null;
                }

                var provider = _ProvidersByType.GetValueOrDefault(type);
                if (provider == null)
                {
                    _Logger.LogError($"no database provider for type '{type}' for database connection '{name}' has been registered");
                    return null;
                }
                
                var connection = provider.Create(connectionString);
                if (autoMigrate)
                    _DatabaseMigrator.Migrate(connection, name);

                return connection;
            }
        }

        private string ReadConnectionStringFromConfiguration(string name)
            => _Configuration.Element<string>($"Data/ConnectionStrings/{name}")
               .Value();

        private (bool success, string type, string connectionString) SplitConnectionString(string configuration)
        {
            int index = configuration.IndexOf(';');
            if (index <= 0)
                return (false, null, null);

            string type = configuration.Substring(0, index);
            string connectionString = configuration.Substring(index + 1);

            return (true, type, connectionString);
        }
    }
}