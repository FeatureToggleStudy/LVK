using System.Threading.Tasks;

using LVK.AppCore.Tray;

namespace ConsoleSandbox
{
    static class Program
    {
        static async Task<int> Main() => await TrayAppBootstrapper.RunAsync<ServicesBootstrapper>();
    }
}