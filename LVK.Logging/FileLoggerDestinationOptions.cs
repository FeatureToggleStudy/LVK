using JetBrains.Annotations;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class FileLoggerDestinationOptions : LoggerDestinationOptions
    {
        [NotNull]
        public string Path { get; set; } = string.Empty;

        [NotNull]
        public string Filename { get; set; } = string.Empty;
    }
}