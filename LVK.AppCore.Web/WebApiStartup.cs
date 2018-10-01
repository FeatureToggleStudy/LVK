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
    internal class WebApiStartup<T>
        where T: class, IServicesBootstrapper
    {
        [NotNull]
        // ReSharper disable once StaticMemberInGenericType
        public static IContainer Container { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                mvc = mvc.AddApplicationPart(assembly);

            Container = Container.WithDependencyInjectionAdapter(services);
            return new WrapServiceProvider(Container.Resolve<IServiceProvider>());
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