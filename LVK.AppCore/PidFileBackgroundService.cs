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
using LVK.Logging;

namespace LVK.AppCore
{
    internal class PidFileBackgroundService : PidFileBackgroundServiceBase, IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        public PidFileBackgroundService([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task Execute(CancellationToken cancellationToken)
        {
            using (Stream stream = CreatePidStream())
            {
                if (stream != null)
                {
                    byte[] bytes = Encoding.Default.GetBytes(Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));
                    stream.Write(bytes, 0, bytes.Length);

                    await cancellationToken.AsTask();

                    _Logger.LogVerbose($"application terminating, deleting .pid file");
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
                    var stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 256, FileOptions.DeleteOnClose);
                    _Logger.LogVerbose($"created .pid file in '{filename}'");
                    return stream;
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