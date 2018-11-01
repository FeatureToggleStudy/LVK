using System;
using System.Collections.Generic;
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
            if (PidFilePaths.Length == 0)
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

        [NotNull, ItemNotNull]
        private IEnumerable<string> QuitFilePaths => PidFilePaths.Select(filePath => Path.ChangeExtension(filePath, ".quit"));

        private bool AnyQuitFilesExists() => QuitFilePaths.Any() && QuitFilePaths.Any(File.Exists);

        private void DeleteQuitFiles()
        {
            foreach (var filePath in QuitFilePaths)
            {
                try
                {
                    if (_FirstRun)
                        _Logger.LogDebug($"looking for .quit file in '{filePath}'");

                    if (File.Exists(filePath))
                        File.Delete(filePath);
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