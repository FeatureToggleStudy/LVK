using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Features;
using LVK.Logging;
using LVK.Reflection;

namespace LVK.AppCore
{
    internal class BackgroundServicesManager : IBackgroundServicesManager
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        [NotNull]
        private readonly IFeatureToggles _FeatureToggles;

        [NotNull]
        private readonly List<IBackgroundService> _BackgroundServices;

        [NotNull, ItemNotNull]
        private readonly List<Task> _Tasks = new List<Task>();

        [NotNull]
        private readonly object _Lock = new object();

        public BackgroundServicesManager(
            [NotNull, ItemNotNull] IEnumerable<IBackgroundService> backgroundServices,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper,
            [NotNull] IFeatureToggles featureToggles)
        {
            if (backgroundServices == null)
                throw new ArgumentNullException(nameof(backgroundServices));

            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
            _FeatureToggles = featureToggles ?? throw new ArgumentNullException(nameof(featureToggles));

            _BackgroundServices = backgroundServices.ToList();
        }

        public void StartBackgroundServices()
        {
            using (_Logger.LogScope(LogLevel.Debug, "Starting background services"))
            {
                lock (_Lock)
                {
                    if (_Tasks.Any())
                        return;

                    foreach (IBackgroundService backgroundService in _BackgroundServices)
                    {
                        Type backgroundServiceType = backgroundService.GetType();
                        IFeatureToggleWithDefault featureToggle =
                            _FeatureToggles.GetByKey($"Services/{backgroundServiceType.FullName}").WithDefault(true);

                        var monitor = new BackgroundServiceMonitor(
                            featureToggle, backgroundService, _ApplicationLifetimeManager, _Logger, _TypeHelper);

                        _Tasks.Add(monitor.RunAsync());
                    }
                }
            }
        }

        public async Task WaitForBackgroundServicesToStop()
        {
            using (_Logger.LogScope(LogLevel.Debug, "Stopping background services"))
            {
                List<Task> tasks;
                lock (_Lock)
                {
                    tasks = _Tasks.ToList();
                    _Tasks.Clear();
                }

                using (_Logger.LogScope(LogLevel.Debug, "Waiting for background tasks to complete"))
                {
                    foreach (Task task in tasks)
                    {
                        try
                        {
                            await task;
                        }
                        catch (Exception)
                        {
                            // Logged as part of RunBackgroundService
                        }
                    }
                }
            }
        }
    }
}