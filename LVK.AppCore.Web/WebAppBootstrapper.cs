using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.AppCore.Web
{
    [PublicAPI]
    public static class WebAppBootstrapper
    {
        public static Task RunWebApiAsync<T>()
            where T: class, IServicesBootstrapper
        {
            var container = ContainerFactory
        }
    }
}