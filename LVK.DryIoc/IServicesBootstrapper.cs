using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    public interface IServicesBootstrapper
    {
        void Bootstrap([NotNull] IContainer container);
    }
}