using System.Collections.Generic;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;
using LVK.Core.Services;
using LVK.DryIoc;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace LVK.AppCore.Web
{
    [PublicAPI]
    public static class WebAppBootstrapper
    {
        [NotNull]
        public static async Task RunWebApiAsync<T>([NotNull] string[] arguments)
            where T: class, IServicesBootstrapper
        {
            var container = ContainerFactory.Bootstrap<WebApiApplicationBootstrapper<T>>();

            WebApiStartup.Container = container;

            var configuration = CreateMicrosoftConfiguration(container, arguments);

            var webHost = WebHost.CreateDefaultBuilder(arguments)
               .UseConfiguration(configuration)
               .UseStartup<WebApiStartup>()
               .UseKestrel().NotNull()
               .Build();

            var bsm = WebApiStartup.Container.Resolve<IBackgroundServicesManager>().NotNull();
            var alm = WebApiStartup.Container.Resolve<IApplicationLifetimeManager>().NotNull();
            bsm.StartBackgroundServices();
            try
            {
                await webHost.RunAsync().NotNull();
                alm.SignalGracefulTermination();
            }
            finally
            {
                await bsm.WaitForBackgroundServicesToStop();
            }
        }

        [NotNull]
        private static IConfiguration CreateMicrosoftConfiguration([NotNull] IContainer container, [NotNull, ItemNotNull] string[] arguments)
        {
            var builder = new ConfigurationBuilder().AddCommandLine(arguments).NotNull();

            var target = new ConfigurationConfiguratorTarget();
            foreach (var configurator in container.Resolve<IEnumerable<IConfigurationConfigurator>>().NotNull())
                configurator.Configure(target);

            foreach (var jsonFile in target.JsonFiles)
                builder.AddJsonFile(jsonFile.Filename, jsonFile.IsOptional);

            return builder.Build().NotNull();
        }
    }
}