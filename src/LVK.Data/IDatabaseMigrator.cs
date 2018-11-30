using System.Data;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Data
{
    [PublicAPI]
    public interface IDatabaseMigrator
    {
        [NotNull]
        Task Migrate([NotNull] IDbConnection connection, [NotNull] string databaseName);
    }
}