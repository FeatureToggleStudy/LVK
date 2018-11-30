using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Mvvm
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<IMvvmContext, MvvmContext>(Reuse.Singleton);
        }
    }
}