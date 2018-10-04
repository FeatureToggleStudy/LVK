using System.Threading.Tasks;

using LVK.AppCore.Console;

namespace PerformanceSandbox
{
    static class Program
    {
        static async Task Main()
        {
            await ConsoleAppBootstrapper.RunAsync<ServicesBootstrapper>();
        }
    }
}
