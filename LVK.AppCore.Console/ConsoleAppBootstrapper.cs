using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore.Console
{
    public static class ConsoleAppBootstrapper
    {
        [NotNull]
        public static Task<int> Execute<T>()
            where T: class, IServicesBootstrapper
        {
            IContainer container = new Container(rules => rules.WithTrackingDisposableTransients());
            container.Bootstrap<ServicesBootstrapper>()
               .Bootstrap<LVK.Core.Services.ServicesBootstrapper>()
               .Bootstrap<T>();

            return container.Resolve<IConsoleApplicationEntryPoint>().NotNull().Execute();
        }
    }
}