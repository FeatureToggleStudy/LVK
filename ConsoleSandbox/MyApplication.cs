using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class MyApplication : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ITestWebApi _Api;

        public MyApplication([NotNull] ITestWebApi api)
        {
            _Api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            await DumpValues("before", cancellationToken);
            await _Api.PostAsync("Lasse var her", cancellationToken);
            await DumpValues("after", cancellationToken);

            return 0;
        }

        [NotNull]
        private async Task DumpValues(string title, CancellationToken cancellationToken)
        {
            var values = await _Api.GetValuesAsync(cancellationToken);
            Console.WriteLine($"{title}:");
            foreach (var value in values)
                Console.WriteLine($"   {value}");
        }
    }
}