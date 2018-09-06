using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IServicesBootstrapper
    {
        void Bootstrap([NotNull] IContainer container);
    }
}