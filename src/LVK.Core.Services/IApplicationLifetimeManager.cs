using System.Threading;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IApplicationLifetimeManager
    {
        CancellationToken GracefulTerminationCancellationToken { get; }

        void SignalGracefulTermination();
    }
}