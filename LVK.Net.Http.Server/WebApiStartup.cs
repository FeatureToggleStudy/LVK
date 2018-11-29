using System;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.Net.Http.Server
{
    internal class WebApiStartup
    {
        [CanBeNull]
        // ReSharper disable once StaticMemberInGenericType
        public static IContainer Container { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                mvc = mvc.AddApplicationPart(assembly);

            Container = Container.WithDependencyInjectionAdapter(services);
            return Container.Resolve<IServiceProvider>();
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