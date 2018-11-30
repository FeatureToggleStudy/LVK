using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.WorkQueues
{
    internal interface IWorkQueueRepositoryManager
    {
        [NotNull]
        Task EnqueueManyAsync([NotNull] IEnumerable<WorkQueueItem> items);

        [NotNull]
        Task FaultedAsync(WorkQueueItem item);

        [NotNull]
        Task<WorkQueueItem?> DequeueAsync();

        [NotNull]
        Task RetryAsync(WorkQueueItem item);

        bool IsEnabled { get; }
    }
}