using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core.Services;

namespace LVK.AppCore
{
    [PublicAPI]
    public abstract class BasicBackgroundService : IApplicationRuntimeContext
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [CanBeNull]
        private Task _BackgroundTask;

        private readonly CancellationTokenSource _ManualCancellationTokenSource = new CancellationTokenSource();

        private readonly CancellationTokenSource _CombinedCancellationTokenSource;

        protected BasicBackgroundService([NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _CombinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                _ApplicationLifetimeManager.GracefulTerminationCancellationToken, _ManualCancellationTokenSource.Token);
        }

        Task IApplicationRuntimeContext.Start(CancellationToken cancellationToken)
        {
            Starting();
            _BackgroundTask = BackgroundTask(_CombinedCancellationTokenSource.Token);
            Started();
            return Task.CompletedTask;
        }

        protected void SignalGracefulTermination()
        {
            _ApplicationLifetimeManager.SignalGracefulTermination();
        }

        protected virtual void Starting()
        {
        }

        protected virtual void Started()
        {
        }

        protected abstract Task BackgroundTask(CancellationToken cancellationToken);

        protected virtual void Stopping()
        {
        }

        protected virtual void Stopped()
        {
        }

        async Task IApplicationRuntimeContext.Stop(CancellationToken cancellationToken)
        {
            _ManualCancellationTokenSource.Cancel();
            if (_BackgroundTask != null)
            {
                Stopping();
                try
                {
                    await _BackgroundTask;
                }
                catch (TaskCanceledException)
                {
                }
                finally
                {
                    Stopped();
                }
            }
        }
    }
}