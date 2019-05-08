using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.Logging;

namespace WinFormsSandbox
{
    internal class MyBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IConfigurationElementWithDefault<ConfigurationElement> _Configuration;

        public MyBackgroundService([NotNull] ILogger logger, [NotNull] IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Configuration = configuration.Element<ConfigurationElement>("Test").WithDefault(new ConfigurationElement());
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _Logger.LogInformation(_Configuration.Value().Value);
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}