using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public interface IWorkQueue
    {
        [NotNull]
        Task EnqueueManyAsync([NotNull] IEnumerable<object> items, [CanBeNull] DateTime? whenToProcess = null);
    }
}