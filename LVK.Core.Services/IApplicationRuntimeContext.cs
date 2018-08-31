using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    public interface IApplicationRuntimeContext
    {
        [NotNull]
        Task Start(CancellationToken cancellationToken);

        [NotNull]
        Task Stop(CancellationToken cancellationToken);
    }
}