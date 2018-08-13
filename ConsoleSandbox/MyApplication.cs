using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;

using Microsoft.Extensions.Logging;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyApplication : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ILogger<MyApplication> _Logger;

        [NotNull]
        private readonly ITestWebApi _Api;

        public MyApplication([NotNull] ILogger<MyApplication> logger, [NotNull] ITestWebApi api)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            foreach (string value in await _Api.GetValuesAsync(cancellationToken))
                Console.WriteLine(value);

            return 0;
        }
    }
}