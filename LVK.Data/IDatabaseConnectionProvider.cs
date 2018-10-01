using System.Data;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseConnectionProvider
    {
        [NotNull]
        string Type { get; }

        [NotNull]
        IDbConnection Create([NotNull] string connectionString);
    }
}