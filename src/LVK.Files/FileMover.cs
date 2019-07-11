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

        public Task MoveAsync(string filePath1, string filePath2, CancellationToken cancellationToken)
        {
            if (filePath1 == null)
                throw new ArgumentNullException(nameof(filePath1));

            if (filePath2 == null)
                throw new ArgumentNullException(nameof(filePath2));

            if (TryFastMove(filePath1, filePath2))
                return Task.CompletedTask;

            return SlowMove(filePath1, filePath2, cancellationToken);
        }

        private async Task SlowMove([NotNull] string filePath1, [NotNull] string filePath2, CancellationToken cancellationToken)
        {
            using (_Logger.LogScope(LogLevel.Verbose, $"Moving '{filePath1}' to '{filePath2}'"))
            {
                try
                {
                    await _FileCopier.CopyAsync(filePath1, filePath2, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);
                    if (File.Exists(filePath2))
                        File.Delete(filePath2);

                    throw;
                }
            }
        }

        private bool TryFastMove([NotNull] string filePath1, [NotNull] string filePath2)
        {
            if (string.IsNullOrWhiteSpace(filePath1) || string.IsNullOrWhiteSpace(filePath2))
                return false;
            
            filePath1 = Path.GetFullPath(filePath1);
            filePath2 = Path.GetFullPath(filePath2);
            
            if (!Regex.IsMatch(filePath1, @"^[A-Z]:\\"))
                return false;

            if (filePath1[0] != filePath2[0])
                return false;

            using (_Logger.LogScope(LogLevel.Verbose, $"Moving '{filePath1}' to '{filePath2}'"))
            {
                File.Move(filePath1, filePath2);
                return true;
            }
        }
    }
}