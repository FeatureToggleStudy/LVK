using JetBrains.Annotations;

namespace LVK.Configuration.Layers
{
    internal interface IJsonFileConfigurationLayer
    {
        [NotNull]
        string GetJsonFilePath();

        bool IsOptional { get; }
    }
}