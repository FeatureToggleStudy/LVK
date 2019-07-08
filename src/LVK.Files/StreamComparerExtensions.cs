using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Files
{
    [PublicAPI]
    public static class StreamComparerExtensions
    {
        public static async Task<bool> CompareAsync([NotNull] this IStreamComparer streamComparer, [NotNull] string filePath1, [NotNull] string filePath2, CancellationToken cancellationToken)
        {
            if (streamComparer == null)
                throw new ArgumentNullException(nameof(streamComparer));

            if (filePath1 == null)
                throw new ArgumentNullException(nameof(filePath1));

            if (filePath2 == null)
                throw new ArgumentNullException(nameof(filePath2));

            filePath1 = Path.GetFullPath(filePath1);
            filePath2 = Path.GetFullPath(filePath2);

            using (var stream1 = File.OpenRead(filePath1))
            using (var stream2 = File.OpenRead(filePath2))
            {
                if (filePath1 == filePath2)
                    return true;

                return await streamComparer.CompareAsync(stream1, stream2, cancellationToken);
            }
        }
    }
}