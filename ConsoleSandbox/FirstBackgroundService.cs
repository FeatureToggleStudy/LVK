using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.Performance.Counters;

namespace ConsoleSandbox
{
    internal class FirstBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly IPerformanceCounters _PerformanceCounters;

        [NotNull]
        private readonly IConfigurationElementWithDefault<string> _Configuration;

        public FirstBackgroundService([NotNull] IConfiguration configuration, [NotNull] IPerformanceCounters performanceCounters)
        {
            _PerformanceCounters = performanceCounters ?? throw new ArgumentNullException(nameof(performanceCounters));
            _Configuration = configuration.Element<string>("Name").WithDefault(() => "Unknown");
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var perfCounter = _PerformanceCounters.GetByKey("System.Memory.GC");
            Console.WriteLine("First background service starting");
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("GC: " + perfCounter.Value);

                    for (int index = 0; index < 10000; index++)
                        // ReSharper disable once ObjectCreationAsStatement
                        new string(' ', 80);
                    await Task.Delay(50, cancellationToken);
                }
            }
            finally
            {
                Console.WriteLine("First background service terminating");
            }
        }
    }
}