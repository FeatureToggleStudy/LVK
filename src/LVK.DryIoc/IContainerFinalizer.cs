using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IContainerFinalizer
    {
        void Finalize([NotNull] IContainer container);
    }
}