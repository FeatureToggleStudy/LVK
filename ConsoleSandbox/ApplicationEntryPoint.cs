using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Configuration;
using LVK.Core;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        public ApplicationEntryPoint([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            int a = _Configuration["a"].Value<int>();
            int b = _Configuration["b"].Value<int>();

            Console.WriteLine($"{a} + {b} = {a + b}");
            return Task.FromResult(0).NotNull();
        }
    }
}