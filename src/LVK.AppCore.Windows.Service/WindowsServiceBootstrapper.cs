using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Console;
using LVK.DryIoc;

namespace LVK.AppCore.Windows.Service
{
    [PublicAPI]
    public static class WindowsServiceBootstrapper
    {
        [NotNull]
        public static async Task RunAsync<T>()
            where T: class, IServicesBootstrapper
            => await ConsoleAppBootstrapper.RunCommandAsync<ServicesBootstrapper<T>>();
    }
}