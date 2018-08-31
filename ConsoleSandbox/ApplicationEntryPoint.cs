using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Configuration;
using LVK.Core;
using LVK.Logging;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly ILogger _Logger;

        public ApplicationEntryPoint([NotNull] IConfiguration configuration, [NotNull] ILogger<ApplicationEntryPoint> logger)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            int a = _Configuration["a"].Value<int>();
            int b = _Configuration["b"].Value<int>();

            _Logger.WriteLine($"{a} + {b} = {a + b}");
            return Task.FromResult(0).NotNull();
        }
    }
}