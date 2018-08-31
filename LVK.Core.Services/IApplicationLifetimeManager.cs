using System.Threading;

namespace LVK.Core.Services
{
    public interface IApplicationLifetimeManager
    {
        CancellationToken GracefulTerminationCancellationToken { get; }

        void SignalGracefulTermination();
    }
}