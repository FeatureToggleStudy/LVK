using System.Data;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseConnectionFactory
    {
        [CanBeNull]
        T TryCreate<T>([NotNull] string name, bool autoMigrate = true)
            where T: class, IDbConnection;
    }
}