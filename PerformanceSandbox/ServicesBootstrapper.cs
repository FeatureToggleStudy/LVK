using DryIoc;

using LVK.AppCore;
using LVK.DryIoc;

namespace PerformanceSandbox
{
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.ServicesBootstrapper>()
               .Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>()
               .Bootstrap<LVK.Commands.ServicesBootstrapper>()
               .Bootstrap<LVK.Configuration.ServicesBootstrapper>()
               .Bootstrap<LVK.Conversion.ServicesBootstrapper>()
               .Bootstrap<LVK.Core.Services.ServicesBootstrapper>()
               .Bootstrap<LVK.Data.ServicesBootstrapper>()
               .Bootstrap<LVK.Data.Sqlite.ServicesBootstrapper>()
               .Bootstrap<LVK.Json.ServicesBootstrapper>()
               .Bootstrap<LVK.Logging.ServicesBootstrapper>()
               .Bootstrap<LVK.Net.ServicesBootstrapper>()
               .Bootstrap<LVK.Net.Http.ServicesBootstrapper>()
               .Bootstrap<LVK.NodaTime.ServicesBootstrapper>()
               .Bootstrap<LVK.Persistence.ServicesBootstrapper>()
               .Bootstrap<LVK.Processes.ServicesBootstrapper>()
               .Bootstrap<LVK.Reflection.ServicesBootstrapper>()
               .Bootstrap<LVK.Security.Cryptography.ServicesBootstrapper>()
               .Bootstrap<LVK.WorkQueues.ServicesBootstrapper>()
               .Bootstrap<LVK.WorkQueues.FileBased.ServicesBootstrapper>()
               .Bootstrap<LVK.WorkQueues.Sqlite.ServicesBootstrapper>();

            container.Register<IApplicationEntryPoint, ApplicationEntryPoint>();
        }
    }
}