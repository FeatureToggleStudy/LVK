using System.Diagnostics;

using JetBrains.Annotations;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class DebugLogDestination : TextLogDestination, ILogDestination
    {
        public DebugLogDestination([NotNull] ITextLogFormatter textLogFormatter)
            : base(textLogFormatter, new LoggingOptions { VerboseEnabled = true, DebugEnabled = true })
        {
        }

        protected override void OutputLineToLog(string line)
        {
            Debug.WriteLine(line);
        }
    }
}