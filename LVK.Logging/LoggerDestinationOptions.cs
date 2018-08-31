using JetBrains.Annotations;

namespace LVK.Logging
{
    public class LoggerDestinationOptions
    {
        [NotNull]
        public static readonly LoggerDestinationOptions Default = new LoggerDestinationOptions();
        
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public bool Enabled { get; set; } = true;
    }
}