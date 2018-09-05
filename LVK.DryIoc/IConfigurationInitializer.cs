using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public interface IConfigurationInitializer
    {
        void Initialize([NotNull] IContainer container);
    }
}