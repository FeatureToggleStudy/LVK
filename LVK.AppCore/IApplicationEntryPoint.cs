using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    public interface IApplicationEntryPoint
    {
        [NotNull]
        Task<int> Execute(CancellationToken cancellationToken);
    }
}