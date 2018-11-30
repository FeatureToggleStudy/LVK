using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public struct WorkQueueItem
    {
        public WorkQueueItem([NotNull] string type, [NotNull] JObject payload, DateTime whenToProcess, int retryCount)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            WhenToProcess = whenToProcess;
            RetryCount = retryCount;
        }

        [NotNull]
        public string Type { get; }

        [NotNull]
        public JObject Payload { get; }

        public DateTime WhenToProcess { get; }

        public int RetryCount { get; }
    }
}