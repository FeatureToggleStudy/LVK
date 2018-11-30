using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationConfigurator
    {
        void Configure([NotNull] IConfigurationBuilder configurationBuilder);
    }
}