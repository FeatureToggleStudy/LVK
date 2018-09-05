using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    internal interface IContainerInitializer
    {
        void Initialize([NotNull] Container container);
    }
}