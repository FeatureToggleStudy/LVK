using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Configuration;
using LVK.Conversion;
using LVK.Core;
using LVK.Logging;
using LVK.Reflection;

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

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        public ApplicationEntryPoint(
            [NotNull] IConfiguration configuration, [NotNull] ILogger logger,
            [NotNull] IValueConverter valueConverter, [NotNull] ITypeHelper typeHelper)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ValueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
            _TypeHelper = typeHelper;
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            _Logger.Log(LogLevel.Trace, "ApplicationEntryPoint.Execute");
            int a = _Configuration["a"].Value<int>();
            int b = _Configuration["b"].Value<int>();

            var c = a + b;
            _Logger.WriteLine($"{a} + {b} = {c}");

            _Logger.WriteLine(_TypeHelper.NameOf(c.GetType()));
            _Logger.WriteLine(_TypeHelper.NameOf(_ValueConverter.Convert<int, char>(c).GetType()));

            return Task.FromResult(0).NotNull();
        }
    }
}