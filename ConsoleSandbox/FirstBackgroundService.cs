using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;

namespace ConsoleSandbox
{
    internal class FirstBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly IConfigurationElementWithDefault<string> _Configuration;

        public FirstBackgroundService([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration.Element<string>("Name").WithDefault(() => "Unknown");
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("First background service starting");
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("First in " + _Configuration.Value());
                    await Task.Delay(5000, cancellationToken);
                }
            }
            finally
            {
                Console.WriteLine("First background service terminating");
            }
        }
    }
}