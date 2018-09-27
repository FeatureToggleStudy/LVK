using JetBrains.Annotations;

namespace LVK.Configuration.Layers
{
    internal interface IJsonFileConfigurationLayer
    {
        [NotNull]
        string GetJsonFilename();

        bool IsOptional { get; }
    }
}