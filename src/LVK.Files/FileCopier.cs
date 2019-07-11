using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;

namespace LVK.Files
{
    internal class FileCopier : IFileCopier
    {
        [NotNull]
        private readonly ILogger _Logger;

        public FileCopier([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CopyAsync(string filePath1, string filePath2, CancellationToken cancellationToken)
        {
            const int bufferSize = 65536;

            using (_Logger.LogScope(LogLevel.Verbose, $"Copying '{filePath1}' to '{filePath2}'"))
            {
                await CopyFileContentsAsync(filePath1, filePath2, cancellationToken, bufferSize);

                File.SetAttributes(filePath2, File.GetAttributes(filePath1));
                File.SetCreationTimeUtc(filePath2, File.GetCreationTimeUtc(filePath1));
                File.SetLastWriteTimeUtc(filePath2, File.GetLastWriteTimeUtc(filePath1));
                File.SetLastAccessTimeUtc(filePath2, File.GetLastAccessTimeUtc(filePath1));
            }
        }

        [NotNull]
        private async Task CopyFileContentsAsync([NotNull] string filePath1, [NotNull] string filePath2, CancellationToken cancellationToken, int bufferSize)
        {
            try
            {
                using (var stream1 = File.OpenRead(filePath1))
                using (var stream2 = File.Create(filePath2))
                {
                    var buffer1 = new byte[65536];
                    var buffer2 = new byte[65536];

                    long leftToWrite = stream1.Length;
                    int inBuffer1 = await stream1.ReadAsync(buffer1, 0, bufferSize, cancellationToken).NotNull();
                    if (inBuffer1 == 0)
                    {
                        if (leftToWrite > 0)
                            throw new InvalidOperationException("Unable to complete file copy");

                        return;
                    }

                    long totalWritten = 0;
                    long total = leftToWrite;
                    while (leftToWrite > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var writeTask = stream2.WriteAsync(buffer1, 0, inBuffer1, cancellationToken).NotNull();
                        int inBuffer2 = await stream1.ReadAsync(buffer2, 0, bufferSize, cancellationToken).NotNull();
                        await writeTask;

                        _Logger.LogDebug($"Copied {totalWritten} / {total}");

                        leftToWrite -= inBuffer1;

                        inBuffer1 = inBuffer2;
                        (buffer1, buffer2) = (buffer2, buffer1);
                        if (inBuffer1 == 0)
                            break;
                    }

                    if (leftToWrite > 0)
                        throw new InvalidOperationException("Unable to complete file copy");
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath2))
                    File.Delete(filePath2);

                throw;
            }
        }
    }
}