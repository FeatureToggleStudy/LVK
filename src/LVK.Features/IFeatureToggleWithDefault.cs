using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public interface IFeatureToggleWithDefault
    {
        bool IsEnabled { get; }
    }
}