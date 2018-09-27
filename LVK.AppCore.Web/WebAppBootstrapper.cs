using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using JetBrains.Annotations;

using LVK.Configuration.Layers;
using LVK.Core.Services;
using LVK.DryIoc;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.AppCore.Web
{
    [PublicAPI]
    public static class WebAppBootstrapper
    {
        public static Task RunWebApiAsync<T>(Type type, [NotNull] string[] arguments)
            where T: class, IServicesBootstrapper
        {
            return WebHost.CreateDefaultBuilder(arguments)
               .UseStartup<WebApiStartup<T>>()
               .Build()
               .RunAsync();
        }
    }

    public class WebApiStartup<T>
        where T: class, IServicesBootstrapper
    {
        public WebApiStartup(IHostingEnvironment env)
        {
            Container = new Container()
               .Bootstrap<WebApiApplicationBootstrapper<T>>();

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", true);
            foreach (var layerProvider in Container.Resolve<IEnumerable<IConfigurationLayersProvider>>())
                foreach (var layer in layerProvider.Provide().OfType<IJsonFileConfigurationLayer>())
                    builder.AddJsonFile(layer.GetJsonFilename(), layer.IsOptional);

            Configuration = builder.Build();
        }

        public IContainer Container { get; private set; }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc()
               .AddControllersAsServices()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                mvc = mvc.AddApplicationPart(assembly);

            Container = Container.WithDependencyInjectionAdapter(services);
            return Container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}