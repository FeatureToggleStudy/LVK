using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public interface IFeatureToggle
    {
        [CanBeNull]
        bool? IsEnabled { get; }
    }
}