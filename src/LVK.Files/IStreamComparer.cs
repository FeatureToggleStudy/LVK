using System.IO;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Files
{
    [PublicAPI]
    public interface IStreamComparer
    {
        [NotNull]
        Task<bool> CompareAsync([NotNull] Stream stream1, [NotNull] Stream stream2, CancellationToken cancellationToken);
    }
}