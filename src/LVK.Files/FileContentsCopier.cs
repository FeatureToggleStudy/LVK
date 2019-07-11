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
    internal class FileContentsCopier : IFileContentsCopier
    {
        [NotNull]
        private readonly ILogger _Logger;

        public FileContentsCopier([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CopyFileContentsAsync(string sourceFilePath, string targetFilePath, IProgress<string> progressReporter, CancellationToken cancellationToken)
        {
            const int bufferSize = 256 * 1024; // 256KB;
            try
            {
                using (var stream1 = File.OpenRead(sourceFilePath))
                using (var stream2 = File.Create(targetFilePath))
                {
                    var buffer1 = new byte[bufferSize];
                    var buffer2 = new byte[bufferSize];

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

                        double percent;
                        if (total == 0)
                            percent = 100;
                        else
                            percent = totalWritten * 100.0 / total;

                        progressReporter.Report($"{totalWritten.Bytes().ToString("0.#")} / {total.Bytes().ToString("0.#")} ({percent:0}%)");
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

                    nextReport = DateTime.Now;
                    report();

                    if (leftToWrite > 0)
                        throw new InvalidOperationException("Unable to complete operation");
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