using JetBrains.Annotations;

namespace LVK.Jobs
{
    [PublicAPI]
    public class JobProgressChangedMessage
    {
        [CanBeNull]
        public IJobDetails Job { get; set; }
    }
}