using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    public interface IApplicationCleanup
    {
        [NotNull]
        Task Cleanup(CancellationToken cancellationToken);
    }
}