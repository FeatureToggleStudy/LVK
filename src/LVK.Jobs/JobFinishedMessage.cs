using JetBrains.Annotations;

namespace LVK.Jobs
{
    [PublicAPI]
    public class JobFinishedMessage
    {
        [CanBeNull]
        public IJobDetails Job { get; set; }
    }
}