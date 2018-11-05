using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Configuration;
using LVK.Core;
using LVK.Data.Protection;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private IConfigurationElementWithDefault<string> _Configuration;

        public ApplicationEntryPoint([NotNull] IDataProtection dataProtection, [NotNull] IConfiguration configuration)
        {
            _Configuration = configuration["LoggingFilename"]
               .Element<string>()
               .WithDefault(() => "No filename");
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine(_Configuration.Value());
            return Task.FromResult(0).NotNull();
        }
    }
}