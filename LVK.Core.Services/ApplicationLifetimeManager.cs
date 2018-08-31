using System;
using System.Threading;

namespace LVK.Core.Services
{
    internal class ApplicationLifetimeManager : IApplicationLifetimeManager
    {
        private readonly CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public CancellationToken GracefulTerminationCancellationToken => _CancellationTokenSource.Token;

        public void SignalGracefulTermination()
        {
            _CancellationTokenSource.Cancel();
        }
    }
}