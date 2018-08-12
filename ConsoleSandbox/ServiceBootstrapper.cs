using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.DryIoc;

using Microsoft.Extensions.Logging;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<IApplicationEntryPoint, MyApplication>();
        }
    }
}