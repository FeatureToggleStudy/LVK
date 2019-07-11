using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Files
{
    [PublicAPI]
    public interface IFileMover
    {
        [NotNull]
        Task MoveAsync([NotNull] string filePath1, [NotNull] string filePath2, CancellationToken cancellationToken);
    }
}