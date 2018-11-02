using System;

using JetBrains.Annotations;

namespace LVK.Jobs
{
    internal class Job : IJob
    {
        [NotNull]
        private readonly JobMonitor _JobMonitor;

        private readonly int _MaximumProgress;

        [NotNull]
        private readonly JobDetails _Details;

        public Job([NotNull] JobMonitor jobMonitor, [NotNull] string name, int maximumProgress)
        {
            _JobMonitor = jobMonitor ?? throw new ArgumentNullException(nameof(jobMonitor));
            _Details = new JobDetails(name, maximumProgress);
        }

        public IJobDetails Details => _Details;

        public void Finish()
        {
            _JobMonitor.FinishJob(this);
        }

        public void ReportProgress(int currentProgress)
        {
            throw new NotImplementedException();
        }
    }
}