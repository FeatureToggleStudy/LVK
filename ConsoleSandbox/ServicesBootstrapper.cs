using System;
using System.Threading.Tasks;

using DryIoc;

using LVK.AppCore;
using LVK.Core.Services;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>();
            container.Bootstrap<LVK.Conversion.ServicesBootstrapper>();
            container.Bootstrap<LVK.Reflection.ServicesBootstrapper>();

            container.Register<IBackgroundService, BackgroundService>();
            container.Register<IApplicationCommand, ListCommand>();

            // container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }

    internal class ListCommand : IApplicationCommand
    {
        public string[] CommandNames => new[] { "list" };

        public string Description => "Shows a list";

        public Task<int> TryExecute()
        {
            Console.WriteLine("list test");
            return Task.FromResult(0);
        }
    }
}