using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Security.Cryptography
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Register<IHasher, Hasher>();
        }
    }
}