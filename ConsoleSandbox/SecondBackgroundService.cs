using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.Core.Services;

namespace ConsoleSandbox
{
    internal class SecondBackgroundService : IBackgroundService
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("Second background service starting");
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Second");
                    await Task.Delay(5000, cancellationToken);
                }
            }
            finally
            {
                Console.WriteLine("Second background service terminating");
            }
        }
    }
}