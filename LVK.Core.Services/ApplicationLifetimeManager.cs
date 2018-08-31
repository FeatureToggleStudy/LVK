using System.Threading;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    internal class ApplicationLifetimeManager : IApplicationLifetimeManager
    {
        [NotNull]
        private readonly CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public CancellationToken GracefulTerminationCancellationToken => _CancellationTokenSource.Token;

        public void SignalGracefulTermination()
        {
            _CancellationTokenSource.Cancel();
        }
    }
}