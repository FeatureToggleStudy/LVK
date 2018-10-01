using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public class ResourceDatabaseMigrationsProvider : IDatabaseMigrationsProvider
    {
        public ResourceDatabaseMigrationsProvider(string resourcePath)
        {
        }

        public IEnumerable<IDatabaseMigration> Provide() => throw new NotImplementedException();
    }
}