using System.Threading.Tasks;

using JetBrains.Annotations;

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
        public static Task RunWebApiAsync<T>([NotNull] string[] arguments)
            where T: class, IServicesBootstrapper
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(arguments)
               .Build();

            return WebHost.CreateDefaultBuilder(arguments)
               .UseConfiguration(configuration)
               .UseStartup<WebApiStartup<T>>()
               .UseKestrel()
               .Build()
               .RunAsync();
        }
    }
}