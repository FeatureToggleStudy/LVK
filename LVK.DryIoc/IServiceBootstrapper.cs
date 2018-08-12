using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    public interface IServiceBootstrapper
    {
        void Bootstrap([NotNull] IContainer container);
    }
}