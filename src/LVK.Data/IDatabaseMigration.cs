using System.Data;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseMigration
    {
        [NotNull]
        string DatabaseName { get; }

        int From { get; }

        int To { get; }

        [NotNull]
        Task PerformMigration([NotNull] IDbConnection connection);

        [NotNull]
        Task PerformMigration([NotNull] IDbConnection connection, [NotNull] IDbTransaction transaction);
    }
}