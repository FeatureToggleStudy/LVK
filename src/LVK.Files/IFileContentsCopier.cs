using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Files
{
    internal interface IFileContentsCopier
    {
        [NotNull]
        Task CopyFileContentsAsync([NotNull] string sourceFilePath, [NotNull] string targetFilePath, [NotNull] IProgress<string> progressReporter, CancellationToken cancellationToken);
    }
}