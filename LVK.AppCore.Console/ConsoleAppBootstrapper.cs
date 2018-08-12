using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;
using LVK.Logging;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore.Console
{
    public static class ConsoleAppBootstrapper
    {
        [NotNull]
        public static Task<int> Execute<T>()
            where T: class, IServiceBootstrapper
        {
            IContainer container = new Container();
            container.Bootstrap<ServiceBootstrapper>()
               .Bootstrap<LVK.Logging.ServiceBootstrapper>()
               .Bootstrap<LVK.Core.Services.ServiceBootstrapper>()
               .Bootstrap<T>();

            container.Resolve<ILogger>().Warning("TEST");

            return container.Resolve<IConsoleApplicationEntryPoint>().NotNull().Execute();
        }
    }
}