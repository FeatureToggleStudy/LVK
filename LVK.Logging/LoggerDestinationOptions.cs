namespace LVK.Logging
{
    public class LoggerDestinationOptions
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public bool Enabled { get; set; } = true;
    }
}