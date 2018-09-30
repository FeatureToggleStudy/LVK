using System;

namespace LVK.WorkQueues
{
    internal class WorkQueueRetryPolicy : IWorkQueueRetryPolicy
    {
        public DateTime? WhenToRetry(int retryCount)
        {
            switch (retryCount)
            {
                case int i when i <= 3:
                    return DateTime.Now.AddSeconds(10);
                
                case int i when i <= 5:
                    return DateTime.Now.AddMinutes(1);
                
                case int i when i <= 10:
                    return DateTime.Now.AddMinutes(5);
                
                case int i when i <= 20:
                    return DateTime.Now.AddMinutes(30);
                
                default:
                    return null;
            }
        }
    }
}