using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore
{
    internal class PidQuitFileMonitorBackgroundService : PidFileBackgroundServiceBase, IBackgroundService
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        private bool _FirstRun = true;

        public PidQuitFileMonitorBackgroundService([NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                    if (_FirstRun)
                        _Logger.LogDebug($"looking for .quit file in '{filename}'");

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

            _FirstRun = false;
        }
    }
}