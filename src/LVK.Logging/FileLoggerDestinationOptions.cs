using JetBrains.Annotations;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class FileLoggerDestinationOptions : LoggerDestinationOptions
    {
        [NotNull]
        public string DirectoryPath { get; set; } = string.Empty;

        [NotNull]
        public string Filename { get; set; } = string.Empty;
    }
}