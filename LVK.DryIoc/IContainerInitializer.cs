using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IContainerInitializer
    {
        void Initialize();
    }
}