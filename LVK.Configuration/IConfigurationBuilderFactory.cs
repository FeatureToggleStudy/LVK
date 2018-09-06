using JetBrains.Annotations;

namespace LVK.Configuration
{
    internal interface IConfigurationBuilderFactory
    {
        [NotNull] IConfigurationBuilder Create();
    }
}