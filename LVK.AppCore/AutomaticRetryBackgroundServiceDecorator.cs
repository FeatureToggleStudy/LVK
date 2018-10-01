using System;
using System.Threading;
using System.Threading.Tasks;

using Humanizer;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;
using LVK.Reflection;

namespace LVK.AppCore
{
    [PublicAPI]
    public class AutomaticRetryBackgroundServiceDecorator : IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        [NotNull]
        private readonly IBackgroundService _DecoratedService;

        public AutomaticRetryBackgroundServiceDecorator(
            [NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper, [NotNull] IBackgroundService decoratedService)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
            _DecoratedService = decoratedService ?? throw new ArgumentNullException(nameof(decoratedService));
        }

        [NotNull]
        public async Task Execute(CancellationToken cancellationToken)
        {
            int restartCount = 0;
            while (true)
            {
                try
                {
                    if (restartCount > 0)
                        _Logger.LogDebug($"restart-attempt #{restartCount} for background service {_TypeHelper.NameOf(_DecoratedService.GetType())}");

                    await _DecoratedService.Execute(cancellationToken);
                    if (restartCount > 0)
                        _Logger.LogDebug($"background service {_TypeHelper.NameOf(_DecoratedService.GetType())} terminated normally");

                    return;
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _Logger.LogException(ex);

                    restartCount++;

                    TimeSpan restartDelay = GetRestartDelay(restartCount);
                    _Logger.LogVerbose(
                        $"background service {_TypeHelper.NameOf(_DecoratedService.GetType())} terminated due to an exception, restarting it in {restartDelay.Humanize()}");

                    await Task.Delay(restartDelay, cancellationToken).NotNull();
                }
            }
        }

        private TimeSpan GetRestartDelay(int restartCount)
        {
            switch (restartCount)
            {
                case int i when i < 3:
                    return TimeSpan.FromSeconds(1);

                case int i when i < 5:
                    return TimeSpan.FromSeconds(5);

                case int i when i < 10:
                    return TimeSpan.FromSeconds(30);

                case int i when i < 15:
                    return TimeSpan.FromMinutes(1);

                case int i when i >= 50:
                    throw new InvalidOperationException(
                        $"Background service {_TypeHelper.NameOf(_DecoratedService.GetType())} has terminated with unhandled exception too many times, giving up");

                default:
                    return TimeSpan.FromMinutes(5);
            }
        }
    }
}