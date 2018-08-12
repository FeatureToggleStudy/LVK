using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.DryIoc;

namespace ConsoleSandbox
{
    [UsedImplicitly]
    internal class CompositionRoot : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<IApplicationEntryPoint, MyApplication>();
        }
    }
}