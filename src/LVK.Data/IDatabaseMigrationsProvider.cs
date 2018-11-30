using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseMigrationsProvider
    {
        [NotNull, ItemNotNull]
        IEnumerable<IDatabaseMigration> Provide();
    }
}