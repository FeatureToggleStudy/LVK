using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;
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
        private readonly List<IBackgroundService> _BackgroundServices;

        [NotNull, ItemNotNull]
        private readonly List<Task> _Tasks = new List<Task>();

        public BackgroundServicesManager([NotNull, ItemNotNull] IEnumerable<IBackgroundService> backgroundServices,
                                         [NotNull] IApplicationLifetimeManager applicationLifetimeManager,
                                         [NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper)
        {
            if (backgroundServices == null)
                throw new ArgumentNullException(nameof(backgroundServices));

            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));

            _BackgroundServices = backgroundServices.ToList();
        }

        public void StartBackgroundServices()
        {
            foreach (IBackgroundService backgroundService in _BackgroundServices)
                _Tasks.Add(RunBackgroundService(backgroundService));
        }

        public async Task WaitForBackgroundServicesToStop()
        {
            foreach (var task in _Tasks)
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

        private async Task RunBackgroundService([NotNull] IBackgroundService backgroundService)
        {
            await Task.Yield();
            try
            {
                await backgroundService.Execute(_ApplicationLifetimeManager.GracefulTerminationCancellationToken);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                _Logger.LogError(
                    $"background service '{_TypeHelper.NameOf(backgroundService.GetType())}' threw an exception, terminating program");

                _Logger.LogException(ex);

                _ApplicationLifetimeManager.SignalGracefulTermination();
            }
        }
    }
}