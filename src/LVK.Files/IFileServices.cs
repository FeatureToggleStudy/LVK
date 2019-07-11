using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Files
{
    public interface IFileServices
    {
        bool Delete([NotNull] string filePath);

        [NotNull]
        Task CopyFileAsync([NotNull] string sourceFilePath, [NotNull] string targetFilePath, CancellationToken cancellationToken);

        [NotNull]
        Task MoveFileAsync([NotNull] string sourceFilePath, [NotNull] string targetFilePath, CancellationToken cancellationToken);

        [NotNull]
        Task<bool> CompareAsync([NotNull] string filePath1, [NotNull] string filePath2, CancellationToken cancellationToken);

        [NotNull]
        string MakeUnique([NotNull] string filePath);
    }
}