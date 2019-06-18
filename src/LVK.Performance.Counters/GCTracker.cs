using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Core.Services;

namespace LVK.Performance.Counters
{
    internal class GCTracker
    {
        [NotNull]
        private readonly IPerformanceCounter _GcCounter;

        [NotNull, ItemNotNull]
        private readonly List<IPerformanceCounter> _GcCounters;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly List<int> _PreviousValues = new List<int>(); 

        public GCTracker([NotNull] IPerformanceCounter gcCounter, [NotNull] List<IPerformanceCounter> gcCounters, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _GcCounter = gcCounter ?? throw new ArgumentNullException(nameof(gcCounter));
            _GcCounters = gcCounters ?? throw new ArgumentNullException(nameof(gcCounters));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            for (int index = 0; index < _GcCounters.Count; index++)
                _PreviousValues.Add(GC.CollectionCount(index));
        }

        ~GCTracker()
        {
            _GcCounter.Increment();
            for (int index = 0; index < _PreviousValues.Count; index++)
            {
                int value = GC.CollectionCount(index);
                if (value > _PreviousValues[index])
                {
                    _PreviousValues[index] = value;
                    _GcCounters[index].Increment();
                }
            }

            if (!_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
                GC.ReRegisterForFinalize(this);
        }
    }
}