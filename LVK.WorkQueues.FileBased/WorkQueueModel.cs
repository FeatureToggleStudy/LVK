using System;

using JetBrains.Annotations;

namespace LVK.WorkQueues.FileBased
{
    internal class WorkQueueModel : WorkQueueModelBase, IWorkQueueModel
    {
        [UsedImplicitly]
        public int RetryCount { get; set; }

        [UsedImplicitly]
        public DateTime WhenToProcess { get; set; }
    }
}