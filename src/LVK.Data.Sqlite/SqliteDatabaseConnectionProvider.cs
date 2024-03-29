using System;

using JetBrains.Annotations;

using Microsoft.Data.Sqlite;

namespace LVK.Data.Sqlite
{
    internal class SqliteDatabaseConnectionProvider : IDatabaseConnectionProvider<SqliteConnection>
    {
        [NotNull]
        private readonly ISQLitePCLInitializer _SqLitePclInitializer;

        public SqliteDatabaseConnectionProvider([NotNull] ISQLitePCLInitializer sqLitePclInitializer)
        {
            _SqLitePclInitializer = sqLitePclInitializer ?? throw new ArgumentNullException(nameof(sqLitePclInitializer));
        }

        public SqliteConnection Create(string connectionString)
        {
            _SqLitePclInitializer.InitializeOnce();
            return new SqliteConnection(connectionString);
        }
    }
}