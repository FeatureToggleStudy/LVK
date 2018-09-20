using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationElement<T>
    {
        [NotNull]
        T Value();
    }
}