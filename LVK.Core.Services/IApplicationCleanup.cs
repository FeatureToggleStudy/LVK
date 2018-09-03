using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IApplicationCleanup
    {
        [NotNull]
        Task Cleanup(CancellationToken cancellationToken);
    }
}