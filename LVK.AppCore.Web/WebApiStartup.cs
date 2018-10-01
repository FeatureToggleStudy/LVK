using System;
using System.Collections.Generic;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.DryIoc;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace LVK.AppCore.Web
{
    public class WebApiStartup<T>
        where T: class, IServicesBootstrapper
    {
        [NotNull]
        private readonly IContainer _Container;

        public WebApiStartup()
        {
            _Container = new Container()
               .Bootstrap<WebApiApplicationBootstrapper<T>>();

            AddJsonConfigurationFiles();
        }

        private void AddJsonConfigurationFiles()
        {
            var builder = new ConfigurationBuilder();
            var target = new ConfigurationConfiguratorTarget();
            foreach (var configurator in _Container.Resolve<IEnumerable<IConfigurationConfigurator>>())
                configurator.Configure(target);

            foreach (var configurationFile in target.JsonFiles)
                builder.AddJsonFile(configurationFile.Filename, configurationFile.IsOptional);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                mvc = mvc.AddApplicationPart(assembly);

            return _Container.WithDependencyInjectionAdapter(services).Resolve<IServiceProvider>();
        }

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