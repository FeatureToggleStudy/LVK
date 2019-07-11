using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.Files
{
    internal class FileMover : IFileMover
    {
        [NotNull]
        private readonly IFileCopier _FileCopier;

        [NotNull]
        private readonly ILogger _Logger;

        public FileMover([NotNull] IFileCopier fileCopier, [NotNull] ILogger logger)
        {
            _FileCopier = fileCopier ?? throw new ArgumentNullException(nameof(fileCopier));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task MoveAsync(string sourceFilePath, string targetFilePath, CancellationToken cancellationToken)
        {
            if (sourceFilePath == null)
                throw new ArgumentNullException(nameof(sourceFilePath));

            if (targetFilePath == null)
                throw new ArgumentNullException(nameof(targetFilePath));

            if (TryFastMove(sourceFilePath, targetFilePath))
                return Task.CompletedTask;

            return SlowMove(sourceFilePath, targetFilePath, cancellationToken);
        }

        private async Task SlowMove([NotNull] string sourceFilePath, [NotNull] string targetFilePath, CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Verbose, $"Moving '{sourceFilePath}' to '{targetFilePath}'"))
            {
                try
                {
                    await _FileCopier.CopyAsync(sourceFilePath, targetFilePath, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    File.Delete(sourceFilePath);
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);
                    if (File.Exists(targetFilePath))
                        File.Delete(targetFilePath);

                    throw;
                }
            }
        }

        private bool TryFastMove([NotNull] string sourceFilePath, [NotNull] string targetFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath) || string.IsNullOrWhiteSpace(targetFilePath))
                return false;
            
            sourceFilePath = Path.GetFullPath(sourceFilePath);
            targetFilePath = Path.GetFullPath(targetFilePath);
            
            if (!Regex.IsMatch(sourceFilePath, @"^[A-Z]:\\"))
                return false;

            if (sourceFilePath[0] != targetFilePath[0])
                return false;

            using (_Logger.LogScope(LogLevel.Verbose, $"Moving '{sourceFilePath}' to '{targetFilePath}'"))
            {
                File.Move(sourceFilePath, targetFilePath);
                return true;
            }
        }
    }
}