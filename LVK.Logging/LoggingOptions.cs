using JetBrains.Annotations;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class LoggingOptions : ILoggingOptions
    {
        public bool VerboseEnabled { get; set; }

        public bool DebugEnabled { get; set; }
    }
}