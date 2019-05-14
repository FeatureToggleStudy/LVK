using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public interface IFeatureToggleWithDefaultValue
    {
        bool IsEnabled { get; }
    }
}