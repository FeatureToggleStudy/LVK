using System.Data;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseConnectionProvider<out T>
        where T: class, IDbConnection
    {
        [NotNull]
        T Create([NotNull] string connectionString);
    }
}