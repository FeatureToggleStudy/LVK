using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using Microsoft.Extensions.Logging;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore.Console
{
    public static class ConsoleAppBootstrapper
    {
        [NotNull]
        public static Task<int> Execute<T>()
            where T: class, IServiceBootstrapper
        {
            IContainer container = new Container(rules => rules.WithTrackingDisposableTransients());
            container.Bootstrap<ServiceBootstrapper>()
               // .Bootstrap<LVK.Logging.ServiceBootstrapper>()
               .Bootstrap<LVK.Core.Services.ServiceBootstrapper>()
               .Bootstrap<T>();

            // container.UseInstance<ILoggerFactory>(new LoggerFactory());

            // MethodInfo loggerFactoryMethod = typeof(LoggerFactoryExtensions).GetMethod("CreateLogger", new[] { typeof(ILoggerFactory) });
            //
            // container.Register(typeof(ILogger<>), made: Made.Of(loggerFactoryMethod));
            
            return container.Resolve<IConsoleApplicationEntryPoint>().NotNull().Execute();
        }
    }
}