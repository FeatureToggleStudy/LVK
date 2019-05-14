using System;
using System.Threading;
using System.Threading.Tasks;

using LVK.Core.Services;

namespace ConsoleSandbox
{
    internal class FirstBackgroundService : IBackgroundService
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("First background service starting");
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("First");
                    await Task.Delay(5000, cancellationToken);
                }
            }
            finally
            {
                Console.WriteLine("First background service terminating");
            }
        }
    }
}