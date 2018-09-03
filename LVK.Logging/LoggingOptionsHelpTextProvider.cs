using System.Collections.Generic;

using LVK.AppCore;

namespace LVK.Logging
{
    public class LoggingOptionsHelpTextProvider : IOptionsHelpTextProvider
    {
        public IEnumerable<(IEnumerable<string> paths, string description)> GetHelpText()
        {
            yield return (new[] { "Logging/Destinations/Console/LogLevel" }, @"Minimum log-level for Console, one of
  Trace
  Debug
  Verbose
  Information
  Warning
  Error

example:
  --Logging/Destinations/Console/LogLevel=Warning");

            yield return (new[] { "Logging/Destinations/Console/Enabled" },
                @"true = logging to console enabled (default)
false = logging to console disabled

example:
  --Logging/Destinations/Console/Enabled=false");
        }
    }
}