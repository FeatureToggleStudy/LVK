using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;

namespace LVK.AppCore.Tray
{
    [PublicAPI]
    public static class TrayAppBootstrapper
    {
        [NotNull]
        public static async Task<int> RunAsync<T>()
            where T : class, IServicesBootstrapper
        {
            await Task.Yield();
            
            var container = new Container();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<T>();

            return await container.New<TrayApp>().NotNull().RunAsync();
        }
    }
}