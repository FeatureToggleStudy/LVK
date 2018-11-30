using JetBrains.Annotations;

namespace LVK.Jobs
{
    [PublicAPI]
    public interface IJob
    {
        [NotNull]
        IJobDetails Details { get; }

        void Finish();
        void ReportProgress(int current, int total);
    }
}