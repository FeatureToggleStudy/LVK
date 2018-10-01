using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.DryIoc;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LVK.AppCore.Web
{
    [PublicAPI]
    public static class WebAppBootstrapper
    {
        [NotNull]
        public static async Task RunWebApiAsync<T>([NotNull] string[] arguments)
            where T: class, IServicesBootstrapper
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(arguments)
               .Build();

            var webHost = WebHost.CreateDefaultBuilder(arguments)
               .UseConfiguration(configuration)
               .UseStartup<WebApiStartup<T>>()
               .UseKestrel()
               .Build();

            var bsm = WebApiStartup<T>.Container.Resolve<IBackgroundServicesManager>();
            var alm = WebApiStartup<T>.Container.Resolve<IApplicationLifetimeManager>();
            bsm.StartBackgroundServices();
            try
            {
                await webHost.RunAsync();
                alm.SignalGracefulTermination();
            }
            finally
            {
                await bsm.WaitForBackgroundServicesToStop();
            }
        }
    }
}