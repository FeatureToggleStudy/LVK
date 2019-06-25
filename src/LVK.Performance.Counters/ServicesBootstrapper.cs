using System;
using System.Collections.Generic;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.Performance.Counters
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<IPerformanceCounters, PerformanceCounters>(Reuse.Singleton);
            container.Register<IContainerFinalizer, SystemPerformanceCounters>();
        }
    }

    internal class SystemPerformanceCounters : IContainerFinalizer
    {
        [NotNull]
        private readonly IPerformanceCounters _PerformanceCounters;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public SystemPerformanceCounters([NotNull] IPerformanceCounters performanceCounters, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _PerformanceCounters = performanceCounters ?? throw new ArgumentNullException(nameof(performanceCounters));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public void Finalize(IContainer container)
        {
            List<IPerformanceCounter> gcCounters = new List<IPerformanceCounter>();
            for (int index = 0; index <= GC.MaxGeneration; index++)
                gcCounters.Add(_PerformanceCounters.GetByKey($"System.Memory.GC.{index}"));

            IPerformanceCounter gcCounter = _PerformanceCounters.GetByKey("System.Memory.GC");
            
            // ReSharper disable once ObjectCreationAsStatement
            new GCTracker(gcCounter, gcCounters, _ApplicationLifetimeManager);
        }
    }
}