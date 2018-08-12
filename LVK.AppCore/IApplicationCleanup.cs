using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    public interface IApplicationCleanup
    {
        [NotNull]
        Task Cleanup(bool wasCancelledByUser, CancellationToken cancellationToken);
    }
}