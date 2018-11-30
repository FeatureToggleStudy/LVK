using System.Collections.Generic;

using LVK.Core.Services;

namespace LVK.Logging
{
    internal class LoggingOptionsHelpTextProvider : IOptionsHelpTextProvider
    {
        public IEnumerable<(IEnumerable<string> paths, bool isConfiguration, string description)> GetHelpText()
        {
            yield return (new[] { "Logging/Destinations/Console/LogLevel" }, true, @"Minimum log-level for Console, one of
  Trace
  Debug
  Verbose
  Information
  Warning
  Error

example:
  --Logging/Destinations/Console/LogLevel=Warning

example appsettings.json:
  {
    ""Logging"": {
      ""Destinations"": {
        ""Console"": {
          ""LogLevel"": ""Warning""
        }
      }
    }
  }");

            yield return (new[] { "Logging/Destinations/Console/Enabled" }, true,
                @"true = logging to console enabled (default)
false = logging to console disabled

example:
  --Logging/Destinations/Console/Enabled=false

example appsettings.json:
  {
    ""Logging"": {
      ""Destinations"": {
        ""Console"": {
          ""Enabled"": false
        }
      }
    }
  }");
        }
    }
}