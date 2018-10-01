using System.Data;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseConnectionFactory
    {
        [CanBeNull]
        IDbConnection TryCreate([NotNull] string name);
    }
}