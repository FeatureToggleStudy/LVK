using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Console;

namespace ConsoleSandbox
{
    static class Program
    {
        static async Task<int> Main([NotNull] string[] args) => await ConsoleAppBootstrapper.RunCommandAsync<ServicesBootstrapper>(args);
    }
}