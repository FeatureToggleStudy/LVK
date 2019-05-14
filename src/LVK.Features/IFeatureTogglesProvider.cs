using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public interface IFeatureTogglesProvider
    {
        [CanBeNull]
        bool? IsEnabled([NotNull] string key);
    }
}