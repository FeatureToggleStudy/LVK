using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace WindowsServiceSandbox
{
    public class TestService : IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        public TestService([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _Logger.LogInformation("Here");
                await Task.Delay(1000, cancellationToken).NotNull();
            }
        }
    }
}