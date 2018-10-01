using System.Data;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseVersionHandler
    {
        [NotNull]
        Task<int> GetCurrentVersion([NotNull] IDbConnection connection, [NotNull] IDbTransaction transaction);

        [NotNull]
        Task SetCurrentVersion([NotNull] IDbConnection connection, [NotNull] IDbTransaction transaction, int version);
    }

    [PublicAPI]
    public interface IDatabaseVersionHandler<in T> : IDatabaseVersionHandler
        where T: class, IDbConnection
    {
    }
}