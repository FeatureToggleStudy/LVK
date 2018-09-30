using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues
{
    internal interface IWorkQueueRepositoryManager
    {
        void Enqueue([NotNull] string type, [NotNull] JObject  payload, DateTime whenToProcess, int retryCount);
        void Faulted([NotNull] string type, [NotNull] JObject  payload);

        [CanBeNull]
        IWorkQueueModel Dequeue();

        bool IsEnabled { get; }

        void Retry([NotNull] IWorkQueueModel model);
    }
}