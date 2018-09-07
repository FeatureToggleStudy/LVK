using System.Collections.Generic;

namespace LVK.AppCore.Console
{
    public class ConsoleApplicationEntryPointOptionsHelpTextProvider : IOptionsHelpTextProvider
    {
        public IEnumerable<(IEnumerable<string> paths, bool isConfiguration, string description)> GetHelpText()
        {
            yield return (new[] { "help" }, true, @"Show this help-text

example:
  --help");
        }
    }
}