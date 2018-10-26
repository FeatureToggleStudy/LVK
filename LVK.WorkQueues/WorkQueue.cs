using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues
{
    internal class WorkQueue : IWorkQueue
    {
        [NotNull]
        private readonly IWorkQueueRepositoryManager _WorkQueueRepositoryManager;

        [NotNull]
        private readonly IBus _Bus;

        public WorkQueue([NotNull] IWorkQueueRepositoryManager workQueueRepositoryManager, [NotNull] IBus bus)
        {
            _WorkQueueRepositoryManager = workQueueRepositoryManager;
            _Bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task EnqueueManyAsync(IEnumerable<object> items, DateTime? whenToProcess)
        {
            IEnumerable<WorkQueueItem> workQueueItems =
                from item in items
                where item != null
                let obj = JObject.FromObject(item)
                where obj != null
                select new WorkQueueItem(item.NotNull().GetType().FullName.NotNull(), obj.NotNull(), whenToProcess ?? DateTime.Now, 0);

            await _WorkQueueRepositoryManager.EnqueueManyAsync(workQueueItems);

            _Bus.Publish(new WorkQueueItemAddedMessage());
        }
    }
}