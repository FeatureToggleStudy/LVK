using System;

using JetBrains.Annotations;

using LVK.Core;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues
{
    internal class WorkQueue : IWorkQueue
    {
        [NotNull]
        private readonly IWorkQueueRepositoryManager _WorkQueueRepositoryManager;

        public WorkQueue([NotNull] IWorkQueueRepositoryManager workQueueRepositoryManager)
        {
            _WorkQueueRepositoryManager = workQueueRepositoryManager;
        }

        public void Enqueue<T>(T item)
            where T: class
        {
            _WorkQueueRepositoryManager.Enqueue(typeof(T).FullName.NotNull(), JObject.FromObject(item).NotNull(), DateTime.Now, 0);
        }
    }
}