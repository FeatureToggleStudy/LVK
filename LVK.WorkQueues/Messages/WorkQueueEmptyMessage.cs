using JetBrains.Annotations;

namespace LVK.WorkQueues.Messages
{
    [PublicAPI]
    public class WorkQueueEmptyMessage
    {
        private WorkQueueEmptyMessage()
        {
        }

        public static readonly WorkQueueEmptyMessage Instance = new WorkQueueEmptyMessage();
    }
}