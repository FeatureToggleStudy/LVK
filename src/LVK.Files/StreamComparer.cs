using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Files
{
    internal class StreamComparer : IStreamComparer
    {
        public async Task<bool> CompareAsync(Stream stream1, Stream stream2, CancellationToken cancellationToken)
        {
            if (stream1 == null)
                throw new ArgumentNullException(nameof(stream1));

            if (stream2 == null)
                throw new ArgumentNullException(nameof(stream2));

            if (ReferenceEquals(stream1, stream2))
                return true;

            if (stream1.Length != stream2.Length)
                return false;

            return await CompareStreamContentsAsync(stream1, stream2, cancellationToken);
        }

        private static async Task<bool> CompareStreamContentsAsync([NotNull] Stream stream1, [NotNull] Stream stream2,
                                                             CancellationToken cancellationToken)
        {
            const int bufferSize = 65536;

            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var readTask1 = stream1.ReadAsync(buffer1, 0, bufferSize, cancellationToken).NotNull();
                var inBuffer2 = await stream2.ReadAsync(buffer2, 0, bufferSize, cancellationToken).NotNull();
                var inBuffer1 = await readTask1;

                if (inBuffer1 != inBuffer2)
                    return false;

                for (int index = 0; index < inBuffer1; index++)
                    if (buffer1[index] != buffer2[index])
                        return false;
            }
        }
    }
}