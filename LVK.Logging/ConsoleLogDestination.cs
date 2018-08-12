using System;

using JetBrains.Annotations;

namespace LVK.Logging
{
    [UsedImplicitly]
    internal class ConsoleLogDestination : TextLogDestination, ILogDestination
    {
        public ConsoleLogDestination([NotNull] ITextLogFormatter textLogFormatter, [NotNull] ILoggingOptions options)
            : base(textLogFormatter, options)
        {
        }

        protected override void OutputLineToLog(string line)
        {
            Console.Out.WriteLine(line);
        }
    }
}