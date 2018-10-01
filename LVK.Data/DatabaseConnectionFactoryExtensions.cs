using System;
using System.Data;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public static class DatabaseConnectionFactoryExtensions
    {
        [NotNull]
        public static IDbConnection Create([NotNull] this IDatabaseConnectionFactory databaseConnectionFactory, [NotNull] string name)
            => databaseConnectionFactory.TryCreate(name) ?? throw new InvalidOperationException($"Unable to create database connection for '{name}'");
    }
}