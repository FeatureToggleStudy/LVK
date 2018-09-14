using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace ConsoleSandbox
{
    internal class BackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        public BackgroundService([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            for (int index = 0; index < 3; index++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                _Logger.WriteLine("here");
                await Task.Delay(1000, cancellationToken).NotNull();
            }

            throw new Exception("Test");
        }
    }
}