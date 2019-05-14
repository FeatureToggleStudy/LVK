using JetBrains.Annotations;

namespace LVK.Features
{
    [PublicAPI]
    public interface IFeatureToggles
    {
        [NotNull]
        IFeatureToggle GetByKey([NotNull] string key);
    }
}