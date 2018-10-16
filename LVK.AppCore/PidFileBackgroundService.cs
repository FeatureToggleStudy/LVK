using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

namespace LVK.AppCore
{
    internal class PidFileBackgroundService : PidFileBackgroundServiceBase, IBackgroundService
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            using (Stream stream = CreatePidStream())
            {
                if (stream != null)
                {
                    byte[] bytes = Encoding.Default.GetBytes(Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));
                    stream.Write(bytes, 0, bytes.Length);

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).NotNull();
                    }
                }
            }
        }

        [CanBeNull]
        private Stream CreatePidStream()
        {
            foreach (var filename in PidFilenames)
            {
                try
                {
                    var folderPath = Path.GetDirectoryName(filename).NotNull();
                    Directory.CreateDirectory(folderPath);
                    return new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 256, FileOptions.DeleteOnClose);
                }
                catch (DirectoryNotFoundException)
                {
                    // Ignore
                }
                catch (FileNotFoundException)
                {
                    // Ignore
                }
                catch (IOException)
                {
                    // Ignore
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignore
                }
            }

            return null;
        }
    }
}