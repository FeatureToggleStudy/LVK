using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public interface IWorkQueueRepository
    {
        [NotNull]
        Task EnqueueManyAsync([NotNull] IEnumerable<WorkQueueItem> items);

        [NotNull]
        Task FaultedAsync(WorkQueueItem item);

        [NotNull]
        Task<WorkQueueItem?> DequeueAsync();

        bool IsEnabled { get; }
    }
}