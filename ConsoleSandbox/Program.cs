using System.Threading.Tasks;

using LVK.AppCore.Console;

namespace ConsoleSandbox
{
    static class Program
    {
        static async Task<int> Main() => await ConsoleAppBootstrapper.RunAsync<ServicesBootstrapper>();
    }
}