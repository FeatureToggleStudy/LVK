using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public interface IWorkQueueProcessor<in T>
        where T: class
    {
        [NotNull]
        Task Process([NotNull] T item, CancellationToken cancellationToken);
    }
}