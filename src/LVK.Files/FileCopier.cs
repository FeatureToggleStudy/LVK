using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Humanizer;

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

        public async Task CopyAsync(string sourceFilePath, string targetFilePath, CancellationToken cancellationToken)
        {
            const int bufferSize = 65536;

            using (_Logger.LogScope(LogLevel.Verbose, $"Copying '{sourceFilePath}' to '{targetFilePath}'"))
            {
                await CopyFileContentsAsync(sourceFilePath, targetFilePath, cancellationToken, bufferSize);

                File.SetAttributes(targetFilePath, File.GetAttributes(sourceFilePath));
                File.SetCreationTimeUtc(targetFilePath, File.GetCreationTimeUtc(sourceFilePath));
                File.SetLastWriteTimeUtc(targetFilePath, File.GetLastWriteTimeUtc(sourceFilePath));
                File.SetLastAccessTimeUtc(targetFilePath, File.GetLastAccessTimeUtc(sourceFilePath));
            }
        }

        [NotNull]
        private async Task CopyFileContentsAsync([NotNull] string sourceFilePath, [NotNull] string targetFilePath, CancellationToken cancellationToken, int bufferSize)
        {
            try
            {
                using (var stream1 = File.OpenRead(sourceFilePath))
                using (var stream2 = File.Create(targetFilePath))
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
                    var nextReport = DateTime.Now;

                    void report()
                    {
                        if (DateTime.Now < nextReport)
                            return;

                        _Logger.LogDebug($"Copied {totalWritten.Bytes().ToString("0.0")} / {total.Bytes().ToString("0.0")}");
                        nextReport = DateTime.Now.AddSeconds(5);
                    }

                    report();

                    while (leftToWrite > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var writeTask = stream2.WriteAsync(buffer1, 0, inBuffer1, cancellationToken).NotNull();
                        int inBuffer2 = await stream1.ReadAsync(buffer2, 0, bufferSize, cancellationToken).NotNull();
                        await writeTask;
                        totalWritten += inBuffer1;

                        report();

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
            catch (Exception)
            {
                if (File.Exists(targetFilePath))
                    File.Delete(targetFilePath);

                throw;
            }
        }
    }
}