using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public interface IFeatureToggle
    {
        bool? IsEnabled { get; }
    }
}