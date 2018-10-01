using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
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
            var container = ContainerFactory.Create()
               .Bootstrap<WebApiApplicationBootstrapper<T>>();

            WebApiStartup<T>.Container = container;

            var configuration = CreateMicrosoftConfiguration(container, arguments);

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

        [NotNull]
        private static IConfiguration CreateMicrosoftConfiguration([NotNull] IContainer container, [NotNull, ItemNotNull] string[] arguments)
        {
            var builder = new ConfigurationBuilder().AddCommandLine(arguments);

            var target = new ConfigurationConfiguratorTarget();
            foreach (var configurator in container.Resolve<IEnumerable<IConfigurationConfigurator>>())
                configurator.Configure(target);

            foreach (var jsonFile in target.JsonFiles)
                builder.AddJsonFile(jsonFile.Filename, jsonFile.IsOptional);

            return builder.Build();
        }
    }
}