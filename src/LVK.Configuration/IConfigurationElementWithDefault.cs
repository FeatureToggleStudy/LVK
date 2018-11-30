using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationElementWithDefault<out T>
    {
        [NotNull]
        T Value();
    }
}