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
    internal class FileCopier : IFileCopier, IProgress<string>
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IFileContentsCopier _FileContentsCopier;

        public FileCopier([NotNull] ILogger logger, [NotNull] IFileContentsCopier fileContentsCopier)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _FileContentsCopier = fileContentsCopier ?? throw new ArgumentNullException(nameof(fileContentsCopier));
        }

        public async Task CopyAsync(string sourceFilePath, string targetFilePath, CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Verbose, $"Copying '{sourceFilePath}' to '{targetFilePath}'"))
            {
                await _FileContentsCopier.CopyFileContentsAsync(sourceFilePath, targetFilePath, this, cancellationToken);

                File.SetAttributes(targetFilePath, File.GetAttributes(sourceFilePath));
                File.SetCreationTimeUtc(targetFilePath, File.GetCreationTimeUtc(sourceFilePath));
                File.SetLastWriteTimeUtc(targetFilePath, File.GetLastWriteTimeUtc(sourceFilePath));
                File.SetLastAccessTimeUtc(targetFilePath, File.GetLastAccessTimeUtc(sourceFilePath));
            }
        }

        public void Report(string value) => _Logger.LogVerbose($"Copied {value}");
    }
}