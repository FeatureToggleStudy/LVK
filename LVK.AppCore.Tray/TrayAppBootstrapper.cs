using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.AppCore.Tray
{
    [PublicAPI]
    public static class TrayAppBootstrapper
    {
        public static Task<int> RunAsync<T>()
            where T : class, IServicesBootstrapper
        {
            var container = new Container();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<T>();

            return container.New<TrayApp>().RunAsync();
        }
    }
}