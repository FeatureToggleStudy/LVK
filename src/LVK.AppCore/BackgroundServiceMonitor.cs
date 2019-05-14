using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Features;
using LVK.Logging;
using LVK.Reflection;

namespace LVK.AppCore
{
    internal class BackgroundServiceMonitor
    {
        [NotNull]
        private readonly IFeatureToggleWithDefaultValue _FeatureToggle;

        [NotNull]
        private readonly IBackgroundService _BackgroundService;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        public BackgroundServiceMonitor(
            [NotNull] IFeatureToggleWithDefaultValue featureToggle, [NotNull] IBackgroundService backgroundService,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper)
        {
            _FeatureToggle = featureToggle ?? throw new ArgumentNullException(nameof(featureToggle));
            _BackgroundService = backgroundService ?? throw new ArgumentNullException(nameof(backgroundService));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
        }

        [NotNull]
        public async Task RunAsync()
        {
            Task backgroundService = null;
            CancellationTokenSource cts = null;
            bool isFirstTime = true;
            try
            {
                while (!_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
                {
                    bool featureToggleValue = _FeatureToggle.IsEnabled;
                    if (featureToggleValue && (backgroundService is null))
                    {
                        if (!isFirstTime)
                            _Logger.LogDebug(
                                $"Starting background service {_TypeHelper.NameOf(_BackgroundService.GetType())} due to configuration changes");

                        cts = new CancellationTokenSource();
                        backgroundService = RunBackgroundService(cts.Token);

                        isFirstTime = false;
                    }
                    else if (!featureToggleValue && !(backgroundService is null))
                    {
                        cts.Cancel();
                        await backgroundService;

                        backgroundService = null;
                        cts.Dispose();
                        cts = null;

                        _Logger.LogDebug(
                            $"Stopped background service {_TypeHelper.NameOf(_BackgroundService.GetType())} due to configuration changes");
                    }

                    // toggle status
                    await Task.Delay(10000, _ApplicationLifetimeManager.GracefulTerminationCancellationToken);
                }

                if (backgroundService != null)
                {
                    cts.Cancel();
                    await backgroundService;

                    _Logger.LogDebug(
                        $"Starting background service {_TypeHelper.NameOf(_BackgroundService.GetType())} due to application termination");
                }
            }
            finally
            {
                cts?.Dispose();
            }
        }

        private async Task RunBackgroundService(CancellationToken cancellationToken)
        {
            await Task.Yield();
            try
            {
                await _BackgroundService.Execute(cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                _Logger.LogException(ex);
                _Logger.LogError(
                    $"background service '{_TypeHelper.NameOf(_BackgroundService.GetType())}' threw an exception, terminating program");

                // TODO: Still do this?
                _ApplicationLifetimeManager.SignalGracefulTermination();
            }
        }
    }
}