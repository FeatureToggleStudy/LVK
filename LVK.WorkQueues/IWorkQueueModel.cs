using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public interface IWorkQueueModel
    {
        [NotNull]
        JObject Payload { get; }

        [NotNull]
        string Type { get; }

        int RetryCount { get; }

        DateTime WhenToProcess { get; }
    }
}