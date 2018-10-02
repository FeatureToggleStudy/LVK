using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues.Sqlite
{
    [UsedImplicitly]
    internal class WorkQueueEntity
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Payload { get; set; }

        public int RetryCount { get; set; }

        public DateTime WhenToProcess { get; set; }

        public WorkQueueItem ToItem()
        {
            return new WorkQueueItem(Type, JObject.Parse(Payload), WhenToProcess, RetryCount);
        }
    }
}