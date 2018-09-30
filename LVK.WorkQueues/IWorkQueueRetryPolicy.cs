using System;

namespace LVK.WorkQueues
{
    internal interface IWorkQueueRetryPolicy
    {
        DateTime? WhenToRetry(int retryCount);
    }
}