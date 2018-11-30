using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public static class WorkQueueExtensions
    {
        [NotNull]
        public static Task EnqueueAsync([NotNull] this IWorkQueue workQueue, [NotNull] object item, [CanBeNull] DateTime? whenToProcess = null)
        {
            if (workQueue is null)
                throw new ArgumentNullException(nameof(workQueue));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return workQueue.EnqueueManyAsync(new[] { item }, whenToProcess);
        }
    }
}