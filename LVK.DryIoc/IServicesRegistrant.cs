using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IServicesRegistrant
    {
        void Register([NotNull] IContainerBuilder containerBuilder);
        void Register([NotNull] IContainer container);
    }
}