using JetBrains.Annotations;

namespace LVK.Jobs
{
    [PublicAPI]
    public interface IJobDetails
    {
        [NotNull]
        string Name { get; }

        int CurrentProgress { get; }

        int MaximumProgress { get; }

        double PercentageCompleted { get; }
    }
}