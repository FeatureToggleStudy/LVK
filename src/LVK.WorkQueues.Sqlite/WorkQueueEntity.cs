using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.WorkQueues.Sqlite
{
    [UsedImplicitly]
    internal class WorkQueueEntity
    {
        public int Id { get; set; }

        public string Type
        {
            get;
            [UsedImplicitly]
            set;
        }

        public string Payload
        {
            get;
            [UsedImplicitly]
            set;
        }

        public int RetryCount
        {
            get;
            [UsedImplicitly]
            set;
        }

        public DateTime WhenToProcess
        {
            get;
            [UsedImplicitly]
            set;
        }

        public WorkQueueItem ToItem()
        {
            assume(Type != null);
            assume(Payload != null);

            JObject obj = JObject.Parse(Payload);
            assume(obj != null);

            return new WorkQueueItem(Type, obj, WhenToProcess, RetryCount);
        }
    }
}