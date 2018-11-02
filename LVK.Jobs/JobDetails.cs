using System;

using JetBrains.Annotations;

namespace LVK.Jobs
{
    internal class JobDetails : IJobDetails
    {
        [NotNull]
        private readonly string _Name;

        public JobDetails([NotNull] string name, int maximumProgress)
        {
            _Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public int CurrentProgress { get; }

        public int MaximumProgress { get; }

        public double PercentageCompleted { get; }

        public IJobDetails Clone() => throw new NotImplementedException();
    }
}