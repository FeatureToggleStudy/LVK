using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;

namespace LVK.AppCore.Windows.Tray
{
    [PublicAPI]
    public static class TrayAppBootstrapper
    {
        [NotNull]
        public static async Task<int> RunAsync<T>()
            where T : class, IServicesBootstrapper
        {
            await Task.Yield();

            var container = ContainerFactory.Bootstrap<ServicesBootstrapper, T>();

            return await container.New<TrayApp>().NotNull().RunAsync();
        }
    }
}