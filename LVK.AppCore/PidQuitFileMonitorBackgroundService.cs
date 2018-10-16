using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

namespace LVK.AppCore
{
    internal class PidQuitFileMonitorBackgroundService : PidFileBackgroundServiceBase, IBackgroundService
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public PidQuitFileMonitorBackgroundService([NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            if (PidFilenames.Length == 0)
                return;

            DeleteQuitFiles();

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken).NotNull();
                if (AnyQuitFilesExists())
                {
                    _ApplicationLifetimeManager.SignalGracefulTermination();
                    DeleteQuitFiles();
                }
            }
        }

        private bool AnyQuitFilesExists() => PidFilenames.Length > 0 && PidFilenames.Any(File.Exists);

        private void DeleteQuitFiles()
        {
            foreach (var filename in PidFilenames)
            {
                try
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignore
                }
                catch (IOException)
                {
                    // Ignore
                }
            }
        }
    }
}