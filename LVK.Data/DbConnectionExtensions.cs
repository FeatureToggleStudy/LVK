using System;
using System.Data;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Data
{
    [PublicAPI]
    public static class DbConnectionExtensions
    {
        [NotNull]
        public static IDbConnection AsOpen([NotNull] this IDbConnection connection)
        {
            if (connection is null)
                throw new ArgumentNullException(nameof(connection));

            connection.Open();
            return connection;
        }

        [NotNull]
        public static IDisposable OpenScope([NotNull] this IDbConnection connection)
        {
            if (connection is null)
                throw new ArgumentNullException(nameof(connection));

            if (connection.State == ConnectionState.Open)
                return new VoidDisposable();

            return new ActionDisposable(connection.Open, connection.Close);
        }
    }
}