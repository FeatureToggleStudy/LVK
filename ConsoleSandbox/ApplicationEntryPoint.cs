using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Configuration;
using LVK.Conversion;
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

        [NotNull]
        private readonly IValueConverter _ValueConverter;

        public ApplicationEntryPoint([NotNull] IConfiguration configuration, [NotNull] ILogger<ApplicationEntryPoint> logger, [NotNull] IValueConverter valueConverter)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ValueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            int a = _Configuration["a"].Value<int>();
            int b = _Configuration["b"].Value<int>();

            var c = a + b;
            _Logger.WriteLine($"{a} + {b} = {c}");

            _Logger.WriteLine(c.GetType().FullName);
            _Logger.WriteLine(_ValueConverter.Convert<int, char>(c).GetType().FullName);
            
            return Task.FromResult(0).NotNull();
        }
    }
}