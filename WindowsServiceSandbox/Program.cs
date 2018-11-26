using System.Threading.Tasks;

using LVK.AppCore.Windows.Service;

namespace WindowsServiceSandbox
{
    internal static class Program
    {
        public static async Task Main() => await WindowsServiceBootstrapper.RunAsync<ServicesBootstrapper>();
    }
}