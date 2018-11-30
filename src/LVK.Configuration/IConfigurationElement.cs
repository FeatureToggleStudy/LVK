using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationElement<out T>
    {
        [CanBeNull]
        T Value();
    }
}