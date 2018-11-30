using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Jobs
{
    [PublicAPI]
    public interface IJobMonitor
    {
        List<IJob> GetSnapshot();
        IJob StartJob([NotNull] string name);
    }
}