using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

namespace ConsoleSandbox
{
    internal class CounterService : IBackgroundService
    {
        [NotNull]
        private readonly ICounterHolder _CounterHolder;

        public CounterService([NotNull] ICounterHolder counterHolder)
        {
            _CounterHolder = counterHolder ?? throw new ArgumentNullException(nameof(counterHolder));
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken).NotNull();
                _CounterHolder.CurrentValue++;
            }
        }
    }
}