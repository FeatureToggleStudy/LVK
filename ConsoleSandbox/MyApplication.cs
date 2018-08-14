using System;
using System.Linq;
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

            var firstValue = (await _Api.GetAllAsync(cancellationToken)).FirstOrDefault();
            if (firstValue.Value != null)
            {
                await _Api.DeleteAsync(firstValue.Key, cancellationToken);
                await DumpValues("deleted", cancellationToken);
            }

            return 0;
        }

        [NotNull]
        private async Task DumpValues(string title, CancellationToken cancellationToken)
        {
            var values = await _Api.GetAllAsync(cancellationToken);
            Console.WriteLine($"{title}:");
            foreach (var value in values)
                Console.WriteLine($"   {value}");
        }
    }
}