using System;
using System.Data;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public static class DatabaseConnectionFactoryExtensions
    {
        [NotNull]
        public static T Create<T>([NotNull] this IDatabaseConnectionFactory databaseConnectionFactory, [NotNull] string name, bool autoMigrate = true)
            where T: class, IDbConnection
            => databaseConnectionFactory.TryCreate<T>(name, autoMigrate)
            ?? throw new InvalidOperationException($"Unable to create database connection for '{name}'");
    }
}