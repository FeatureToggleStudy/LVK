using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LVK.Files
{
    internal class FileContentsCopier : IFileContentsCopier
    {
        public async Task CopyFileContentsAsync(
            string sourceFilePath, string targetFilePath, IProgress<string> progressReporter, CancellationToken cancellationToken)
        {
            try
            {
                using (var stream1 = File.OpenRead(sourceFilePath))
                using (var stream2 = File.Create(targetFilePath))
                {
                    await new StreamContentsCopier(stream1, stream2, progressReporter).Copy(cancellationToken);
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