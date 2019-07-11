using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Logging;
using LVK.Reflection;

namespace LVK.Files
{
    internal class FileServices : IFileServices
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IFileCopier _FileCopier;

        [NotNull]
        private readonly IFileMover _FileMover;

        [NotNull]
        private readonly IStreamComparer _StreamComparer;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        public FileServices(
            [NotNull] ILogger logger, [NotNull] IFileCopier fileCopier, [NotNull] IFileMover fileMover,
            [NotNull] IStreamComparer streamComparer, [NotNull] ITypeHelper typeHelper)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _FileCopier = fileCopier ?? throw new ArgumentNullException(nameof(fileCopier));
            _FileMover = fileMover ?? throw new ArgumentNullException(nameof(fileMover));
            _StreamComparer = streamComparer ?? throw new ArgumentNullException(nameof(streamComparer));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
        }

        public bool Delete(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _Logger.LogDebug($"Did not delete '{filePath}', file does not exist");
                return false;
            }

            try
            {
                File.Delete(filePath);
                _Logger.LogDebug($"Deleted '{filePath}'");
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                _Logger.LogException(ex);
                _Logger.LogDebug($"Did not delete '{filePath}', {_TypeHelper.NameOf(ex.GetType() ?? typeof(UnauthorizedAccessException))}: {ex.Message}");

                return false;
            }
            catch (IOException ex)
            {
                _Logger.LogException(ex);
                _Logger.LogDebug($"Did not delete '{filePath}', {_TypeHelper.NameOf(ex.GetType() ?? typeof(IOException))}: {ex.Message}");
                return false;
            }
        }

        public Task CopyFileAsync(string filePath1, string filePath2, CancellationToken cancellationToken)
            => _FileCopier.CopyAsync(filePath1, filePath2, cancellationToken);

        public Task MoveFileAsync(string filePath1, string filePath2, CancellationToken cancellationToken)
            => _FileMover.MoveAsync(filePath1, filePath2, cancellationToken);

        public Task<bool> CompareAsync(string filePath1, string filePath2, CancellationToken cancellationToken)
            => _StreamComparer.CompareAsync(filePath1, filePath2, cancellationToken);

        public string MakeUnique(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            if (!File.Exists(filePath))
                return filePath;

            string folderPath = Path.GetDirectoryName(filePath).NotNull();
            string filename = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetFileNameWithoutExtension(filePath);

            int counter = 1;
            while (true)
            {
                filePath = Path.Combine(folderPath, $"{filename} ({counter}){extension}");
                if (!File.Exists(filePath))
                    return filePath;

                counter++;
            }
        }
    }
}