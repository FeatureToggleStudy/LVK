using System.Threading.Tasks;

using LVK.AppCore.Console;

namespace ConsoleSandbox
{
    static class Program
    {
        public static Task Main(string[] args)
        {
            return ConsoleAppBootstrapper.RunAsync<ServicesBootstrapper>();
        }
    }
}