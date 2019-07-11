using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Files
{
    [PublicAPI]
    public interface IFileMover
    {
        [NotNull]
        Task MoveAsync([NotNull] string sourceFilePath, [NotNull] string targetFilePath, CancellationToken cancellationToken);
    }
}